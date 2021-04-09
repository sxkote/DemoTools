using DemoTools.Modules.Main.Domain.Contracts;
using DemoTools.Modules.Main.Domain.Contracts.Services;
using DemoTools.Modules.Main.Domain.Services;
using DemoTools.Modules.Main.Infrastructure.Data;
using DemoTools.Modules.Main.Infrastructure.Services;
using DemoTools.Modules.Main.Shared.DomainEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using System.Reflection;

namespace DemoTools.Modules.Main.Api
{
    public class MainModuleConfiguration
    {
        static public void Config(IConfiguration configuration, IServiceCollection services, IMvcBuilder mvcBuilder)
        {
            // Db-Context
            services.AddDbContext<MainDbContext>(options =>
                options.UseNpgsql(configuration["ConnectionStrings:DemoDBConnection"]));

            // Services
            services.AddScoped<ICaptchaService, CaptchaService>();
            services.AddScoped<IAuthenticationProvider, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();

            services.AddScoped<IMainUnitOfWork, MainUnitOfWork>();
            services.AddScoped<ITodoService, TodoService>();

            // Domain-Events
            services.AddScoped<IDomainEventHandler<RegistrationInitDomainEvent>, MainNotificationService>();
            services.AddScoped<IDomainEventHandler<PasswordRecoveryInitDomainEvent>, MainNotificationService>();
            services.AddScoped<IDomainEventHandler<PasswordChangedDomainEvent>, MainNotificationService>();

            mvcBuilder.AddApplicationPart(Assembly.GetAssembly(typeof(MainModuleConfiguration)));
        }
    }
}
