using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayer> Players { get; }
        IEnumerable<Region> RegionsThatCanBeSourceForAttackOrFortification { get; }
        void Attack(Region attackingRegion, Region defendingRegion);
        void Fortify(Region sourceRegion, Region destinationRegion, int armies);
        IEnumerable<Region> GetRegionsThatCanBeAttacked(Region sourceRegion);
        IEnumerable<Region> GetRegionsThatCanBeFortified(Region sourceRegion);
        void EndTurn();
    }
}