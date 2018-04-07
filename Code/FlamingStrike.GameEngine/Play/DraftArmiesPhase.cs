using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public interface IDraftArmiesPhase
    {
        PlayerName CurrentPlayerName { get; }
        IReadOnlyList<ITerritory> Territories { get; }
        IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        int NumberOfArmiesToDraft { get; }
        IReadOnlyList<IRegion> RegionsAllowedToDraftArmies { get; }
        void PlaceDraftArmies(IRegion region, int numberOfArmies);
    }

    public class DraftArmiesPhase : IDraftArmiesPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IArmyDrafter _armyDrafter;

        public DraftArmiesPhase(
            IGamePhaseConductor gamePhaseConductor,
            PlayerName currentPlayerName,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayerGameData> playerGameDatas,
            IDeck deck,
            int numberOfArmiesToDraft,
            IArmyDrafter armyDrafter)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _armyDrafter = armyDrafter;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            PlayerGameDatas = playerGameDatas;
            Deck = deck;
            NumberOfArmiesToDraft = numberOfArmiesToDraft;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayerGameData> PlayerGameDatas { get; }
        public IDeck Deck { get; }
        public int NumberOfArmiesToDraft { get; }

        public IReadOnlyList<IRegion> RegionsAllowedToDraftArmies
        {
            get
            {
                var regionsAllowedToDraftArmies = Territories
                    .Where(x => CanPlaceDraftArmies(x.Region))
                    .Select(x => x.Region).ToList();

                return regionsAllowedToDraftArmies;
            }
        }

        public void PlaceDraftArmies(IRegion region, int numberOfArmies)
        {
            if (!IsCurrentPlayerOccupyingRegion(region))
            {
                throw new InvalidOperationException($"Current player is not occupying {nameof(region)}.");
            }

            if (numberOfArmies > NumberOfArmiesToDraft)
            {
                throw new ArgumentOutOfRangeException(nameof(numberOfArmies));
            }

            var updatedTerritories = _armyDrafter.PlaceDraftArmies(Territories, region, numberOfArmies);
            var updatedGameData = new GameData(updatedTerritories, PlayerGameDatas, CurrentPlayerName, Deck);

            var numberOfArmiesLeftToPlace = NumberOfArmiesToDraft - numberOfArmies;
            if (numberOfArmiesLeftToPlace > 0)
            {
                _gamePhaseConductor.ContinueToDraftArmies(numberOfArmiesLeftToPlace, updatedGameData);
            }
            else
            {
                _gamePhaseConductor.ContinueWithAttackPhase(ConqueringAchievement.NoTerritoryHasBeenConquered, updatedGameData);
            }
        }

        private bool CanPlaceDraftArmies(IRegion region)
        {
            return IsCurrentPlayerOccupyingRegion(region);
        }

        private bool IsCurrentPlayerOccupyingRegion(IRegion region)
        {
            return CurrentPlayerName == Territories.Single(x => x.Region == region).PlayerName;
        }
    }
}