TODO
====
Game states:
- TradeInSetsHoldingAtLeastThreeCards
+ DraftArmies
+ Attack
+ SendArmiesToOccupy
- TradeInSetsHoldingFiveOrMoreCards
- Fortify
- GameOver


SendArmiesToOccupyGameState, should not be initiated if all available armies, were already moved into the territory

GameStateBase should not contain helper methods for game data!

Good ideas?
===========
Extract Fortifier? (border, different players)
+ GetNextOrFirst extension should be replaced by "Sequence" implementation


Not used?
=========
AttackGameState::IsGameOver?
GameStateBase::ThrowIfTerritoriesDoesNotContain?


Not/partially tested
====================
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory