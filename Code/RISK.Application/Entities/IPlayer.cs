using System.Collections.Generic;

namespace RISK.Domain.Entities
{
    public interface IPlayer
    {
        string Name { get; }
        IEnumerable<Card> Cards { get; }
        void AddCard(Card card);
    }
}