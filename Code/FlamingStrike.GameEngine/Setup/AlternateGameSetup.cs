using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using Toore.Shuffling;

namespace FlamingStrike.GameEngine.Setup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, players still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public class AlternateGameSetup : IArmyPlacer
    {
        private readonly IAlternateGameSetupObserver _alternateGameSetupObserver;
        private readonly IReadOnlyList<Region> _regions;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private readonly IShuffler _shuffler;
        private List<Player> _players;
        private List<Territory> _territories;
        private int _currentPlayerIndex;

        public AlternateGameSetup(
            IAlternateGameSetupObserver alternateGameSetupObserver,
            IReadOnlyList<Region> regions,
            IStartingInfantryCalculator startingInfantryCalculator,
            IShuffler shuffler)
        {
            _alternateGameSetupObserver = alternateGameSetupObserver;
            _regions = regions;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public void Run(IReadOnlyList<PlayerName> playerNames)
        {
            _players = Shuffle(playerNames);
            AssignPlayersToTerritories();

            PlaceArmies();
        }

        public void PlaceArmyInRegion(Region selectedRegion)
        {
            _territories.Single(x => x.Region == selectedRegion).PlaceArmy();

            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;

            ContinueToPlaceArmy();
        }

        private List<Player> Shuffle(IReadOnlyCollection<PlayerName> players)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            return players
                .Shuffle(_shuffler)
                .Select(player => new Player(player, numberOfStartingInfantry))
                .ToList();
        }

        private void AssignPlayersToTerritories()
        {
            _territories = _regions
                .Shuffle(_shuffler)
                .Select((region, i) => new Territory(region, _players[i % _players.Count]))
                .ToList();

            foreach (var territory in _territories)
            {
                territory.PlaceArmy();
            }
        }

        private void PlaceArmies()
        {
            _currentPlayerIndex = _territories.Count % _players.Count;
            ContinueToPlaceArmy();
        }

        private void ContinueToPlaceArmy()
        {
            if (GetCurrentPlayer().HasArmiesLeftToPlace())
            {
                SelectRegionToPlaceArmy();
            }
            else
            {
                SetupHasEnded();
            }
        }

        private Player GetCurrentPlayer()
        {
            return _players[_currentPlayerIndex];
        }

        private void SetupHasEnded()
        {
            var gamePlaySetup = new GamePlaySetup(
                _players.Select(x => x.Name).ToList(),
                _territories.Select(x => new Finished.Territory(x.Region, x.Player.Name, x.Armies)).ToList());

            _alternateGameSetupObserver.NewGamePlaySetup(gamePlaySetup);
        }

        private void SelectRegionToPlaceArmy()
        {
            var currentPlayer = GetCurrentPlayer();

            var territories = _territories
                .Select(x => new TerritorySelection.Territory(x.Region, x.Player.Name, x.Armies, x.Player == currentPlayer))
                .ToList();

            var territorySelector = new TerritorySelector(this, currentPlayer.Name, currentPlayer.ArmiesToPlace, territories);

            _alternateGameSetupObserver.SelectRegion(territorySelector);
        }
    }
}