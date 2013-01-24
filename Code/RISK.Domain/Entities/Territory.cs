namespace RISK.Domain.Entities
{
    public class Territory : ITerritory
    {
        public Territory(ITerritoryLocation territoryLocation)
        {
            TerritoryLocation = territoryLocation;
        }

        public ITerritoryLocation TerritoryLocation { get; private set; }

        public IPlayer Owner { get; set; }
        public int Armies { get; set; }
        public bool HasOwner
        {
            get { return Owner != null; }
        }
    }
}