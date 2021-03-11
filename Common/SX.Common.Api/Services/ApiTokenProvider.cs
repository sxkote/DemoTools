using Microsoft.AspNetCore.Http;
using SX.Common.Api.Classes;
using SX.Common.Shared.Contracts;
using SX.Common.Shared.Interfaces;
using System.Linq;

namespace SX.Common.Api.Services
{
    public class ApiTokenProvider : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiTokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IToken GetToken()
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null)
                return null;

            var userIdentity = user.Identities.FirstOrDefault(i => i is ApiIdentity);
            if (userIdentity == null)
                return null;

            var apiIdentity = userIdentity as ApiIdentity;
            if (apiIdentity == null)
                return null;

            return apiIdentity.Token;
        }
    }
}
