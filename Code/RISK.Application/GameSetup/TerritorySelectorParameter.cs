using System.Collections.Generic;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace RISK.Application.GameSetup
{
    public interface ITerritoryRequestParameter
    {
        IEnumerable<ITerritory> EnabledTerritories { get; }
        IGameboard Gameboard { get; }
        IPlayerId GetPlayerThatTakesTurn();
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly Player _player;

        public TerritoryRequestParameter(IGameboard gameboard, IEnumerable<ITerritory> enabledTerritories, Player player)
        {
            Gameboard = gameboard;
            _player = player;
            EnabledTerritories = enabledTerritories;
        }

        public IGameboard Gameboard { get; }
        public IEnumerable<ITerritory> EnabledTerritories { get; }

        public IPlayerId GetPlayerThatTakesTurn()
        {
            return _player.PlayerId;
        }

        public int GetArmiesLeftToPlace()
        {
            return _player.GetNumberOfArmiesLeftToPlace();
        }
    }
}