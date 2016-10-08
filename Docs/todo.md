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
- Show number of armies to draft
- Number of armies to move in after attack or when fortifying should be selectable
- After attacking and winning, auto-select conquered territory
- Player name formatting service "Player: abc"?
- What about a common starting point for setup and the game engine?
- WorldMapViewModelFactory::Update should have type Maybe for selectedRegion?


##Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
