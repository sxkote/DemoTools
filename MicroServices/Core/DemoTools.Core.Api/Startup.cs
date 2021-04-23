using DemoTools.Core.Domain.Contracts;
using DemoTools.Core.Infrastructure.Data;
using DemoTools.Core.Infrastructure.Services;
using DemoTools.Core.Shared.DomainEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SX.Common.Api.Classes;
using SX.Common.Api.Filters;
using SX.Common.Api.Services;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;

namespace DemoTools.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddDbContext<CoreDbContext>(options =>
               options.UseNpgsql(Configuration["ConnectionStrings:DemoToolsAuthorizationDBConnection"]));

            // Services
            services.AddScoped<ILogger, ConsoleLogger>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ICacheProvider>(x => new MemoryCacheProvider());
            services.AddScoped<ITokenProvider, ApiTokenProvider>();
            services.AddScoped<IAuthenticationProvider, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();

            // Domain-Events
            services.AddScoped<IDomainEventHandler<RegistrationInitDomainEvent>, NotificationService>();
            services.AddScoped<IDomainEventHandler<PasswordRecoveryInitDomainEvent>, NotificationService>();
            services.AddScoped<IDomainEventHandler<PasswordChangedDomainEvent>, NotificationService>();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });


            services
                .AddControllers(options =>
                {
                    // добавляем фильтр для обработки исключений
                    options.Filters.Add(new ApiExceptionFilter());
                })
                .AddJsonOptions(options =>
                {
                    // определяем CamelCase для свойств JSON объектов
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = TokenAuthenticationHandler.SCHEMA;
            })
            .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(TokenAuthenticationHandler.SCHEMA, op => { });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoTools.Authorization.Api", Version = "v1" });
            });

            var resolver = new ServiceResolver(services.BuildServiceProvider());
            AppSettings.Global.DependencyResolver = resolver;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoTools.Authorization.Api v1"));
            }

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
