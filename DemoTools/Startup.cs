using DemoTools.Modules.Main.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SX.Common.Api.Classes;
using SX.Common.Api.Services;
using SX.Common.Infrastructure.Services;
using SX.Common.Shared.Contracts;
using VueCliMiddleware;

namespace DemoTools
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
            services.AddScoped<ITokenProvider, ApiTokenProvider>();
            services.AddSingleton<ICacheProvider>(x => new MemoryCacheProvider());


            var mvcBuilder = services.AddControllersWithViews()
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = TokenAuthenticationHandler.SCHEMA;
            })
            .AddScheme<TokenAuthenticationOptions, TokenAuthenticationHandler>(TokenAuthenticationHandler.SCHEMA, op => { });

            services.AddHttpContextAccessor();


            MainModuleConfiguration.Config(Configuration, services, mvcBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //app.UseStaticFiles();

            app.UseRouting();

            app.UseSpaStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp/";
                spa.UseVueCli(npmScript: "serve", forceKill: true);

                //if (env.IsDevelopment())
                //    spa.Options.SourcePath = "ClientApp/";
                //else
                //    spa.Options.SourcePath = "dist";

                //if (env.IsDevelopment())
                //{
                //    spa.UseVueCli(npmScript: "serve", forceKill: true);
                //}
            });
        }
    }
}
