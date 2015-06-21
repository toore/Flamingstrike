using System.Collections.Generic;
using RISK.Application;
using RISK.Application.Setup;

namespace RISK.Tests.Builders
{
    public class GameSetupBuilder
    {
        private static readonly IPlayer _defaultPlayer = new Player("default");
        private readonly List<IPlayer> _players = new List<IPlayer> { _defaultPlayer };
        private readonly List<IGameboardSetupTerritory> _gameboardTerritories = new List<IGameboardSetupTerritory>();

        public IGameSetup Build()
        {
            return new GameSetup(_players, _gameboardTerritories);
        }

        public GameSetupBuilder WithPlayer(IPlayer player)
        {
            _players.Remove(_defaultPlayer);
            _players.Add(player);
            return this;
        }

        public GameSetupBuilder WithTerritory(GameboardSetupTerritory gameboardSetupTerritory)
        {
            _gameboardTerritories.Add(gameboardSetupTerritory);
            return this;
        }
    }
}