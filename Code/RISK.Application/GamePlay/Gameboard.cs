using System.Collections.Generic;

namespace RISK.Application.GamePlay
{
    public interface IGameboard {}

    public class Gameboard : IGameboard
    {
        private readonly List<GameboardTerritory> _gameboardTerritories;

        public Gameboard(List<GameboardTerritory> gameboardTerritories)
        {
            _gameboardTerritories = gameboardTerritories;
        }
    }
}