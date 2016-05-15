using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public class GameData
    {
        public GameData(IInGamePlayer currentPlayer, IReadOnlyList<IInGamePlayer> players, IReadOnlyList<ITerritory> territories, IDeck deck)
        {
            CurrentPlayer = currentPlayer;
            Players = players;
            Territories = territories;
            Deck = deck;
        }

        public IInGamePlayer CurrentPlayer { get; }

        public IReadOnlyList<IInGamePlayer> Players { get; }

        public IReadOnlyList<ITerritory> Territories { get; }

        public IDeck Deck { get; }
    }
}