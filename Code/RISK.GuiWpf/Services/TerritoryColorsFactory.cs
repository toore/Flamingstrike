using System;
using System.Collections.Generic;
using RISK.Application;
using RISK.Application.Entities;

namespace GuiWpf.Services
{
    public interface ITerritoryColorsFactory
    {
        ITerritoryColors Create(IWorldMap worldMap, ITerritory territory);
    }

    public class TerritoryColorsFactory : ITerritoryColorsFactory
    {
        private readonly IColorService _colorService;

        public TerritoryColorsFactory(IColorService colorService)
        {
            _colorService = colorService;
        }

        public ITerritoryColors Create(IWorldMap worldMap, ITerritory territory)
        {
            if (territory.IsOccupied())
            {
                return _colorService.GetPlayerTerritoryColors(territory.Occupant);
            }

            var colors = new Dictionary<ITerritory, Func<ITerritoryColors>>
            {
                { worldMap.Alaska, () => _colorService.NorthAmericaColors },
                { worldMap.Alberta, () => _colorService.NorthAmericaColors },
                { worldMap.CentralAmerica, () => _colorService.NorthAmericaColors },
                { worldMap.EasternUnitedStates, () => _colorService.NorthAmericaColors },
                { worldMap.Greenland, () => _colorService.NorthAmericaColors },
                { worldMap.NorthwestTerritory, () => _colorService.NorthAmericaColors },
                { worldMap.Ontario, () => _colorService.NorthAmericaColors },
                { worldMap.Quebec, () => _colorService.NorthAmericaColors },
                { worldMap.WesternUnitedStates, () => _colorService.NorthAmericaColors },
                { worldMap.Argentina, () => _colorService.SouthAmericaColors },
                { worldMap.Brazil, () => _colorService.SouthAmericaColors },
                { worldMap.Peru, () => _colorService.SouthAmericaColors },
                { worldMap.Venezuela, () => _colorService.SouthAmericaColors },
                { worldMap.GreatBritain, () => _colorService.EuropeColors },
                { worldMap.Iceland, () => _colorService.EuropeColors },
                { worldMap.NorthernEurope, () => _colorService.EuropeColors },
                { worldMap.Scandinavia, () => _colorService.EuropeColors },
                { worldMap.SouthernEurope, () => _colorService.EuropeColors },
                { worldMap.Ukraine, () => _colorService.EuropeColors },
                { worldMap.WesternEurope, () => _colorService.EuropeColors },
                { worldMap.Congo, () => _colorService.AfricaColors },
                { worldMap.EastAfrica, () => _colorService.AfricaColors },
                { worldMap.Egypt, () => _colorService.AfricaColors },
                { worldMap.Madagascar, () => _colorService.AfricaColors },
                { worldMap.NorthAfrica, () => _colorService.AfricaColors },
                { worldMap.SouthAfrica, () => _colorService.AfricaColors },
                { worldMap.Afghanistan, () => _colorService.AsiaColors },
                { worldMap.China, () => _colorService.AsiaColors },
                { worldMap.India, () => _colorService.AsiaColors },
                { worldMap.Irkutsk, () => _colorService.AsiaColors },
                { worldMap.Japan, () => _colorService.AsiaColors },
                { worldMap.Kamchatka, () => _colorService.AsiaColors },
                { worldMap.MiddleEast, () => _colorService.AsiaColors },
                { worldMap.Mongolia, () => _colorService.AsiaColors },
                { worldMap.Siam, () => _colorService.AsiaColors },
                { worldMap.Siberia, () => _colorService.AsiaColors },
                { worldMap.Ural, () => _colorService.AsiaColors },
                { worldMap.Yakutsk, () => _colorService.AsiaColors },
                { worldMap.EasternAustralia, () => _colorService.AustraliaColors },
                { worldMap.Indonesia, () => _colorService.AustraliaColors },
                { worldMap.NewGuinea, () => _colorService.AustraliaColors },
                { worldMap.WesternAustralia, () => _colorService.AustraliaColors },
            };

            return colors[territory]();
        }
    }
}