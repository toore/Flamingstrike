using System.Collections.Generic;
using RISK.Application;
using RISK.Application.Setup;

namespace RISK.Tests.Builders
{
    public class GamePlaySetupBuilder
    {
        private static readonly IPlayer _defaultPlayer = new Player("default");
        private readonly List<IPlayer> _players = new List<IPlayer> { _defaultPlayer };
        private readonly List<ITerritory> _territories = new List<ITerritory>();

        public IGamePlaySetup Build()
        {
            return new GamePlaySetup(_players, _territories);
        }

        public GamePlaySetupBuilder WithPlayer(IPlayer player)
        {
            _players.Remove(_defaultPlayer);
            _players.Add(player);
            return this;
        }

        public GamePlaySetupBuilder WithTerritory(ITerritory territory)
        {
            _territories.Add(territory);
            return this;
        }
    }
}