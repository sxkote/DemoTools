using SX.Common.Shared.Interfaces;
using System.Security.Principal;

namespace SX.Common.Api.Classes
{
    public class ApiIdentity : GenericIdentity
    {
        public IToken Token { get; protected set; }

        public ApiIdentity(IToken token)
            : base(token?.Name ?? "")
        {
            Token = token;
        }
    }
}
