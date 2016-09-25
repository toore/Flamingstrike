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
Fortify can not deselect Territory
Crash when ending turn in fortify mode

AlternateGameSetup:
- Dependency inject TerritoryResponder (Hollywood-principle)
- InitializeInfantryToPlace: Fix commented code

Sequence.cs - should be CQRS compliant?

IPlayer.cs - Should NOT contain game setup specific public(?) code!

Player name formatting service "Player: abc"?

What about a common starting point for setup and the game engine?



##Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
