using System;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : GameStateBase
    {
        private readonly IGameStateFactory _gameStateFactory;
        private readonly int _numberOfArmiesToDraft;
        private readonly IArmyDraftUpdater _armyDraftUpdater;

        public DraftArmiesGameState(IGameStateFactory gameStateFactory, IArmyDraftUpdater armyDraftUpdater, GameData gameData, int numberOfArmiesToDraft)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
            _armyDraftUpdater = armyDraftUpdater;
        }

        public override bool CanPlaceDraftArmies(IRegion region)
        {
            return GetTerritory(region).Player == CurrentPlayer;
        }

        public override int GetNumberOfArmiesToDraft()
        {
            return _numberOfArmiesToDraft;
        }

        public override IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            if (numberOfArmiesToPlace > _numberOfArmiesToDraft)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArmiesToPlace));
            }

            var updatedTerritories = _armyDraftUpdater.PlaceArmies(Territories, region, numberOfArmiesToPlace);

            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories);
            var numberOfArmiesLeftToPlace = _numberOfArmiesToDraft - numberOfArmiesToPlace;

            if (numberOfArmiesLeftToPlace > 0)
            {
                return _gameStateFactory.CreateDraftArmiesGameState(gameData, numberOfArmiesLeftToPlace);
            }
            return _gameStateFactory.CreateAttackGameState(gameData);
        }
    }
}