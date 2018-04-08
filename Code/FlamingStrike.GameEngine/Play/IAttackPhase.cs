using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        IReadOnlyList<IRegion> GetRegionsThatCanBeSourceForAttackOrFortification();
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion);
        IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion);
        void EndTurn();
    }
}