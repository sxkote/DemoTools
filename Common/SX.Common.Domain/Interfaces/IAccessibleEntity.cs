using SX.Common.Shared.Interfaces;

namespace SX.Common.Domain.Interfaces
{
    public interface IAccessibleEntity
    {
        bool HasAccess(IToken token);
    }
}
