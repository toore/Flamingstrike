# TODO

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
- When attacking and not occupying, do not deselect territory
- After attacking and winning, auto-select (new) conquered territory
- Eliminated player should not take turn
- Select number of armies to move in after attack or when fortifying
- Continent information on board?
- What about a common starting point for setup and the game engine?
- Does GameStates really have to call back to Game?
- Show how territories are connected
- Give user hints or help messages. E.g. A selected territory with only one army can't attack any territory.
- Create Neutral AI player
- 
- Add Nuclear custom rules


# Not/partially tested
- GameStateFactory (CreateNextTurnGameState, etc)
- Sequence
- Deck
- DeckFactory
- GameStateConductor
- ArmyModifier
- Territory
