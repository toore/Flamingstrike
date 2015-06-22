using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play
{
    public interface IGameboard
    {
        IGameboardTerritory GetTerritory(ITerritory territory);
    }

    public class Gameboard : IGameboard
    {
        private readonly IReadOnlyList<IGameboardTerritory> _gameboardTerritories;

        public Gameboard(IReadOnlyList<IGameboardTerritory> gameboardTerritories)
        {
            _gameboardTerritories = gameboardTerritories;
        }

        public IGameboardTerritory GetTerritory(ITerritory territory)
        {
            return _gameboardTerritories.Single(x => x.Territory == territory);
        }
    }
}