using System;
using System.Linq;

namespace RISK.GameEngine.Play.GameStates
{
    public interface IDraftArmiesPhaseGameState
    {
        bool CanPlaceDraftArmies(IRegion region);
        void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace);
    }

    public class DraftArmiesPhaseGameState : IDraftArmiesPhaseGameState
    {
        private readonly IPlayer _currentPlayer;
        private readonly ITerritoriesContext _territoriesContext;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IArmyDrafter _armyDrafter;
        private readonly int _numberOfArmiesToDraft;

        public DraftArmiesPhaseGameState(
            IPlayer currentPlayer,
            ITerritoriesContext territoriesContext,
            IGamePhaseConductor gamePhaseConductor,
            IArmyDrafter armyDrafter,
            int numberOfArmiesToDraft)
        {
            _currentPlayer = currentPlayer;
            _territoriesContext = territoriesContext;
            _gamePhaseConductor = gamePhaseConductor;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
            _armyDrafter = armyDrafter;
        }

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return IsCurrentPlayerOccupyingRegion(region);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _currentPlayer == _territoriesContext.Territories.Single(x => x.Region == region).Player;
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            if (!IsCurrentPlayerOccupyingRegion(region))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(region)}.");
            }
            if (numberOfArmiesToPlace > _numberOfArmiesToDraft)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArmiesToPlace));
            }

            PlaceDraftArmiesAndUpdateTerritories(region, numberOfArmiesToPlace);

            var numberOfArmiesLeftToPlace = _numberOfArmiesToDraft - numberOfArmiesToPlace;
            if (numberOfArmiesLeftToPlace > 0)
            {
                _gamePhaseConductor.ContinueToDraftArmies(numberOfArmiesLeftToPlace);
            }
            else
            {
                _gamePhaseConductor.ContinueWithAttackPhase(TurnConqueringAchievement.NoTerritoryHasBeenConquered);
            }
        }

        private void PlaceDraftArmiesAndUpdateTerritories(IRegion region, int numberOfArmiesToPlace)
        {
            var updatedTerritories = _armyDrafter.PlaceDraftArmies(_territoriesContext.Territories, region, numberOfArmiesToPlace);
            _territoriesContext.Set(updatedTerritories);
        }
    }
}