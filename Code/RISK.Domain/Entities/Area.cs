namespace RISK.Domain.Entities
{
    public class Area : IArea
    {
        public Area(IAreaDefinition areaDefinition)
        {
            AreaDefinition = areaDefinition;
        }

        public IAreaDefinition AreaDefinition { get; private set; }

        public IPlayer Owner { get; set; }
        public int Armies { get; set; }
        public bool HasOwner
        {
            get { return Owner != null; }
        }
    }
}