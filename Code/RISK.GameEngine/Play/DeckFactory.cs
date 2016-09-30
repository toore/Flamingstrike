using System;
using System.Linq;
using RISK.Core;
using Toore.Shuffling;

namespace RISK.GameEngine.Play
{
    public interface IDeckFactory
    {
        IDeck Create();
    }

    public class DeckFactory : IDeckFactory
    {
        private readonly IRegions _regions;
        private readonly IShuffle _shuffler;

        public DeckFactory(IRegions regions, IShuffle shuffler)
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

        private static CircularBuffer<CardSymbol> GetCardSymbols()
        {
            var cardSymbols = Enum.GetValues(typeof(CardSymbol))
                .Cast<CardSymbol>()
                .ToArray();

            return new CircularBuffer<CardSymbol>(cardSymbols);
        }
    }
}