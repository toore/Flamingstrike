using System;
using System.Collections.Generic;
using RISK.Application.World;

namespace GuiWpf.Services
{
    public interface ITerritoryColorsFactory
    {
        ITerritoryColors Create(ITerritoryId territoryId);
    }

    public class TerritoryColorsFactory : ITerritoryColorsFactory
    {
        private Dictionary<ITerritoryId, Func<GuiWpf.Services.ITerritoryColors>> _colors;

        public TerritoryColorsFactory(IColorService colorService, IWorldMap worldMap)
        {
            _colors = new Dictionary<ITerritoryId, Func<ITerritoryColors>>
            {
                { worldMap.Alaska, () => colorService.NorthAmericaColors },
                { worldMap.Alberta, () => colorService.NorthAmericaColors },
                { worldMap.CentralAmerica, () => colorService.NorthAmericaColors },
                { worldMap.EasternUnitedStates, () => colorService.NorthAmericaColors },
                { worldMap.Greenland, () => colorService.NorthAmericaColors },
                { worldMap.NorthwestTerritoryId, () => colorService.NorthAmericaColors },
                { worldMap.Ontario, () => colorService.NorthAmericaColors },
                { worldMap.Quebec, () => colorService.NorthAmericaColors },
                { worldMap.WesternUnitedStates, () => colorService.NorthAmericaColors },
                { worldMap.Argentina, () => colorService.SouthAmericaColors },
                { worldMap.Brazil, () => colorService.SouthAmericaColors },
                { worldMap.Peru, () => colorService.SouthAmericaColors },
                { worldMap.Venezuela, () => colorService.SouthAmericaColors },
                { worldMap.GreatBritain, () => colorService.EuropeColors },
                { worldMap.Iceland, () => colorService.EuropeColors },
                { worldMap.NorthernEurope, () => colorService.EuropeColors },
                { worldMap.Scandinavia, () => colorService.EuropeColors },
                { worldMap.SouthernEurope, () => colorService.EuropeColors },
                { worldMap.Ukraine, () => colorService.EuropeColors },
                { worldMap.WesternEurope, () => colorService.EuropeColors },
                { worldMap.Congo, () => colorService.AfricaColors },
                { worldMap.EastAfrica, () => colorService.AfricaColors },
                { worldMap.Egypt, () => colorService.AfricaColors },
                { worldMap.Madagascar, () => colorService.AfricaColors },
                { worldMap.NorthAfrica, () => colorService.AfricaColors },
                { worldMap.SouthAfrica, () => colorService.AfricaColors },
                { worldMap.Afghanistan, () => colorService.AsiaColors },
                { worldMap.China, () => colorService.AsiaColors },
                { worldMap.India, () => colorService.AsiaColors },
                { worldMap.Irkutsk, () => colorService.AsiaColors },
                { worldMap.Japan, () => colorService.AsiaColors },
                { worldMap.Kamchatka, () => colorService.AsiaColors },
                { worldMap.MiddleEast, () => colorService.AsiaColors },
                { worldMap.Mongolia, () => colorService.AsiaColors },
                { worldMap.Siam, () => colorService.AsiaColors },
                { worldMap.Siberia, () => colorService.AsiaColors },
                { worldMap.Ural, () => colorService.AsiaColors },
                { worldMap.Yakutsk, () => colorService.AsiaColors },
                { worldMap.EasternAustralia, () => colorService.AustraliaColors },
                { worldMap.Indonesia, () => colorService.AustraliaColors },
                { worldMap.NewGuinea, () => colorService.AustraliaColors },
                { worldMap.WesternAustralia, () => colorService.AustraliaColors },
            };
        }

        public ITerritoryColors Create(ITerritoryId territoryId)
        {
            //if (territory.IsOccupied())
            //{
            //    return _colorService.GetPlayerTerritoryColors(territory.Occupant);
            //}

            return _colors[territoryId]();
        }
    }
}