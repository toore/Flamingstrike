using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public class GameData
    {
        public GameData(IPlayer currentPlayer, IReadOnlyList<IPlayer> players, IReadOnlyList<ITerritory> territories, IDeck deck)
        {
            CurrentPlayer = currentPlayer;
            Players = players;
            Territories = territories;
            Deck = deck;
        }

        public IPlayer CurrentPlayer { get; }

        public IReadOnlyList<IPlayer> Players { get; }

        public IReadOnlyList<ITerritory> Territories { get; }

        public IDeck Deck { get; }
    }
}