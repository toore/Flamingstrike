namespace RISK.Domain.Entities
{
    public interface IArea
    {
        IPlayer Owner { get; set; }
        int Armies { get; set; }
        bool HasOwner { get; }
    }
}