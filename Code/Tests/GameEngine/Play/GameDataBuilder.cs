using System.Collections.Generic;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class GameDataBuilder
    {
        private IReadOnlyList<ITerritory> _territories;
        private readonly List<IPlayerGameData> _playerGameDatas = new List<IPlayerGameData>();
        private PlayerName _currentPlayerName;
        private IDeck _deck = new DeckBuilder().Build();

        public GameData Build()
        {
            return new GameData(_territories, _playerGameDatas, _currentPlayerName, _deck);
        }

        public GameDataBuilder Territories(params ITerritory[] territories)
        {
            _territories = territories;
            return this;
        }

        public GameDataBuilder AddPlayer(IPlayerGameData playerGameData)
        {
            _playerGameDatas.Add(playerGameData);
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