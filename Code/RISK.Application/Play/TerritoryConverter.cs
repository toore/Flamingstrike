using System.Collections.Generic;
using System.Linq;
using RISK.Application.Setup;

namespace RISK.Application.Play
{
    public interface ITerritoryConverter
    {
        List<GameboardTerritory> Convert(IEnumerable<IGameboardSetupTerritory> gameboardTerritories);
    }

    public class TerritoryConverter : ITerritoryConverter
    {
        public List<GameboardTerritory> Convert(IEnumerable<IGameboardSetupTerritory> gameboardTerritories)
        {
            return gameboardTerritories.Select(Convert).ToList();
        }

        private static GameboardTerritory Convert(IGameboardSetupTerritory gameboardSetupTerritory)
        {
            return new GameboardTerritory(
                gameboardSetupTerritory.Territory,
                gameboardSetupTerritory.Player,
                gameboardSetupTerritory.Armies);
        }
    }
}