using System.Collections.Generic;
using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace Tests.RISK.GameEngine.Builders
{
    public class GameDataBuilder
    {
        private IReadOnlyList<ITerritory> _territories;
        private IReadOnlyList<IPlayerGameData> _players;
        private IPlayer _currentPlayer;
        private IReadOnlyList<ICard> _cards;

        public GameData Build()
        {
            return new GameData(_territories, _players, _currentPlayer, _cards);
        }

        public GameDataBuilder CurrentPlayer(IPlayer player)
        {
            _currentPlayer = player;
            return this;
        }

        public GameDataBuilder Territories(params ITerritory[] territories)
        {
            _territories = territories;
            return this;
        }
    }
}