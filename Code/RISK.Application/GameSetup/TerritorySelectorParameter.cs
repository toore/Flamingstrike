using System.Collections.Generic;
using RISK.Application.GamePlay;
using RISK.Application.World;

namespace RISK.Application.GameSetup
{
    public interface ITerritoryRequestParameter
    {
        IGameboard Gameboard { get; }
        IReadOnlyList<ITerritory> EnabledTerritories { get; }
        IPlayer Player { get; }
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly GameSetupPlayer _gameSetupPlayer;

        public TerritoryRequestParameter(IGameboard gameboard, IReadOnlyList<ITerritory> enabledTerritories, GameSetupPlayer gameSetupPlayer)
        {
            Gameboard = gameboard;
            _gameSetupPlayer = gameSetupPlayer;
            EnabledTerritories = enabledTerritories;
        }

        public IGameboard Gameboard { get; }
        public IReadOnlyList<ITerritory> EnabledTerritories { get; }
        public IPlayer Player => _gameSetupPlayer.Player;

        public int GetArmiesLeftToPlace()
        {
            return _gameSetupPlayer.ArmiesToPlace;
        }
    }
}