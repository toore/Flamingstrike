using System.Collections.Generic;
using RISK.Core;
using RISK.GameEngine.Play;
using IPlayer = RISK.GameEngine.Play.IPlayer;

namespace RISK.Tests.Builders
{
    public class GameDataBuilder
    {
        private IPlayer _currentPlayer;
        private readonly List<IPlayer> _players = new List<IPlayer>();
        private readonly List<ITerritory> _territories = new List<ITerritory>();
        private IDeck _deck = new Deck(new[] { new WildCard() });

        public GameData Build()
        {
            return new GameData(_currentPlayer, _players, _territories, _deck);
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

        public GameDataBuilder WithDeck(IDeck deck)
        {
            _deck = deck;
            return this;
        }
    }
}