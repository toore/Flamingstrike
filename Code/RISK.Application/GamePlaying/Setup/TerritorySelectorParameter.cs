using System.Collections.Generic;

namespace RISK.Application.GamePlaying.Setup
{
    public interface ITerritorySelectorParameter
    {
        IEnumerable<ITerritory> EnabledTerritories { get; }
        IWorldMap WorldMap { get; }
        IPlayer GetPlayerThatTakesTurn();
        int GetArmiesLeft();
    }

    public class TerritorySelectorParameter : ITerritorySelectorParameter
    {
        private readonly Player _player;

        public TerritorySelectorParameter(IWorldMap worldMap, IEnumerable<ITerritory> enabledTerritories, Player player)
        {
            WorldMap = worldMap;
            _player = player;
            EnabledTerritories = enabledTerritories;
        }

        public IWorldMap WorldMap { get; private set; }
        public IEnumerable<ITerritory> EnabledTerritories { get; private set; }

        public IPlayer GetPlayerThatTakesTurn()
        {
            return _player.GetPlayer();
        }

        public int GetArmiesLeft()
        {
            return _player.ArmiesToPlace;
        }

    }
}