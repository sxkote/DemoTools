using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Services;
using DemoTools.Modules.Main.Infrastructure.Data;
using DemoTools.Modules.Main.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SX.Common.Shared.Contracts;
using System.Reflection;

namespace DemoTools.Modules.Main.Api
{
    public class MainModuleConfiguration
    {
        static public void Config(IConfiguration configuration, IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            services.AddDbContext<MainDbContext>(options =>
                options.UseNpgsql(configuration["ConnectionStrings:DemoDBConnection"]));

            services.AddScoped<IAuthenticationProvider, AuthenticationService>();

            services.AddScoped<IMainUnitOfWork, MainUnitOfWork>();
            services.AddScoped<ITodoService, TodoService>();

            mvcBuilder.AddApplicationPart(Assembly.GetAssembly(typeof(MainModuleConfiguration)));
        }
    }
}
