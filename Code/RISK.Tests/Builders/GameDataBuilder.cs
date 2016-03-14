using System.Collections.Generic;
using RISK.Application;
using RISK.Application.Play;

namespace RISK.Tests.Builders
{
    public class GameDataBuilder
    {
        private IPlayer _currentPlayer;
        private readonly List<IPlayer> _players = new List<IPlayer>();
        private readonly List<ITerritory> _territories = new List<ITerritory>();

        public GameData Build()
        {
            return new GameData(_currentPlayer, _players, _territories);
        }

        public GameDataBuilder CurrentPlayer(IPlayer currentPlayer)
        {
            _currentPlayer = currentPlayer;
            return this;
        }

        public GameDataBuilder WithPlayer(IPlayer player)
        {
            _players.Add(player);
            return this;
        }

        public GameDataBuilder WithTerritory(ITerritory territory)
        {
            _territories.Add(territory);
            return this;
        }
    }
}