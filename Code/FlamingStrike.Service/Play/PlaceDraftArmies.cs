namespace FlamingStrike.Service.Play
{
    public class PlaceDraftArmies
    {
        public PlaceDraftArmies(string region, int numberOfArmies)
        {
            Region = region;
            NumberOfArmies = numberOfArmies;
        }

        public string Region { get; }
        public int NumberOfArmies { get; }
    }
}