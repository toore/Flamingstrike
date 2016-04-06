TODO
==
Game states:
+ DraftArmies 
+ Attacking
-   SendInArmiesToOccupy
-   DraftArmiesHoldingFiveOrMoreCards
- Fortifying
- TurnEnding? 
- GameOver


Good ideas?
==
Extract Fortifier? (border, different players)
+ GetNextOrFirst extension should be replaced by "Sequence" implementation


Not used?
==
AttackGameState::IsGameOver?
GameStateBase::ThrowIfTerritoriesDoesNotContain?


Add tests
==
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence