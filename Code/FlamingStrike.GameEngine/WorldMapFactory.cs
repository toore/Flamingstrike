namespace FlamingStrike.GameEngine
{
    public class WorldMapFactory
    {
        public IWorldMap Create()
        {
            var worldMap = new WorldMap();

            worldMap.AddBorder(Region.Alaska, Region.Alberta);
            worldMap.AddBorder(Region.Alaska, Region.NorthwestTerritory);
            worldMap.AddBorder(Region.Alaska, Region.Kamchatka);
            worldMap.AddBorder(Region.Alberta, Region.NorthwestTerritory);
            worldMap.AddBorder(Region.Alberta, Region.Ontario);
            worldMap.AddBorder(Region.Alberta, Region.WesternUnitedStates);
            worldMap.AddBorder(Region.CentralAmerica, Region.EasternUnitedStates);
            worldMap.AddBorder(Region.CentralAmerica, Region.WesternUnitedStates);
            worldMap.AddBorder(Region.CentralAmerica, Region.Venezuela);
            worldMap.AddBorder(Region.EasternUnitedStates, Region.Ontario);
            worldMap.AddBorder(Region.EasternUnitedStates, Region.Quebec);
            worldMap.AddBorder(Region.EasternUnitedStates, Region.WesternUnitedStates);
            worldMap.AddBorder(Region.Greenland, Region.NorthwestTerritory);
            worldMap.AddBorder(Region.Greenland, Region.Ontario);
            worldMap.AddBorder(Region.Greenland, Region.Quebec);
            worldMap.AddBorder(Region.Greenland, Region.Iceland);
            worldMap.AddBorder(Region.NorthwestTerritory, Region.Ontario);
            worldMap.AddBorder(Region.Ontario, Region.Quebec);
            worldMap.AddBorder(Region.Ontario, Region.WesternUnitedStates);

            worldMap.AddBorder(Region.Argentina, Region.Brazil);
            worldMap.AddBorder(Region.Argentina, Region.Peru);
            worldMap.AddBorder(Region.Brazil, Region.Peru);
            worldMap.AddBorder(Region.Brazil, Region.Venezuela);
            worldMap.AddBorder(Region.Brazil, Region.NorthAfrica);
            worldMap.AddBorder(Region.Peru, Region.Venezuela);

            worldMap.AddBorder(Region.GreatBritain, Region.Iceland);
            worldMap.AddBorder(Region.GreatBritain, Region.NorthernEurope);
            worldMap.AddBorder(Region.GreatBritain, Region.Scandinavia);
            worldMap.AddBorder(Region.GreatBritain, Region.WesternEurope);
            worldMap.AddBorder(Region.Iceland, Region.Scandinavia);
            worldMap.AddBorder(Region.NorthernEurope, Region.Scandinavia);
            worldMap.AddBorder(Region.NorthernEurope, Region.SouthernEurope);
            worldMap.AddBorder(Region.NorthernEurope, Region.Ukraine);
            worldMap.AddBorder(Region.NorthernEurope, Region.WesternEurope);
            worldMap.AddBorder(Region.Scandinavia, Region.Ukraine);
            worldMap.AddBorder(Region.SouthernEurope, Region.Ukraine);
            worldMap.AddBorder(Region.SouthernEurope, Region.WesternEurope);
            worldMap.AddBorder(Region.SouthernEurope, Region.Egypt);
            worldMap.AddBorder(Region.SouthernEurope, Region.NorthAfrica);
            worldMap.AddBorder(Region.SouthernEurope, Region.MiddleEast);
            worldMap.AddBorder(Region.Ukraine, Region.Afghanistan);
            worldMap.AddBorder(Region.Ukraine, Region.MiddleEast);
            worldMap.AddBorder(Region.Ukraine, Region.Ural);
            worldMap.AddBorder(Region.WesternEurope, Region.NorthAfrica);

            worldMap.AddBorder(Region.Congo, Region.EastAfrica);
            worldMap.AddBorder(Region.Congo, Region.NorthAfrica);
            worldMap.AddBorder(Region.Congo, Region.SouthAfrica);
            worldMap.AddBorder(Region.EastAfrica, Region.Egypt);
            worldMap.AddBorder(Region.EastAfrica, Region.Madagascar);
            worldMap.AddBorder(Region.EastAfrica, Region.NorthAfrica);
            worldMap.AddBorder(Region.EastAfrica, Region.SouthAfrica);
            worldMap.AddBorder(Region.EastAfrica, Region.MiddleEast);
            worldMap.AddBorder(Region.Egypt, Region.NorthAfrica);
            worldMap.AddBorder(Region.Egypt, Region.MiddleEast);
            worldMap.AddBorder(Region.Madagascar, Region.SouthAfrica);

            worldMap.AddBorder(Region.Afghanistan, Region.China);
            worldMap.AddBorder(Region.Afghanistan, Region.India);
            worldMap.AddBorder(Region.Afghanistan, Region.MiddleEast);
            worldMap.AddBorder(Region.Afghanistan, Region.Ural);
            worldMap.AddBorder(Region.China, Region.India);
            worldMap.AddBorder(Region.China, Region.Mongolia);
            worldMap.AddBorder(Region.China, Region.Siam);
            worldMap.AddBorder(Region.China, Region.Siberia);
            worldMap.AddBorder(Region.China, Region.Ural);
            worldMap.AddBorder(Region.India, Region.MiddleEast);
            worldMap.AddBorder(Region.India, Region.Siam);
            worldMap.AddBorder(Region.Irkutsk, Region.Kamchatka);
            worldMap.AddBorder(Region.Irkutsk, Region.Mongolia);
            worldMap.AddBorder(Region.Irkutsk, Region.Siberia);
            worldMap.AddBorder(Region.Irkutsk, Region.Yakutsk);
            worldMap.AddBorder(Region.Japan, Region.Kamchatka);
            worldMap.AddBorder(Region.Japan, Region.Mongolia);
            worldMap.AddBorder(Region.Kamchatka, Region.Mongolia);
            worldMap.AddBorder(Region.Kamchatka, Region.Yakutsk);
            worldMap.AddBorder(Region.Mongolia, Region.Siberia);
            worldMap.AddBorder(Region.Siam, Region.Indonesia);
            worldMap.AddBorder(Region.Siberia, Region.Ural);
            worldMap.AddBorder(Region.Siberia, Region.Yakutsk);

            worldMap.AddBorder(Region.EasternAustralia, Region.NewGuinea);
            worldMap.AddBorder(Region.EasternAustralia, Region.WesternAustralia);
            worldMap.AddBorder(Region.Indonesia, Region.NewGuinea);
            worldMap.AddBorder(Region.Indonesia, Region.WesternAustralia);
            worldMap.AddBorder(Region.NewGuinea, Region.WesternAustralia);

            return worldMap;
        }
    }
}