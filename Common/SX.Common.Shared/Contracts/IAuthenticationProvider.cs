using SX.Common.Shared.Interfaces;
using System.Threading.Tasks;

namespace SX.Common.Shared.Contracts
{
    public interface IAuthenticationProvider
    {
        IToken Authenticate(string login, string password, string ip = null);
        IToken Authenticate(string token);
    }

}
