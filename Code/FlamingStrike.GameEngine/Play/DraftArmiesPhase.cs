using System;
using System.Collections.Generic;
using System.Linq;

namespace FlamingStrike.GameEngine.Play
{
    public class DraftArmiesPhase : IDraftArmiesPhase
    {
        private readonly IGamePhaseConductor _gamePhaseConductor;
        private readonly IArmyDrafter _armyDrafter;

        public DraftArmiesPhase(
            IGamePhaseConductor gamePhaseConductor,
            PlayerName currentPlayerName,
            IReadOnlyList<ITerritory> territories,
            IReadOnlyList<IPlayer> players,
            IDeck deck,
            int numberOfArmiesToDraft,
            IArmyDrafter armyDrafter)
        {
            _gamePhaseConductor = gamePhaseConductor;
            _armyDrafter = armyDrafter;
            CurrentPlayerName = currentPlayerName;
            Territories = territories;
            Players = players;
            Deck = deck;
            NumberOfArmiesToDraft = numberOfArmiesToDraft;
        }

        public PlayerName CurrentPlayerName { get; }
        public IReadOnlyList<ITerritory> Territories { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public IDeck Deck { get; }
        public int NumberOfArmiesToDraft { get; }

        public IReadOnlyList<Region> RegionsAllowedToDraftArmies
        {
            get
            {
                var regionsAllowedToDraftArmies = Territories
                    .Where(x => CanPlaceDraftArmies(x.Region))
                    .Select(x => x.Region).ToList();

                return regionsAllowedToDraftArmies;
            }
        }

        public void PlaceDraftArmies(Region region, int numberOfArmies)
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
            var updatedGameData = new GameData(updatedTerritories, Players, CurrentPlayerName, Deck);

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

        private bool CanPlaceDraftArmies(Region region)
        {
            return IsCurrentPlayerOccupyingRegion(region);
        }

        private bool IsCurrentPlayerOccupyingRegion(Region region)
        {
            return CurrentPlayerName == Territories.Single(x => x.Region == region).Name;
        }
    }
}