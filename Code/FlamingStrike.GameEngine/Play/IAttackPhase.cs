using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        IReadOnlyList<Region> GetRegionsThatCanBeSourceForAttackOrFortification();
        void Attack(Region attackingRegion, Region defendingRegion);
        void Fortify(Region sourceRegion, Region destinationRegion, int armies);
        IEnumerable<Region> GetRegionsThatCanBeAttacked(Region sourceRegion);
        IEnumerable<Region> GetRegionsThatCanBeFortified(Region sourceRegion);
        void EndTurn();
    }
}