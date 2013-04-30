namespace RISK.Domain.Entities
{
    public interface ITerritory
    {
        ILocation Location { get; }
        IPlayer AssignedPlayer { get; set; }
        int Armies { get; set; }
        bool IsPlayerAssigned();
    }
}