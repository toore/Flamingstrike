using System.Collections.Generic;

namespace FlamingStrike.GameEngine.Play
{
    public interface IPlayerGameData
    {
        PlayerName PlayerName { get; }
        IReadOnlyList<ICard> Cards { get; }
    }

    public class PlayerGameData : IPlayerGameData
    {
        public PlayerGameData(PlayerName playerName, IReadOnlyList<ICard> cards)
        {
            PlayerName = playerName;
            Cards = cards;
        }

        public PlayerName PlayerName { get; }

        public IReadOnlyList<ICard> Cards { get; }
    }
}