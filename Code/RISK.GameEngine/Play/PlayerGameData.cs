using System.Collections.Generic;

namespace RISK.GameEngine.Play
{
    public interface IPlayerGameData
    {
        IPlayer Player { get; }
        IReadOnlyList<ICard> Cards { get; }
    }

    public class PlayerGameData : IPlayerGameData
    {
        public PlayerGameData(IPlayer player, IReadOnlyList<ICard> cards)
        {
            Player = player;
            Cards = cards;
        }

        public IPlayer Player { get; }
        public IReadOnlyList<ICard> Cards { get; }
    }
}