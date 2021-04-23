using DemoTools.Core.Proxy.Services;
using DemoTools.Records.Domain.Contracts;
using DemoTools.Records.Infrastructure.Data;
using DemoTools.Records.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SX.Common.Api.Classes;
using SX.Common.Api.Filters;
using SX.Common.Api.Services;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoTools.Records.Api
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

            services.AddDbContext<RecordsDbContext>(options =>
                options.UseNpgsql(Configuration["ConnectionStrings:DemoToolsRecordsDBConnection"]));

            services.AddScoped<SX.Common.Shared.Contracts.ILogger, ConsoleLogger>();
            services.AddScoped<IAuthenticationProvider, AuthenticationProxy>();
            services.AddScoped<ITokenProvider, ApiTokenProvider>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ICacheProvider>(x => new MemoryCacheProvider());
            services.AddScoped<ITodoService, TodoService>();

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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoTools.Records.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DemoTools.Records.Api v1"));
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
