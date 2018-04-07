using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.GameEngine.Play
{
    public class DeckBuilder
    {
        public IDeck Build()
        {
            return new Deck(new[] { new WildCard() });
        }
    }
}