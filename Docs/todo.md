##TODO
Game states/phases:
- TradeInSetsHoldingAtLeastThreeCards
+ DraftArmies
+ Attack
+ SendArmiesToOccupy
- TradeInSetsHoldingFiveOrMoreCards
+ Fortify
+ GameOver

Issues:
- Show player cards!, and enable trade-in of cards

Improvements:
- When attacking and not occupying, keep attacking territory selection
- After attacking and winning, auto-select conquered territory
- Color regions per player (show color for player next to player name)
- Select number of armies to move in after attack or when fortifying
- Continent information on board
- Player name formatting service "Player: abc"?
- What about a common starting point for setup and the game engine?
- WorldMapViewModelFactory::Update should have type Maybe for selectedRegion?

- What about making changes to the states of Game with invalid game api objects (immutable core?)
     - Deck?

- Eliminated player should not take turn
- private bool _cardHasBeenAwardedThisTurn;  ...should not be used?


##Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
