using System;
using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine.Play.GameStates;

namespace FlamingStrike.GameEngine.Play
{
    public interface IAttackPhase
    {
        IPlayer CurrentPlayer { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        IReadOnlyList<IRegion> RegionsThatCanBeSourceForAttackOrFortification { get; }
        void Attack(IRegion attackingRegion, IRegion defendingRegion);
        void Fortify(IRegion sourceRegion, IRegion destinationRegion, int armies);
        IEnumerable<IRegion> GetRegionsThatCanBeAttacked(IRegion sourceRegion);
        IEnumerable<IRegion> GetRegionsThatCanBeFortified(IRegion sourceRegion);
        int GetMaxNumberOfAttackingArmies(IRegion region);
        void EndTurn();
    }

    public class AttackPhase : IAttackPhase
    {
        private readonly IAttackPhaseGameState _attackPhaseGameState;

        private const int MaxNumberOfAttackingArmies = 3;

        public AttackPhase(IPlayer currentPlayer, IReadOnlyList<ITerritory> territories, IReadOnlyList<IPlayerGameData> playerGameDatas, IAttackPhaseGameState attackPhaseGameState, IReadOnlyList<IRegion> regionsThatCanBeSourceForAttackOrFortification)
        {
            _attackPhaseGameState = attackPhaseGameState;
            CurrentPlayer = currentPlayer;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            RegionsThatCanBeSourceForAttackOrFortification = regionsThatCanBeSourceForAttackOrFortification;
        }

        public IPlayer CurrentPlayer { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
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

        public int GetMaxNumberOfAttackingArmies(IRegion region)
        {
            var maxAvailableArmies = _attackPhaseGameState.Territories.Single(x => x.Region == region).GetNumberOfArmiesThatCanBeUsedInAnAttack();

            return Math.Min(MaxNumberOfAttackingArmies, maxAvailableArmies);
        }

        public void EndTurn()
        {
            _attackPhaseGameState.EndTurn();
        }
    }
}