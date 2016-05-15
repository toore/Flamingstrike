using System.Collections.Generic;
using RISK.Core;

namespace RISK.GameEngine.Play
{
    public interface IPlayerInfo
    {
        string Name { get; }
        IReadOnlyList<ICard> Cards { get; }
    }

    public class PlayerInfo : IPlayerInfo
    {
        public PlayerInfo(string name, IReadOnlyList<ICard> cards)
        {
            Name = name;
            Cards = cards;
        }

        public string Name { get; }
        public IReadOnlyList<ICard> Cards { get; }
    }
}