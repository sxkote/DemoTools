using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;

namespace DemoTools.Core.Proxy.Services
{
    public class AuthenticationFakeService : IAuthenticationProvider
    {
        public IToken Authenticate(string login, string password, string ip = null)
        {
            throw new System.NotImplementedException();
        }

        public IToken Authenticate(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}
