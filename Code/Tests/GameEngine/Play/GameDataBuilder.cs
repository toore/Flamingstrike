using System.Collections.Generic;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class GameDataBuilder
    {
        private IReadOnlyList<ITerritory> _territories;
        private readonly List<IPlayer> _players = new List<IPlayer>();
        private PlayerName _currentPlayerName;
        private IDeck _deck = new DeckBuilder().Build();

        public GameData Build()
        {
            return new GameData(_territories, _players, _currentPlayerName, _deck);
        }

        public GameDataBuilder Territories(params ITerritory[] territories)
        {
            _territories = territories;
            return this;
        }

        public GameDataBuilder AddPlayer(IPlayer player)
        {
            _players.Add(player);
            return this;
        }

        public GameDataBuilder CurrentPlayer(PlayerName playerName)
        {
            _currentPlayerName = playerName;
            return this;
        }

        public GameDataBuilder Deck(IDeck deck)
        {
            _deck = deck;
            return this;
        }
    }
}