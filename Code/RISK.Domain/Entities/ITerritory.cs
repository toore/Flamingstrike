namespace RISK.Domain.Entities
{
    public interface ITerritory
    {
        ILocation Location { get; }
        IPlayer AssignedToPlayer { get; set; }
        int Armies { get; set; }
    }
}