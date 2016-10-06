using System.Collections.Generic;

namespace RISK.GameEngine
{
    public interface IPlayer
    {
        string Name { get; }
        IEnumerable<ICard> Cards { get; }
    }

    public class Player : IPlayer
    {
        private readonly IList<ICard> _cards = new List<ICard>();

        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public IEnumerable<ICard> Cards => _cards;
    }
}