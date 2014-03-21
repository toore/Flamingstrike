using System;
using System.Collections.Generic;
using RISK.Domain;
using RISK.Domain.Entities;

namespace GuiWpf.Services
{
    public class TerritoryColorsFactory : ITerritoryColorsFactory
    {
        private readonly IColorService _colorService;
        private readonly Dictionary<ILocation, Func<ITerritoryColors>> _colors;

        public TerritoryColorsFactory(Locations locations, IColorService colorService)
        {
            _colorService = colorService;

            _colors = new Dictionary<ILocation, Func<ITerritoryColors>>
            {
                { locations.Alaska, () => colorService.NorthAmericaColors },
                { locations.Alberta, () => colorService.NorthAmericaColors },
                { locations.CentralAmerica, () => colorService.NorthAmericaColors },
                { locations.EasternUnitedStates, () => colorService.NorthAmericaColors },
                { locations.Greenland, () => colorService.NorthAmericaColors },
                { locations.NorthwestTerritory, () => colorService.NorthAmericaColors },
                { locations.Ontario, () => colorService.NorthAmericaColors },
                { locations.Quebec, () => colorService.NorthAmericaColors },
                { locations.WesternUnitedStates, () => colorService.NorthAmericaColors },
                { locations.Argentina, () => colorService.SouthAmericaColors },
                { locations.Brazil, () => colorService.SouthAmericaColors },
                { locations.Peru, () => colorService.SouthAmericaColors },
                { locations.Venezuela, () => colorService.SouthAmericaColors },
                { locations.GreatBritain, () => colorService.EuropeColors },
                { locations.Iceland, () => colorService.EuropeColors },
                { locations.NorthernEurope, () => colorService.EuropeColors },
                { locations.Scandinavia, () => colorService.EuropeColors },
                { locations.SouthernEurope, () => colorService.EuropeColors },
                { locations.Ukraine, () => colorService.EuropeColors },
                { locations.WesternEurope, () => colorService.EuropeColors },
                { locations.Congo, () => colorService.AfricaColors },
                { locations.EastAfrica, () => colorService.AfricaColors },
                { locations.Egypt, () => colorService.AfricaColors },
                { locations.Madagascar, () => colorService.AfricaColors },
                { locations.NorthAfrica, () => colorService.AfricaColors },
                { locations.SouthAfrica, () => colorService.AfricaColors },
                { locations.Afghanistan, () => colorService.AsiaColors },
                { locations.China, () => colorService.AsiaColors },
                { locations.India, () => colorService.AsiaColors },
                { locations.Irkutsk, () => colorService.AsiaColors },
                { locations.Japan, () => colorService.AsiaColors },
                { locations.Kamchatka, () => colorService.AsiaColors },
                { locations.MiddleEast, () => colorService.AsiaColors },
                { locations.Mongolia, () => colorService.AsiaColors },
                { locations.Siam, () => colorService.AsiaColors },
                { locations.Siberia, () => colorService.AsiaColors },
                { locations.Ural, () => colorService.AsiaColors },
                { locations.Yakutsk, () => colorService.AsiaColors },
                { locations.EasternAustralia, () => colorService.AustraliaColors },
                { locations.Indonesia, () => colorService.AustraliaColors },
                { locations.NewGuinea, () => colorService.AustraliaColors },
                { locations.WesternAustralia, () => colorService.AustraliaColors },
            };
        }

        public ITerritoryColors Create(ITerritory territory)
        {
            if (territory.IsOccupied())
            {
                return _colorService.GetPlayerTerritoryColors(territory.Occupant);
            }

            return _colors[territory.Location]();
        }
    }
}