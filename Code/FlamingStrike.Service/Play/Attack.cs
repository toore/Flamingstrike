namespace FlamingStrike.Service.Play
{
    public class Attack
    {
        public Attack(string attackingRegion, string defendingRegion)
        {
            AttackingRegion = attackingRegion;
            DefendingRegion = defendingRegion;
        }

        public string AttackingRegion { get; }
        public string DefendingRegion { get; }
    }
}