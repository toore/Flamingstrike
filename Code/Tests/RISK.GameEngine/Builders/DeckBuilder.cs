using RISK.GameEngine;
using RISK.GameEngine.Play;

namespace Tests.RISK.GameEngine.Builders
{
    public class DeckBuilder
    {
        public IDeck Build()
        {
            return new Deck(new[] { new WildCard() });
        }
    }
}