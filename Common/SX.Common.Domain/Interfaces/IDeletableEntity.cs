namespace SX.Common.Domain.Interfaces
{
    public interface IDeletableEntity
    {
        bool IsDeleted { get; }
        void MarkDeleted();
    }
}
