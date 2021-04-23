using DemoTools.Core.Shared.Models;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DemoTools.Core.Proxy.Services
{
    public class AuthenticationProxy : IAuthenticationProvider
    {
        public const bool CONFIGURE_AWAIT = false;
        public const int TIMEOUT_IN_MINUTES = 3;
        public const string CORE_API_URL_CONFIG_NAME = "CoreApiURL";

        private readonly ISettingsProvider _settingsProvider;

        private HttpClient _client;

        private HttpClient Client
        {
            get
            {
                if (_client == null)
                    _client = this.BuildClient();

                return _client;
            }
        }

        public AuthenticationProxy(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
        }

        protected HttpClient BuildClient()
        {
            var clientHandler = new HttpClientHandler()
            {
                CookieContainer = new CookieContainer(),
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            };

            var client = new HttpClient(clientHandler)
            {
                Timeout = TimeSpan.FromMinutes(TIMEOUT_IN_MINUTES),
                BaseAddress = new Uri(_settingsProvider.GetSettings(CORE_API_URL_CONFIG_NAME))
            };

            return client;
        }

        protected virtual TResult Synchronize<TResult>(Func<Task<TResult>> func)
        {
            //var task = Task.Factory.StartNew(func, CancellationToken.None, TaskCreationOptions.None, this.GetTaskScheduler()).Unwrap();
            var task = Task.Run(func);
            task.ConfigureAwait(CONFIGURE_AWAIT);
            task.Wait();
            return task.Result;
        }

        public async Task<IToken> AuthenticateAsync(string login, string password, string ip = null)
        {
            var responseMessage = await this.Client.PostAsJsonAsync("/api/auth/login", new LoginModel()
            {
                Login = login,
                Password = password
            });

            return await responseMessage.Content.ReadFromJsonAsync<Token>();
        }

        public async Task<IToken> AuthenticateAsync(string token)
        {
            var responseMessage = await this.Client.PostAsync($"/api/auth/token/{token}", null);
            return await responseMessage.Content.ReadFromJsonAsync<Token>();
        }

        public IToken Authenticate(string login, string password, string ip = null)
        {
            return this.Synchronize(() => this.AuthenticateAsync(login, password, ip));
        }

        public IToken Authenticate(string token)
        {
            return this.Synchronize(() => this.AuthenticateAsync(token));
        }
    }
}
