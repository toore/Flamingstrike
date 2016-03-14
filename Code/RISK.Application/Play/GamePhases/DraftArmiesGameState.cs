using System;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : GameStateBase
    {
        private readonly GameStateFactory _gameStateFactory;
        private readonly int _numberOfArmiesToDraft;
        private readonly ITerritoryUpdater _territoryUpdater;

        public DraftArmiesGameState(GameStateFactory gameStateFactory, GameData gameData, int numberOfArmiesToDraft, ITerritoryUpdater territoryUpdater)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
            _territoryUpdater = territoryUpdater;
        }

        public override int GetNumberOfArmiesToDraft()
        {
            return _numberOfArmiesToDraft;
        }

        public override bool CanPlaceDraftArmies(IRegion region)
        {
            return GetTerritory(region).Player == CurrentPlayer;
        }

        public override IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            if (numberOfArmiesToPlace > _numberOfArmiesToDraft)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArmiesToPlace));
            }

            var updatedTerritories = _territoryUpdater.PlaceArmies(Territories, region, numberOfArmiesToPlace);

            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories);
            var numberOfArmiesToPlaceLeft = _numberOfArmiesToDraft - numberOfArmiesToPlace;

            return _gameStateFactory.CreateDraftArmiesGameState(gameData, numberOfArmiesToPlaceLeft);
        }
    }
}