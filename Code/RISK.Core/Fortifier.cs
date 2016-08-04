using System;
using System.Collections.Generic;
using System.Linq;

namespace RISK.Core
{
    public interface IFortifier
    {
        bool CanFortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion);
        IReadOnlyList<ITerritory> Fortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion, int armies);
    }

    public class Fortifier : IFortifier
    {
        public bool CanFortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion)
        {
            var sourceTerritory = territories.Single(x => x.Region == sourceRegion);
            var destinationTerritory = territories.Single(x => x.Region == destinationRegion);
            var playerOccupiesBothTerritories = sourceTerritory.Player == destinationTerritory.Player;
            var hasBorder = IsTerritoriesAdjacent(sourceRegion, destinationRegion);

            var canFortify =
                playerOccupiesBothTerritories
                &&
                hasBorder;

            return canFortify;
        }

        private static bool IsTerritoriesAdjacent(IRegion sourceRegion, IRegion destinationRegion)
        {
            return sourceRegion.HasBorder(destinationRegion);
        }

        public IReadOnlyList<ITerritory> Fortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            if (!CanFortify(territories, sourceRegion, destinationRegion))
            {
                throw new InvalidOperationException("Can't fortify");
            }

            var sourceTerritory = territories.Single(territory => territory.Region == sourceRegion);
            var destinationTerritory = territories.Single(territory => territory.Region == destinationRegion);

            var updatedSourceTerritory = new Territory(sourceRegion, sourceTerritory.Player, sourceTerritory.Armies - armies);
            var updatedDestinationTerritory = new Territory(destinationRegion, destinationTerritory.Player, destinationTerritory.Armies + armies);

            var updatedTerritories = territories
                .ReplaceItem(sourceTerritory, updatedSourceTerritory)
                .ReplaceItem(destinationTerritory, updatedDestinationTerritory)
                .ToList();

            return updatedTerritories;
        }
    }
}