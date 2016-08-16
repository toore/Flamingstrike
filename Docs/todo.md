##TODO
Game states:
- TradeInSetsHoldingAtLeastThreeCards
+ DraftArmies
+ Attack
+ SendArmiesToOccupy
- TradeInSetsHoldingFiveOrMoreCards
- Fortify
- GameOver


SendArmiesToOccupyGameState
- should not be initiated if all the available armies were already moved into the territory
- should allow to end turn, if no desire to move into territory

AlternateGameSetup:
- Dependency inject TerritoryResponder
- InitializeInfantryToPlace: Fix commented code

Player name formatting service "Player: abc"?


##Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
