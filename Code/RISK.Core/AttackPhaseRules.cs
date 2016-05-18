using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface IAttackPhaseRules
    {
        bool CanAttack(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion defendingRegion);
        bool CanFortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion);
        bool IsPlayerEliminated(IEnumerable<ITerritory> territories, IPlayer player);
        bool IsGameOver(IEnumerable<ITerritory> territories);
    }

    public class AttackPhaseRules : IAttackPhaseRules
    {
        public bool CanAttack(IReadOnlyList<ITerritory> territories, IRegion attackingRegion, IRegion defendingRegion)
        {
            var attackingTerritory = territories.Single(x => x.Region == attackingRegion);
            var defendingTerritory = territories.Single(x => x.Region == defendingRegion);

            var canAttack =
                HasBorder(attackingTerritory, defendingTerritory)
                &&
                IsAttackerAndDefenderDifferentPlayers(attackingTerritory, defendingTerritory)
                &&
                HasAttackerEnoughArmiesToPerformAttack(attackingTerritory);

            return canAttack;
        }

        private static bool HasBorder(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Region.HasBorder(defendingTerritory.Region);
        }

        private static bool IsAttackerAndDefenderDifferentPlayers(ITerritory attackingTerritory, ITerritory defendingTerritory)
        {
            return attackingTerritory.Player != defendingTerritory.Player;
        }

        private static bool HasAttackerEnoughArmiesToPerformAttack(ITerritory attackingTerritory)
        {
            return attackingTerritory.GetNumberOfArmiesAvailableForAttack() > 0;
        }

        public bool CanFortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion)
        {
            var sourceTerritory = territories.Single(x => x.Region == sourceRegion);
            var destinationTerritory = territories.Single(x => x.Region == destinationRegion);
            var playerOccupiesBothTerritories = sourceTerritory.Player == destinationTerritory.Player;
            var hasBorder = sourceRegion.HasBorder(destinationRegion);

            var canFortify =
                playerOccupiesBothTerritories
                &&
                hasBorder;

            return canFortify;
        }

        public bool IsPlayerEliminated(IEnumerable<ITerritory> territories, IPlayer player)
        {
            var playerOccupiesTerritories = territories.Any(x => x.Player == player);

            return !playerOccupiesTerritories;
        }

        public bool IsGameOver(IEnumerable<ITerritory> territories)
        {
            var allTerritoriesAreOccupiedBySamePlayer = territories
                .Select(x => x.Player)
                .Distinct()
                .Count() == 1;

            return allTerritoriesAreOccupiedBySamePlayer;
        }
    }
}