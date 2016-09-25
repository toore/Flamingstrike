using System.Collections.Generic;
using System.Linq;
using RISK.Core;
using RISK.GameEngine.Play.GameStates;

namespace RISK.GameEngine.Play
{
    public interface IAttackPhase
    {
        IReadOnlyList<IRegion> RegionsThatCanBeSourceForAttackOrFortification { get; }
        bool CanAttack(IRegion attackingRegion, IRegion defendingRegion);
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        bool CanFortify(IRegion sourceRegion, IRegion destinationRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion);
        IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion);
        void EndTurn();
    }

    public class AttackPhase : IAttackPhase
    {
        private readonly IAttackPhaseGameState _attackPhaseGameState;

        public AttackPhase(IAttackPhaseGameState attackPhaseGameState, IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IRegion> regionsThatCanBeSourceForAttackOrFortification)
        {
            _attackPhaseGameState = attackPhaseGameState;
            CurrentPlayer = currentPlayer;
            Territories = territories;
            RegionsThatCanBeSourceForAttackOrFortification = regionsThatCanBeSourceForAttackOrFortification;
        }

        public IPlayer CurrentPlayer { get; }

        public IReadOnlyList<ITerritory> Territories { get; }

        public IReadOnlyList<IRegion> RegionsThatCanBeSourceForAttackOrFortification { get; }

        public bool CanAttack(IRegion attackingRegion, IRegion defendingRegion)
        {
            return _attackPhaseGameState.CanAttack(attackingRegion, defendingRegion);
        }

        public void Attack(IRegion attackingRegion, IRegion defendingRegion)
        {
            _attackPhaseGameState.Attack(attackingRegion, defendingRegion);
        }

        public bool CanFortify(IRegion sourceRegion, IRegion destinationRegion)
        {
            return _attackPhaseGameState.CanFortify(sourceRegion, destinationRegion);
        }

        public void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            _attackPhaseGameState.Fortify(sourceRegion, destinationRegion, armies);
        }

        public IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion)
        {
            return sourceRegion.GetBorderingRegions()
                .Where(borderRegion => CanAttack(sourceRegion, borderRegion));
        }

        public IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion)
        {
            return sourceRegion.GetBorderingRegions()
                .Where(borderRegion => CanFortify(sourceRegion, borderRegion));
        }

        public void EndTurn()
        {
            _attackPhaseGameState.EndTurn();
        }
    }
}