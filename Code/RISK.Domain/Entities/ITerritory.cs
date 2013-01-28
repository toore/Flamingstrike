namespace RISK.Domain.Entities
{
    public interface ITerritory
    {
        ILocation Location { get; }
        IPlayer Owner { get; set; }
        int Armies { get; set; }
        bool HasOwner { get; }
    }
}