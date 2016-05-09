using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class SendArmiesToOccupyGameState : GameStateBase
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IArmyModifier _armyModifier;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;

        public SendArmiesToOccupyGameState(IGameStateConductor gameStateConductor, IArmyModifier armyModifier, GameData gameData, IRegion attackingRegion, IRegion occupiedRegion)
            : base(gameData)
        {
            _gameStateConductor = gameStateConductor;
            _armyModifier = armyModifier;
            _attackingRegion = attackingRegion;
            _occupiedRegion = occupiedRegion;
        }

        public override bool CanSendArmiesToOccupy()
        {
            return true;
        }

        public override IGameState SendArmiesToOccupy(int numberOfArmies)
        {
            var updatedTerritories = _armyModifier.SendInAdditionalArmiesToOccupy(Territories, _attackingRegion, _occupiedRegion, numberOfArmies);
            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories, Deck);

            var attackPhase = _gameStateConductor.ContinueWithAttackPhase(gameData, ConqueringAchievement.AwardCardAtEndOfTurn);

            return attackPhase;
        }
    }
}