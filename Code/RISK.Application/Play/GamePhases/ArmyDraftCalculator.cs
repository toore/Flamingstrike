using System;
using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public interface IArmyDraftCalculator
    {
        int Calculate(IPlayer currentPlayer, IEnumerable<ITerritory> territories);
    }

    public class ArmyDraftCalculator : IArmyDraftCalculator
    {
        private readonly IContinents _continents;

        public ArmyDraftCalculator(IContinents continents)
        {
            _continents = continents;
        }

        public int Calculate(IPlayer currentPlayer, IEnumerable<ITerritory> territories)
        {
            var armiesDraftedBasedOnTerritoriesOccupied = CalculateArmiesDraftedBasedOnTerritoriesOccupied(currentPlayer, territories);
            var armiesDraftedBasedOnContinentsOccupied = CalculateArmiesDraftedBasedOnContinentsOccupied(currentPlayer, territories);

            return armiesDraftedBasedOnTerritoriesOccupied + armiesDraftedBasedOnContinentsOccupied;
        }

        private static int CalculateArmiesDraftedBasedOnTerritoriesOccupied(IPlayer currentPlayer, IEnumerable<ITerritory> territories)
        {
            var numberOfOccupiedTerritories = territories.Count(territory => territory.Player == currentPlayer);

            var armiesDraftedBasedOnTerritoriesOccupied = Math.Min(numberOfOccupiedTerritories / 3, 3);
            return armiesDraftedBasedOnTerritoriesOccupied;
        }

        private int CalculateArmiesDraftedBasedOnContinentsOccupied(IPlayer currentPlayer, IEnumerable<ITerritory> territories)
        {
            var armiesDraftedBasedOnContinentsOccupied = _continents.GetAll()
                .Where(continent => IsContinentOccupiedByPlayer(continent, currentPlayer, territories))
                .Sum(continent => continent.Bonus);

            return armiesDraftedBasedOnContinentsOccupied;
        }

        private static bool IsContinentOccupiedByPlayer(IContinent continent, IPlayer player, IEnumerable<ITerritory> territories)
        {
            var isContinentOccupiedByPlayer = territories
                .All(x =>
                    x.Region.Continent == continent
                    &&
                    x.Player == player);

            return isContinentOccupiedByPlayer;
        }
    }
}