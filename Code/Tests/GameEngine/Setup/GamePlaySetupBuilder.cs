using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup.Finished;

namespace Tests.GameEngine.Setup
{
    public class GamePlaySetupBuilder
    {
        private static readonly PlayerName _defaultPlayer = new PlayerName("default");
        private readonly List<PlayerName> _players = new List<PlayerName> { _defaultPlayer };
        private readonly List<Territory> _territories = new List<Territory>();

        public IGamePlaySetup Build()
        {
            return new GamePlaySetup(_players, _territories);
        }

        public GamePlaySetupBuilder WithPlayer(PlayerName player)
        {
            _players.Remove(_defaultPlayer);
            _players.Add(player);
            return this;
        }

        public GamePlaySetupBuilder WithTerritory(Territory territory)
        {
            _territories.Add(territory);
            return this;
        }
    }
}