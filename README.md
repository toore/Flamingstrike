# flaming-strike

Clone of a famous strategic game. Details and history about the original game can be found here: https://en.wikipedia.org/wiki/Risk_(game).

The game is based on the standard rules, but the intent is to add some homebrew or custom rules.

The game engine is built with .NET standard. WPF front end using Caliburn.Micro. The service is built with .Net Core. The client can host the game server in-process or use the server api.

Enjoy!

## TODO

The basics, game states/phases:

- [ ] TradeInSetsHoldingAtLeastThreeCards
- [x] DraftArmies
- [x] Attack
- [x] SendArmiesToOccupy
- [ ] TradeInSetsHoldingFiveOrMoreCards
- [x] Fortify
- [x] GameOver

Must haves:

- [ ] Show current player's cards, and enable trade-in set of cards
- [ ] Eliminated player should not take turn

Improvements:

- [ ] When attacking and not occupying, do not deselect territory
- [ ] After attacking and winning, auto-select (new) conquered territory
- [ ] Select number of armies to move in after attack or when fortifying
- [ ] Show continent information on board?
- [ ] Give user hints or help messages. E.g. A selected territory with only one army can't attack any territory.
- [ ] Create Neutral AI player
- [ ] Add Nuclear custom rules...What!?

Nice to haves:

- [ ] Multiplayer, not only hotseat