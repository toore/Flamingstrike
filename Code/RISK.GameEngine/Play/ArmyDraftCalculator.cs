using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.GameEngine.Play
{
    public interface IArmyDraftCalculator
    {
        int Calculate(Core.IPlayer currentPlayer, IReadOnlyList<ITerritory> territories);
    }

    public class ArmyDraftCalculator : IArmyDraftCalculator
    {
        private readonly IContinents _continents;

        public ArmyDraftCalculator(IContinents continents)
        {
            _continents = continents;
        }

        public int Calculate(Core.IPlayer currentPlayer, IReadOnlyList<ITerritory> territories)
        {
            var armiesDraftedBasedOnTerritoriesOccupied = CalculateArmiesDraftedBasedOnTerritoriesOccupied(currentPlayer, territories);
            var armiesDraftedBasedOnContinentsOccupied = CalculateArmiesDraftedBasedOnContinentsOccupied(currentPlayer, territories);

            return armiesDraftedBasedOnTerritoriesOccupied + armiesDraftedBasedOnContinentsOccupied;
        }

        private static int CalculateArmiesDraftedBasedOnTerritoriesOccupied(Core.IPlayer currentPlayer, IEnumerable<ITerritory> territories)
        {
            var numberOfOccupiedTerritories = territories.Count(territory => territory.Player == currentPlayer);

            return Math.Max(numberOfOccupiedTerritories / 3, 3);
        }

        private int CalculateArmiesDraftedBasedOnContinentsOccupied(Core.IPlayer currentPlayer, IEnumerable<ITerritory> territories)
        {
            return _continents.GetAll()
                .Where(continent => IsContinentOccupiedByPlayer(continent, currentPlayer, territories))
                .Sum(continent => continent.Bonus);
        }

        private static bool IsContinentOccupiedByPlayer(IContinent continent, Core.IPlayer player, IEnumerable<ITerritory> territories)
        {
            return territories
                .Where(x => x.Region.Continent == continent)
                .All(x => x.Player == player);
        }
    }
}