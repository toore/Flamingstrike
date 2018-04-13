using System;
using System.Collections.Generic;
using System.Linq;
using Toore.Shuffling;

namespace FlamingStrike.GameEngine.Play
{
    public interface IDeckFactory
    {
        IDeck Create();
    }

    public class DeckFactory : IDeckFactory
    {
        private readonly IReadOnlyList<Region> _regions;
        private readonly IShuffler _shuffler;
        private readonly List<CardSymbol> _cardSymbols;

        public DeckFactory(IReadOnlyList<Region> regions, IShuffler shuffler)
        {
            _regions = regions;
            _shuffler = shuffler;

            _cardSymbols = Enum.GetValues(typeof(CardSymbol)).Cast<CardSymbol>().ToList();
        }

        public IDeck Create()
        {
            var cards = _regions
                .Select(GetCard)
                .Concat(new[] { new WildCard(), new WildCard() })
                .Shuffle(_shuffler)
                .ToList();

            return new Deck(new Stack<ICard>(cards));
        }

        private ICard GetCard(Region region, int cardIndex)
        {
            return new StandardCard(region, _cardSymbols[cardIndex % _cardSymbols.Count]);
        }
    }
}