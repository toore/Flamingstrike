using System;
using RISK.Application.Play.Planning;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : GameStateBase
    {
        private readonly IGameStateConductor _gameStateConductor;
        private readonly IGameDataFactory _gameDataFactory;
        private readonly int _numberOfArmiesToDraft;
        private readonly IArmyModifier _armyModifier;

        public DraftArmiesGameState(IGameStateConductor gameStateConductor, IGameDataFactory gameDataFactory, IArmyModifier armyModifier, GameData gameData, int numberOfArmiesToDraft)
            : base(gameData)
        {
            _gameStateConductor = gameStateConductor;
            _gameDataFactory = gameDataFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
            _armyModifier = armyModifier;
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

            var updatedTerritories = _armyModifier.PlaceDraftArmies(Territories, region, numberOfArmiesToPlace);

            var gameData = _gameDataFactory.Create(CurrentPlayer, Players, updatedTerritories, Deck);
            var numberOfArmiesLeftToPlace = _numberOfArmiesToDraft - numberOfArmiesToPlace;

            if (numberOfArmiesLeftToPlace > 0)
            {
                return _gameStateConductor.ContinueToDraftArmies(gameData, numberOfArmiesLeftToPlace);
            }

            return _gameStateConductor.ContinueWithAttackPhase(gameData, ConqueringAchievement.DoNotAwardCardAtEndOfTurn);
        }
    }
}