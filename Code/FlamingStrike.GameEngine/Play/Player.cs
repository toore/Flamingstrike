using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IPlayer
    {
        PlayerName PlayerName { get; }
        IReadOnlyList<ICard> Cards { get; }
    }

    public class Player : IPlayer
    {
        public Player(PlayerName playerName, IReadOnlyList<ICard> cards)
        {
            PlayerName = playerName;
            Cards = cards;
        }

        public PlayerName PlayerName { get; }

        public IReadOnlyList<ICard> Cards { get; }
    }
}