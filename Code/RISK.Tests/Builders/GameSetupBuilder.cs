using System.Collections.Generic;
using NSubstitute;
using RISK.Application;
using RISK.Application.Setup;

namespace RISK.Tests.Builders
{
    public class GameSetupBuilder
    {
        private static readonly IPlayerId _defaultPlayerId = new PlayerId("default");
        private readonly List<IPlayerId> _players = new List<IPlayerId> { _defaultPlayerId };
        private readonly List<ITerritory> _territories = new List<ITerritory>();

        public IGamePlaySetup Build()
        {
            return new GamePlaySetup(_players, _territories);
        }

        public GameSetupBuilder WithPlayer(IPlayerId playerId)
        {
            _players.Remove(_defaultPlayerId);
            _players.Add(playerId);
            return this;
        }
    }
}