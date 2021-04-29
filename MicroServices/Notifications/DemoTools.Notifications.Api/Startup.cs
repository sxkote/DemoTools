using DemoTools.Core.Proxy.Services;
using DemoTools.Notifications.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SX.Common.Api.Classes;
using SX.Common.Api.Filters;
using SX.Common.Api.Services;
using SX.Common.Infrastructure.Models;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Events;
using SX.Common.Shared.Interfaces;

namespace DemoTools.Notifications.Api
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

            // Options & Configurations
            services.Configure<RabbitMQConfig>(Configuration.GetSection(RabbitMQConfig.CONFIG_NAME));

            // Hosted Services
            services.AddHostedService<RabbitMQBackgroundService>();

            // Services
            services.AddScoped<ILogger, ConsoleLogger>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ICacheProvider>(x => new MemoryCacheProvider());
            services.AddScoped<ITokenProvider, ApiTokenProvider>();
            services.AddScoped<IAuthenticationProvider, AuthenticationProxy>();

            // Domain-Events
            services.AddScoped<IDomainEventHandler<EventBusMessageReceivedEvent>, NotificationService>();

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoTools.Notifications.Api", Version = "v1" });
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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoTools.Notifications.Api v1"));
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
