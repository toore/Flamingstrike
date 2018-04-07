using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IArmyDraftCalculator
    {
        int Calculate(PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories);
    }

    public class ArmyDraftCalculator : IArmyDraftCalculator
    {
        private readonly IContinents _continents;

        public ArmyDraftCalculator(IContinents continents)
        {
            _continents = continents;
        }

        public int Calculate(PlayerName currentPlayerName, IReadOnlyList<ITerritory> territories)
        {
            var armiesDraftedBasedOnTerritoriesOccupied = CalculateArmiesDraftedBasedOnTerritoriesOccupied(currentPlayerName, territories);
            var armiesDraftedBasedOnContinentsOccupied = CalculateArmiesDraftedBasedOnContinentsOccupied(currentPlayerName, territories);

            return armiesDraftedBasedOnTerritoriesOccupied + armiesDraftedBasedOnContinentsOccupied;
        }

        private static int CalculateArmiesDraftedBasedOnTerritoriesOccupied(PlayerName currentPlayerName, IEnumerable<ITerritory> territories)
        {
            var numberOfOccupiedTerritories = territories.Count(territory => territory.PlayerName == currentPlayerName);

            return Math.Max(numberOfOccupiedTerritories / 3, 3);
        }

        private int CalculateArmiesDraftedBasedOnContinentsOccupied(PlayerName currentPlayerName, IEnumerable<ITerritory> territories)
        {
            return _continents.GetAll()
                .Where(continent => IsContinentOccupiedByPlayer(continent, currentPlayerName, territories))
                .Sum(continent => continent.Bonus);
        }

        private static bool IsContinentOccupiedByPlayer(IContinent continent, PlayerName playerName, IEnumerable<ITerritory> territories)
        {
            return territories
                .Where(x => x.Region.Continent == continent)
                .All(x => x.PlayerName == playerName);
        }
    }
}