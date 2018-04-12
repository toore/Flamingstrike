using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IFortifier
    {
        bool CanFortify(IReadOnlyList<ITerritory> territories, Region sourceRegion, Region destinationRegion);
        IReadOnlyList<ITerritory> Fortify(IReadOnlyList<ITerritory> territories, Region sourceRegion, Region destinationRegion, int armies);
    }

    public class Fortifier : IFortifier
    {
        private readonly IWorldMap _worldMap;

        public Fortifier(IWorldMap worldMap)
        {
            _worldMap = worldMap;
        }

        public bool CanFortify(IReadOnlyList<ITerritory> territories, Region sourceRegion, Region destinationRegion)
        {
            var sourceTerritory = territories.Single(x => x.Region == sourceRegion);
            var destinationTerritory = territories.Single(x => x.Region == destinationRegion);
            var playerOccupiesBothTerritories = sourceTerritory.Name == destinationTerritory.Name;
            var hasBorder = IsTerritoriesAdjacent(sourceRegion, destinationRegion);
            var hasAtLeastOneArmyThatCanFortifyAnotherTerritory = sourceTerritory.GetNumberOfArmiesThatCanFortifyAnotherTerritory() > 0;

            var canFortify =
                playerOccupiesBothTerritories
                &&
                hasBorder
                &&
                hasAtLeastOneArmyThatCanFortifyAnotherTerritory;

            return canFortify;
        }

        private bool IsTerritoriesAdjacent(Region sourceRegion, Region destinationRegion)
        {
            return _worldMap.HasBorder(sourceRegion, destinationRegion);
        }

        public IReadOnlyList<ITerritory> Fortify(IReadOnlyList<ITerritory> territories, Region sourceRegion, Region destinationRegion, int armies)
        {
            if (!CanFortify(territories, sourceRegion, destinationRegion))
            {
                throw new InvalidOperationException("Can't fortify");
            }

            var sourceTerritory = territories.Single(territory => territory.Region == sourceRegion);
            var destinationTerritory = territories.Single(territory => territory.Region == destinationRegion);

            var updatedSourceTerritory = new Territory(sourceRegion, sourceTerritory.Name, sourceTerritory.Armies - armies);
            var updatedDestinationTerritory = new Territory(destinationRegion, destinationTerritory.Name, destinationTerritory.Armies + armies);

            return territories
                .Except(new[] { sourceTerritory, destinationTerritory })
                .Union(new[] { updatedSourceTerritory, updatedDestinationTerritory })
                .ToList();
        }
    }
}