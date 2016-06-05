using System;
using System.Collections.Generic;
using RISK.Core;
using RISK.GameEngine;

namespace GuiWpf.Services
{
    public interface IRegionColorSettingsFactory
    {
        IRegionColorSettings Create(IRegion region);
    }

    public class RegionColorSettingsFactory : IRegionColorSettingsFactory
    {
        private readonly Dictionary<IRegion, Func<IRegionColorSettings>> _colors;

        public RegionColorSettingsFactory(IColorService colorService, IRegions regions)
        {
            _colors = new Dictionary<IRegion, Func<IRegionColorSettings>>
            {
                { regions.Alaska, () => colorService.NorthAmericaColors },
                { regions.Alberta, () => colorService.NorthAmericaColors },
                { regions.CentralAmerica, () => colorService.NorthAmericaColors },
                { regions.EasternUnitedStates, () => colorService.NorthAmericaColors },
                { regions.Greenland, () => colorService.NorthAmericaColors },
                { regions.NorthwestRegion, () => colorService.NorthAmericaColors },
                { regions.Ontario, () => colorService.NorthAmericaColors },
                { regions.Quebec, () => colorService.NorthAmericaColors },
                { regions.WesternUnitedStates, () => colorService.NorthAmericaColors },
                { regions.Argentina, () => colorService.SouthAmericaColors },
                { regions.Brazil, () => colorService.SouthAmericaColors },
                { regions.Peru, () => colorService.SouthAmericaColors },
                { regions.Venezuela, () => colorService.SouthAmericaColors },
                { regions.GreatBritain, () => colorService.EuropeColors },
                { regions.Iceland, () => colorService.EuropeColors },
                { regions.NorthernEurope, () => colorService.EuropeColors },
                { regions.Scandinavia, () => colorService.EuropeColors },
                { regions.SouthernEurope, () => colorService.EuropeColors },
                { regions.Ukraine, () => colorService.EuropeColors },
                { regions.WesternEurope, () => colorService.EuropeColors },
                { regions.Congo, () => colorService.AfricaColors },
                { regions.EastAfrica, () => colorService.AfricaColors },
                { regions.Egypt, () => colorService.AfricaColors },
                { regions.Madagascar, () => colorService.AfricaColors },
                { regions.NorthAfrica, () => colorService.AfricaColors },
                { regions.SouthAfrica, () => colorService.AfricaColors },
                { regions.Afghanistan, () => colorService.AsiaColors },
                { regions.China, () => colorService.AsiaColors },
                { regions.India, () => colorService.AsiaColors },
                { regions.Irkutsk, () => colorService.AsiaColors },
                { regions.Japan, () => colorService.AsiaColors },
                { regions.Kamchatka, () => colorService.AsiaColors },
                { regions.MiddleEast, () => colorService.AsiaColors },
                { regions.Mongolia, () => colorService.AsiaColors },
                { regions.Siam, () => colorService.AsiaColors },
                { regions.Siberia, () => colorService.AsiaColors },
                { regions.Ural, () => colorService.AsiaColors },
                { regions.Yakutsk, () => colorService.AsiaColors },
                { regions.EasternAustralia, () => colorService.AustraliaColors },
                { regions.Indonesia, () => colorService.AustraliaColors },
                { regions.NewGuinea, () => colorService.AustraliaColors },
                { regions.WesternAustralia, () => colorService.AustraliaColors },
            };
        }

        public IRegionColorSettings Create(IRegion region)
        {
            //if (territory.IsOccupied())
            //{
            //    return _colorService.GetPlayerTerritoryColors(territory.Occupant);
            //}

            return _colors[region]();
        }
    }
}