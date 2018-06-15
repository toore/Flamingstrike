namespace FlamingStrike.Service.Play
{
    public class Fortify
    {
        public Fortify(string sourceRegion, string destinationRegion, int armies)
        {
            SourceRegion = sourceRegion;
            DestinationRegion = destinationRegion;
            Armies = armies;
        }

        public string SourceRegion { get; }
        public string DestinationRegion { get; }
        public int Armies { get; }
    }
}