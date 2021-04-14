using Microsoft.Extensions.Configuration;
using SX.Common.Shared.Contracts;

namespace SX.Common.Infrastructure.Services
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly IConfiguration _configuration;

        public SettingsProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetSettings(string name)
        {
            return _configuration[name];
        }

        public T GetSettings<T>(string name)
        {
            return _configuration.GetValue<T>(name);
        }
    }
}
