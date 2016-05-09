using System.Collections.Generic;

namespace RISK.Application.Play
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