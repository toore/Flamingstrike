using System;
using System.Collections.Generic;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.Services
{
    public class TerritoryColorsFactory : ITerritoryColorsFactory
    {
        private readonly ILocationProvider _locationProvider;
        private readonly IColorService _colorService;
        private readonly Dictionary<ILocation, Func<ITerritoryColors>> _colors;

        public TerritoryColorsFactory(ILocationProvider locationProvider, IColorService colorService)
        {
            _locationProvider = locationProvider;
            _colorService = colorService;

            _colors = new Dictionary<ILocation, Func<ITerritoryColors>>
                {
                    { _locationProvider.Alaska, () => colorService.NorthAmericaColors },
                    { _locationProvider.Alberta, () => colorService.NorthAmericaColors },
                    { _locationProvider.CentralAmerica, () => colorService.NorthAmericaColors },
                    { _locationProvider.EasternUnitedStates, () => colorService.NorthAmericaColors },
                    { _locationProvider.Greenland, () => colorService.NorthAmericaColors },
                    { _locationProvider.NorthwestTerritory, () => colorService.NorthAmericaColors },
                    { _locationProvider.Ontario, () => colorService.NorthAmericaColors },
                    { _locationProvider.Quebec, () => colorService.NorthAmericaColors },
                    { _locationProvider.WesternUnitedStates, () => colorService.NorthAmericaColors },
                    { _locationProvider.Argentina, () => colorService.SouthAmericaColors },
                    { _locationProvider.Brazil, () => colorService.SouthAmericaColors },
                    { _locationProvider.Peru, () => colorService.SouthAmericaColors },
                    { _locationProvider.Venezuela, () => colorService.SouthAmericaColors },
                    { _locationProvider.GreatBritain, () => colorService.EuropeColors },
                    { _locationProvider.Iceland, () => colorService.EuropeColors },
                    { _locationProvider.NorthernEurope, () => colorService.EuropeColors },
                    { _locationProvider.Scandinavia, () => colorService.EuropeColors },
                    { _locationProvider.SouthernEurope, () => colorService.EuropeColors },
                    { _locationProvider.Ukraine, () => colorService.EuropeColors },
                    { _locationProvider.WesternEurope, () => colorService.EuropeColors },
                    { _locationProvider.Congo, () => colorService.AfricaColors },
                    { _locationProvider.EastAfrica, () => colorService.AfricaColors },
                    { _locationProvider.Egypt, () => colorService.AfricaColors },
                    { _locationProvider.Madagascar, () => colorService.AfricaColors },
                    { _locationProvider.NorthAfrica, () => colorService.AfricaColors },
                    { _locationProvider.SouthAfrica, () => colorService.AfricaColors },
                    { _locationProvider.Afghanistan, () => colorService.AsiaColors },
                    { _locationProvider.China, () => colorService.AsiaColors },
                    { _locationProvider.India, () => colorService.AsiaColors },
                    { _locationProvider.Irkutsk, () => colorService.AsiaColors },
                    { _locationProvider.Japan, () => colorService.AsiaColors },
                    { _locationProvider.Kamchatka, () => colorService.AsiaColors },
                    { _locationProvider.MiddleEast, () => colorService.AsiaColors },
                    { _locationProvider.Mongolia, () => colorService.AsiaColors },
                    { _locationProvider.Siam, () => colorService.AsiaColors },
                    { _locationProvider.Siberia, () => colorService.AsiaColors },
                    { _locationProvider.Ural, () => colorService.AsiaColors },
                    { _locationProvider.Yakutsk, () => colorService.AsiaColors },
                    { _locationProvider.EasternAustralia, () => colorService.AustraliaColors },
                    { _locationProvider.Indonesia, () => colorService.AustraliaColors },
                    { _locationProvider.NewGuinea, () => colorService.AustraliaColors },
                    { _locationProvider.WesternAustralia, () => colorService.AustraliaColors },
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