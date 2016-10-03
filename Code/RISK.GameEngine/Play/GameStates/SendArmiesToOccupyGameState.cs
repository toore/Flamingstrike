namespace RISK.GameEngine.Play.GameStates
{
    public interface ISendArmiesToOccupyGameState
    {
        void SendAdditionalArmiesToOccupy(int numberOfArmies);
    }

    public class SendArmiesToOccupyGameState : ISendArmiesToOccupyGameState
    {
        private readonly ITerritoriesContext _territoriesContext;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly ITerritoryOccupier _territoryOccupier;
        private readonly IRegion _attackingRegion;
        private readonly IRegion _occupiedRegion;

        public SendArmiesToOccupyGameState(
            ITerritoriesContext territoriesContext,
            IGamePhaseConductor gamePhaseConductor,
            ITerritoryOccupier territoryOccupier,
            IRegion attackingRegion,
            IRegion occupiedRegion)
        {
            _territoriesContext = territoriesContext;
            _gamePhaseConductor = gamePhaseConductor;
            _territoryOccupier = territoryOccupier;
            _attackingRegion = attackingRegion;
            _occupiedRegion = occupiedRegion;
        }

        public void SendAdditionalArmiesToOccupy(int numberOfArmies)
        {
            var updatedTerritories = _territoryOccupier.SendInAdditionalArmiesToOccupy(_territoriesContext.Territories, _attackingRegion, _occupiedRegion, numberOfArmies);
            _territoriesContext.Set(updatedTerritories);

            _gamePhaseConductor.ContinueWithAttackPhase(TurnConqueringAchievement.SuccessfullyConqueredAtLeastOneTerritory);
        }
    }
}