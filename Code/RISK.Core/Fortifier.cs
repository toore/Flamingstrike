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
            var hasBorder = sourceRegion.HasBorder(destinationRegion);

            var canFortify =
                playerOccupiesBothTerritories
                &&
                hasBorder;

            return canFortify;
        }

        public IReadOnlyList<ITerritory> Fortify(IReadOnlyList<ITerritory> territories, IRegion sourceRegion, IRegion destinationRegion, int armies)
        {
            throw new NotImplementedException();
        }
    }
}