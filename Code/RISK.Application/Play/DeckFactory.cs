using System;
using System.Linq;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Toore.Shuffling;

namespace RISK.Application.Play
{
    public interface IDeckFactory
    {
        IDeck Create();
    }

    public class DeckFactory : IDeckFactory
    {
        private readonly IRegions _regions;
        private readonly IShuffler _shuffler;

        public DeckFactory(IRegions regions, IShuffler shuffler)
        {
            _regions = regions;
            _shuffler = shuffler;
        }

        public IDeck Create()
        {
            var cardSymbols = GetCardSymbols();

            var cards = _regions.GetAll()
                .Select(region => new StandardCard(region, cardSymbols.Next()))
                .Cast<ICard>()
                .Concat(new[] { new WildCard(), new WildCard() })
                .Shuffle(_shuffler)
                .ToList();

            var deck = new Deck(cards);

            return deck;
        }

        private static Sequence<CardSymbol> GetCardSymbols()
        {
            var cardSymbols = Enum.GetValues(typeof(CardSymbol))
                .Cast<CardSymbol>()
                .ToArray();

            var sequence = new Sequence<CardSymbol>(cardSymbols);

            return sequence;
        }
    }
}