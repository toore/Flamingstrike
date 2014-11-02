using System;
using System.Collections.Generic;
using RISK.Application.Entities;

namespace GuiWpf.Services
{
    public class TerritoryColorsFactory : ITerritoryColorsFactory
    {
        private readonly IColorService _colorService;
        private readonly Dictionary<ITerritory, Func<ITerritoryColors>> _colors;

        public TerritoryColorsFactory(RISK.Application.Territories territories, IColorService colorService)
        {
            _colorService = colorService;

            _colors = new Dictionary<ITerritory, Func<ITerritoryColors>>
            {
                { territories.Alaska, () => colorService.NorthAmericaColors },
                { territories.Alberta, () => colorService.NorthAmericaColors },
                { territories.CentralAmerica, () => colorService.NorthAmericaColors },
                { territories.EasternUnitedStates, () => colorService.NorthAmericaColors },
                { territories.Greenland, () => colorService.NorthAmericaColors },
                { territories.NorthwestTerritory, () => colorService.NorthAmericaColors },
                { territories.Ontario, () => colorService.NorthAmericaColors },
                { territories.Quebec, () => colorService.NorthAmericaColors },
                { territories.WesternUnitedStates, () => colorService.NorthAmericaColors },
                { territories.Argentina, () => colorService.SouthAmericaColors },
                { territories.Brazil, () => colorService.SouthAmericaColors },
                { territories.Peru, () => colorService.SouthAmericaColors },
                { territories.Venezuela, () => colorService.SouthAmericaColors },
                { territories.GreatBritain, () => colorService.EuropeColors },
                { territories.Iceland, () => colorService.EuropeColors },
                { territories.NorthernEurope, () => colorService.EuropeColors },
                { territories.Scandinavia, () => colorService.EuropeColors },
                { territories.SouthernEurope, () => colorService.EuropeColors },
                { territories.Ukraine, () => colorService.EuropeColors },
                { territories.WesternEurope, () => colorService.EuropeColors },
                { territories.Congo, () => colorService.AfricaColors },
                { territories.EastAfrica, () => colorService.AfricaColors },
                { territories.Egypt, () => colorService.AfricaColors },
                { territories.Madagascar, () => colorService.AfricaColors },
                { territories.NorthAfrica, () => colorService.AfricaColors },
                { territories.SouthAfrica, () => colorService.AfricaColors },
                { territories.Afghanistan, () => colorService.AsiaColors },
                { territories.China, () => colorService.AsiaColors },
                { territories.India, () => colorService.AsiaColors },
                { territories.Irkutsk, () => colorService.AsiaColors },
                { territories.Japan, () => colorService.AsiaColors },
                { territories.Kamchatka, () => colorService.AsiaColors },
                { territories.MiddleEast, () => colorService.AsiaColors },
                { territories.Mongolia, () => colorService.AsiaColors },
                { territories.Siam, () => colorService.AsiaColors },
                { territories.Siberia, () => colorService.AsiaColors },
                { territories.Ural, () => colorService.AsiaColors },
                { territories.Yakutsk, () => colorService.AsiaColors },
                { territories.EasternAustralia, () => colorService.AustraliaColors },
                { territories.Indonesia, () => colorService.AustraliaColors },
                { territories.NewGuinea, () => colorService.AustraliaColors },
                { territories.WesternAustralia, () => colorService.AustraliaColors },
            };
        }

        public ITerritoryColors Create(ITerritory territory)
        {
            if (territory.IsOccupied())
            {
                return _colorService.GetPlayerTerritoryColors(territory.Occupant);
            }

            return _colors[territory]();
        }
    }
}