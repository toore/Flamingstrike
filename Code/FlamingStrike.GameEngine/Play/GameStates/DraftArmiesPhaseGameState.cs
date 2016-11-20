using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play.GameStates
{
    public interface IDraftArmiesPhaseGameState
    {
        IPlayer Player { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        int NumberOfArmiesToDraft { get; }
        IReadOnlyList<IPlayerGameData> Players { get; }
        bool CanPlaceDraftArmies(IRegion region);
        void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace);
    }

    public class DraftArmiesPhaseGameState : IDraftArmiesPhaseGameState
    {
        private readonly GameData _gameData;
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IArmyDrafter _armyDrafter;

        public DraftArmiesPhaseGameState(
            GameData gameData,
            IGamePhaseConductor gamePhaseConductor,
            IArmyDrafter armyDrafter,
            int numberOfArmiesToDraft)
        {
            NumberOfArmiesToDraft = numberOfArmiesToDraft;
            _gameData = gameData;
            _gamePhaseConductor = gamePhaseConductor;
            _armyDrafter = armyDrafter;
        }

        public IPlayer Player => _gameData.CurrentPlayer;
        public IReadOnlyList<ITerritory> Territories => _gameData.Territories;
        public int NumberOfArmiesToDraft { get; }
        public IReadOnlyList<IPlayerGameData> Players => _gameData.PlayerGameDatas;

        public bool CanPlaceDraftArmies(IRegion region)
        {
            return IsCurrentPlayerOccupyingRegion(region);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return _gameData.CurrentPlayer == _gameData.Territories.Single(x => x.Region == region).Player;
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmiesToPlace)
        {
            if (!IsCurrentPlayerOccupyingRegion(region))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(region)}.");
            }
            if (numberOfArmiesToPlace > NumberOfArmiesToDraft)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArmiesToPlace));
            }

            var updatedTerritories = _armyDrafter.PlaceDraftArmies(_gameData.Territories, region, numberOfArmiesToPlace);
            var updatedGameData = new GameData(updatedTerritories, _gameData.PlayerGameDatas, _gameData.CurrentPlayer, _gameData.Deck);

            var numberOfArmiesLeftToPlace = NumberOfArmiesToDraft - numberOfArmiesToPlace;
            if (numberOfArmiesLeftToPlace > 0)
            {
                _gamePhaseConductor.ContinueToDraftArmies(numberOfArmiesLeftToPlace, updatedGameData);
            }
            else
            {
                _gamePhaseConductor.ContinueWithAttackPhase(TurnConqueringAchievement.NoTerritoryHasBeenConquered, updatedGameData);
            }
        }
    }
}