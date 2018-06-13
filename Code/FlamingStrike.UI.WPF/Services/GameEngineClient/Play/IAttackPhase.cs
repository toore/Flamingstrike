using System.Collections.Generic;

namespace FlamingStrike.UI.WPF.Services.GameEngineClient.Play
{
    public interface IAttackPhase
    {
        string CurrentPlayerName { get; }
        IReadOnlyList<Territory> Territories { get; }
        IReadOnlyList<Player> Players { get; }
        IReadOnlyList<Region> GetRegionsThatCanBeSourceForAttackOrFortification();
        void Attack(Region attackingRegion, Region defendingRegion);
        void Fortify(Region sourceRegion, Region destinationRegion, int armies);
        IEnumerable<Region> GetRegionsThatCanBeAttacked(Region sourceRegion);
        IEnumerable<Region> GetRegionsThatCanBeFortified(Region sourceRegion);
        void EndTurn();
    }
}