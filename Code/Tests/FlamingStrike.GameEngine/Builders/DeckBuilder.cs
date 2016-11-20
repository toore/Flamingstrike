using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;

namespace Tests.FlamingStrike.GameEngine.Builders
{
    public class DeckBuilder
    {
        public IDeck Build()
        {
            return new Deck(new[] { new WildCard() });
        }
    }
}