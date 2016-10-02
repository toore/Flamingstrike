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
- AlternateGameSetupView does not respect disabled buttons!?
- Fortify can not deselect Territory
- Crash when ending turn in fortify mode


IPlayer.cs - Should NOT contain game setup specific public(?) code!
Player name formatting service "Player: abc"?
What about a common starting point for setup and the game engine?
WorldMapViewModelFactory::Update should have type Maybe for selectedRegion


##Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
