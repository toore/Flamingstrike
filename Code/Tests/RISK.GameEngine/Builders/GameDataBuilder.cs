using System.Collections.Generic;
using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace Tests.RISK.GameEngine.Builders
{
    public class GameDataBuilder
    {
        private IReadOnlyList<ITerritory> _territories;
        private readonly List<IPlayerGameData> _playerGameDatas = new List<IPlayerGameData>();
        private IPlayer _currentPlayer;
        private IReadOnlyList<ICard> _cards;

        public GameData Build()
        {
            return new GameData(_territories, _playerGameDatas, _currentPlayer, _cards);
        }

        public GameDataBuilder Territories(params ITerritory[] territories)
        {
            _territories = territories;
            return this;
        }

        public GameDataBuilder AddPlayer(PlayerGameData playerGameData)
        {
            _playerGameDatas.Add(playerGameData);
            return this;
        }

        public GameDataBuilder CurrentPlayer(IPlayer player)
        {
            _currentPlayer = player;
            return this;
        }
    }
}