using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SX.Common.Shared.Contracts;
using System;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace SX.Common.Api.Classes
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {
        public const string SCHEMA = "Token";
        public const string HEADER = "Authorization";

        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly Shared.Contracts.ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenAuthenticationHandler(
            IOptionsMonitor<TokenAuthenticationOptions> options,
            ILoggerFactory loggerFactory,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthenticationProvider authenticationProvider,
            Shared.Contracts.ILogger logger,
            IHttpContextAccessor httpContextAccessor)
            : base(options, loggerFactory, encoder, clock)
        {
            _authenticationProvider = authenticationProvider;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            _logger.Trace("checking request");

            if (!Request.Headers.ContainsKey(HEADER))
                return Unauthorized();

            _logger.Trace("checking header");

            string header = Request.Headers[HEADER];
            if (string.IsNullOrEmpty(header))
                return Unauthorized();
            //return AuthenticateResult.NoResult();

            _logger.Trace($"header: {header}");

            if (!header.StartsWith(SCHEMA, StringComparison.OrdinalIgnoreCase))
                return Unauthorized();

            string tokenCode = header.Substring(SCHEMA.Length).Trim();
            if (string.IsNullOrEmpty(tokenCode))
                return Unauthorized();

            _logger.Trace($"token code from header: {tokenCode}");

            try
            {
                return ValidateToken(tokenCode);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail(ex.Message);
            }
        }

        private AuthenticateResult ValidateToken(string code)
        {
            _logger.Trace($"validating token: {code}");

            var token = _authenticationProvider.Authenticate(code);
            if (token == null || !token.IsValid())
                return Unauthorized();

            _logger.Trace($"creating principal for: {code}");

            var identity = new ApiIdentity(token);
            var principal = new GenericPrincipal(identity, token.Roles);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            _httpContextAccessor.HttpContext.User = principal;
            _httpContextAccessor.HttpContext.User.AddIdentity(identity);
            
            return AuthenticateResult.Success(ticket);
        }

        static private AuthenticateResult Unauthorized() => AuthenticateResult.Fail("Unauthorized");
    }

    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
}
