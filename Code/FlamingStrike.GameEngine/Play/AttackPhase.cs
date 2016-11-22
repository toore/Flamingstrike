using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackPhase : IGameStatus
    {
        IReadOnlyList<IRegion> RegionsThatCanBeSourceForAttackOrFortification { get; }
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion);
        IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion);
        void EndTurn();
    }

    public class AttackPhase : IAttackPhase
    {
        private readonly IAttackPhaseGameState _attackPhaseGameState;

        public AttackPhase(IAttackPhaseGameState attackPhaseGameState, IReadOnlyList<IRegion> regionsThatCanBeSourceForAttackOrFortification)
        {
            _attackPhaseGameState = attackPhaseGameState;
            RegionsThatCanBeSourceForAttackOrFortification = regionsThatCanBeSourceForAttackOrFortification;
        }

        public IPlayer Player => _attackPhaseGameState.Player;

        public IReadOnlyList<ITerritory> Territories => _attackPhaseGameState.Territories;
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas => _attackPhaseGameState.Players;

        public IReadOnlyList<IRegion> RegionsThatCanBeSourceForAttackOrFortification { get; }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            _attackPhaseGameState.Attack(attackingRegion, defendingRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            _attackPhaseGameState.Fortify(sourceRegion, destinationRegion, armies);
        }

        public IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion)
        {
            return sourceRegion.GetBorderingRegions()
                .Where(borderRegion => _attackPhaseGameState.CanAttack(sourceRegion, borderRegion));
        }

        public IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion)
        {
            return sourceRegion.GetBorderingRegions()
                .Where(borderRegion => _attackPhaseGameState.CanFortify(sourceRegion, borderRegion));
        }

        public void EndTurn()
        {
            _attackPhaseGameState.EndTurn();
        }
    }
}