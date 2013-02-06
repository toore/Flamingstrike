using System;
using System.Collections.Generic;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace GuiWpf.Services
{
    public class TerritoryColorsFactory : ITerritoryColorsFactory
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IColorService _colorService;
        private readonly Dictionary<ILocation, Func<ITerritoryColors>> _colors;

        public TerritoryColorsFactory(ILocationRepository locationRepository, IColorService colorService)
        {
            _locationRepository = locationRepository;
            _colorService = colorService;

            _colors = new Dictionary<ILocation, Func<ITerritoryColors>>
                {
                    { _locationRepository.Alaska, () => colorService.NorthAmericaColors },
                    { _locationRepository.Alberta, () => colorService.NorthAmericaColors },
                    { _locationRepository.CentralAmerica, () => colorService.NorthAmericaColors },
                    { _locationRepository.EasternUnitedStates, () => colorService.NorthAmericaColors },
                    { _locationRepository.Greenland, () => colorService.NorthAmericaColors },
                    { _locationRepository.NorthwestTerritory, () => colorService.NorthAmericaColors },
                    { _locationRepository.Ontario, () => colorService.NorthAmericaColors },
                    { _locationRepository.Quebec, () => colorService.NorthAmericaColors },
                    { _locationRepository.WesternUnitedStates, () => colorService.NorthAmericaColors },
                    { _locationRepository.Argentina, () => colorService.SouthAmericaColors },
                    { _locationRepository.Brazil, () => colorService.SouthAmericaColors },
                    { _locationRepository.Peru, () => colorService.SouthAmericaColors },
                    { _locationRepository.Venezuela, () => colorService.SouthAmericaColors },
                    { _locationRepository.GreatBritain, () => colorService.EuropeColors },
                    { _locationRepository.Iceland, () => colorService.EuropeColors },
                    { _locationRepository.NorthernEurope, () => colorService.EuropeColors },
                    { _locationRepository.Scandinavia, () => colorService.EuropeColors },
                    { _locationRepository.SouthernEurope, () => colorService.EuropeColors },
                    { _locationRepository.Ukraine, () => colorService.EuropeColors },
                    { _locationRepository.WesternEurope, () => colorService.EuropeColors },
                    { _locationRepository.Congo, () => colorService.AfricaColors },
                    { _locationRepository.EastAfrica, () => colorService.AfricaColors },
                    { _locationRepository.Egypt, () => colorService.AfricaColors },
                    { _locationRepository.Madagascar, () => colorService.AfricaColors },
                    { _locationRepository.NorthAfrica, () => colorService.AfricaColors },
                    { _locationRepository.SouthAfrica, () => colorService.AfricaColors },
                    { _locationRepository.Afghanistan, () => colorService.AsiaColors },
                    { _locationRepository.China, () => colorService.AsiaColors },
                    { _locationRepository.India, () => colorService.AsiaColors },
                    { _locationRepository.Irkutsk, () => colorService.AsiaColors },
                    { _locationRepository.Japan, () => colorService.AsiaColors },
                    { _locationRepository.Kamchatka, () => colorService.AsiaColors },
                    { _locationRepository.MiddleEast, () => colorService.AsiaColors },
                    { _locationRepository.Mongolia, () => colorService.AsiaColors },
                    { _locationRepository.Siam, () => colorService.AsiaColors },
                    { _locationRepository.Siberia, () => colorService.AsiaColors },
                    { _locationRepository.Ural, () => colorService.AsiaColors },
                    { _locationRepository.Yakutsk, () => colorService.AsiaColors },
                    { _locationRepository.EasternAustralia, () => colorService.AustraliaColors },
                    { _locationRepository.Indonesia, () => colorService.AustraliaColors },
                    { _locationRepository.NewGuinea, () => colorService.AustraliaColors },
                    { _locationRepository.WesternAustralia, () => colorService.AustraliaColors },
                };
        }

        public ITerritoryColors Create(ITerritory territory)
        {
            if (territory.HasOwner)
            {
                return _colorService.GetPlayerTerritoryColors(territory.Owner);
            }

            return _colors[territory.Location]();
        }
    }
}