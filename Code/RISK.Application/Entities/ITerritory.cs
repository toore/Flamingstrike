namespace RISK.Domain.Entities
{
    public interface ITerritory
    {
        ILocation Location { get; }
        IPlayer Occupant { get; set; }
        int Armies { get; set; }
    }
}