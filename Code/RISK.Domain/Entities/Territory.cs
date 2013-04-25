namespace RISK.Domain.Entities
{
    public class Territory : ITerritory
    {
        public Territory(ILocation location)
        {
            Location = location;
        }

        public ILocation Location { get; private set; }

        public IPlayer AssignedToPlayer { get; set; }
        public int Armies { get; set; }
    }
}