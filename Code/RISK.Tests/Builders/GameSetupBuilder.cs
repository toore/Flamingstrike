using System.Collections.Generic;
using NSubstitute;
using RISK.Application;
using RISK.Application.Setup;

namespace RISK.Tests.Builders
{
    public class GameSetupBuilder
    {
        private static readonly IPlayer _defaultPlayer = new Player("default");
        private readonly List<IPlayer> _players = new List<IPlayer> { _defaultPlayer };
        private readonly List<ITerritory> _territories = new List<ITerritory>();

        public IGamePlaySetup Build()
        {
            return new GamePlaySetup(_players, _territories);
        }

        public GameSetupBuilder WithPlayer(IPlayer player)
        {
            _players.Remove(_defaultPlayer);
            _players.Add(player);
            return this;
        }
    }
}