using System.Collections.Generic;
using System.Linq;
using RISK.Application.World;

namespace RISK.Application.Play.GamePhases
{
    public class DraftArmiesGameState : GameStateBase
    {
        private readonly GameStateFactory _gameStateFactory;
        private readonly int _numberOfArmiesToDraft;

        public DraftArmiesGameState(GameStateFactory gameStateFactory, GameData gameData, int numberOfArmiesToDraft)
            : base(gameData)
        {
            _gameStateFactory = gameStateFactory;
            _numberOfArmiesToDraft = numberOfArmiesToDraft;
        }

        public override int GetNumberOfArmiesToDraft()
        {
            return _numberOfArmiesToDraft;
        }

        public override IGameState PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            var updatedTerritories = PlaceArmies(Territories, region, numberOfArmiesToPlace);

            var gameData = new GameData(CurrentPlayer, Players, updatedTerritories);
            var numberOfArmiesToPlaceLeft = _numberOfArmiesToDraft - numberOfArmiesToPlace;

            return _gameStateFactory.CreateDraftArmiesGameState(gameData, numberOfArmiesToPlaceLeft);
        }

        private static IReadOnlyList<ITerritory> PlaceArmies(IReadOnlyList<ITerritory> territories, IRegion region, int numberOfArmiesToPlace)
        {
            var territoryToUpdate = territories.Single(x => x.Region == region);
            var currentNumberOfArmies = territoryToUpdate.Armies;
            var updatedTerritory = new Territory(region, territoryToUpdate.Player, currentNumberOfArmies + numberOfArmiesToPlace);

            var updatedTerritories = territories
                .Except(new[] { territoryToUpdate })
                .Union(new[] { updatedTerritory })
                .ToList();

            return updatedTerritories;
        }
    }
}