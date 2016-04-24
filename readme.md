TODO
====
Game states:
+ DraftArmies 
+ Attacking
-   SendInArmiesToOccupy
-   DraftArmiesHoldingFiveOrMoreCards
- 	GameOver
- Fortifying
- TurnEnding? 

CardFactory-->CardDeck? (fixed cards)


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