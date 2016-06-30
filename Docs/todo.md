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
- should not be initiated if all available armies, were already moved into the territory
- should allow to end turn, if no desire to move into territory


##Good ideas?
Extract Fortifier? (border, different players)
+ GetNextOrFirst extension should be replaced by "Sequence" implementation


##Not used?
AttackGameState::IsGameOver?


##Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
