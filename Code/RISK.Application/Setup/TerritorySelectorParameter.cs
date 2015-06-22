using System.Collections.Generic;
using RISK.Application.Play;
using RISK.Application.World;

namespace RISK.Application.Setup
{
    public interface ITerritoryRequestParameter
    {
        IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
        IReadOnlyList<ITerritory> EnabledTerritories { get; }
        IPlayer Player { get; }
        int GetArmiesLeftToPlace();
    }

    public class TerritoryRequestParameter : ITerritoryRequestParameter
    {
        private readonly GameSetupPlayer _gameSetupPlayer;

        public TerritoryRequestParameter(IReadOnlyList<GameboardTerritory> gameboardTerritories, IReadOnlyList<ITerritory> enabledTerritories, GameSetupPlayer gameSetupPlayer)
        {
            GameboardTerritories = gameboardTerritories;
            _gameSetupPlayer = gameSetupPlayer;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
        public IReadOnlyList<ITerritory> EnabledTerritories { get; }
        public IPlayer Player => _gameSetupPlayer.Player;

        public int GetArmiesLeftToPlace()
        {
            return _gameSetupPlayer.ArmiesToPlace;
        }
    }
}