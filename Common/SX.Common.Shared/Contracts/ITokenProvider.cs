using SX.Common.Shared.Interfaces;

namespace SX.Common.Shared.Contracts
{
    public interface ITokenProvider
    {
        IToken GetToken();
    }
}
