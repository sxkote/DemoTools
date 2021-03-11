using SX.Common.Domain.Enums;

namespace SX.Common.Domain.Interfaces
{
    public interface IDbState
    {
        DbState DbState { get; }

        void ChangeDbState(DbState state);
        void DeleteFromDb();
    }
}
