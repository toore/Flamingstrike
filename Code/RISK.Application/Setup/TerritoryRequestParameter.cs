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
        private readonly PlayerInSetup _playerInSetup;

        public TerritoryRequestParameter(IReadOnlyList<GameboardTerritory> gameboardTerritories, IReadOnlyList<ITerritory> enabledTerritories, PlayerInSetup playerInSetup)
        {
            GameboardTerritories = gameboardTerritories;
            _playerInSetup = playerInSetup;
            EnabledTerritories = enabledTerritories;
        }

        public IReadOnlyList<IGameboardTerritory> GameboardTerritories { get; }
        public IReadOnlyList<ITerritory> EnabledTerritories { get; }
        public IPlayer Player => _playerInSetup.Player;

        public int GetArmiesLeftToPlace()
        {
            return _playerInSetup.ArmiesToPlace;
        }
    }
}