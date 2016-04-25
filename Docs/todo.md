TODO
====
Game states:
- TradeInSetsHoldingAtLeastThreeCards
+ DraftArmies
+ Attack
- SendInArmiesToOccupy
- TradeInSetsHoldingFiveOrMoreCards
- Fortify
- GameOver



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