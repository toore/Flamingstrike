using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Shuffling;

namespace RISK.GameEngine.Setup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, players still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public interface IAlternateGameSetupObserver
    {
        void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector);
        void NewGamePlaySetup(IGamePlaySetup gamePlaySetup);
    }

    public interface IAlternateGameSetup {}

    public interface IArmyPlacer
    {
        void PlaceArmyInRegion(IRegion selectedRegion);
    }

    public class AlternateGameSetup : IAlternateGameSetup, IArmyPlacer
    {
        private readonly IAlternateGameSetupObserver _alternateGameSetupObserver;
        private readonly IRegions _regions;
        private readonly IShuffle _shuffle;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private CircularBuffer<PlayerSetupData> _playerDoingGameSetups;
        private IReadOnlyList<Territory> _territories;
        private PlayerSetupData _currentPlayerSetupData;

        public AlternateGameSetup(
            IAlternateGameSetupObserver alternateGameSetupObserver,
            IRegions regions,
            IEnumerable<IPlayer> players,
            IStartingInfantryCalculator startingInfantryCalculator,
            IShuffle shuffle)
        {
            _alternateGameSetupObserver = alternateGameSetupObserver;
            _regions = regions;
            _shuffle = shuffle;
            _startingInfantryCalculator = startingInfantryCalculator;

            Initialize(players);
        }

        private void Initialize(IEnumerable<IPlayer> players)
        {
            _playerDoingGameSetups = Shuffle(players);

            InitializeArmiesToPlaceInSetup();

            var territories = AssignPlayersToTerritories();

            PlaceArmies(territories);
        }

        private CircularBuffer<PlayerSetupData> Shuffle(IEnumerable<IPlayer> players)
        {
            return players
                .Shuffle(_shuffle)
                .Select(player => new PlayerSetupData(player))
                .ToCircularBuffer();
        }

        private void InitializeArmiesToPlaceInSetup()
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(_playerDoingGameSetups.Count());

            foreach (var playerDoingGameSetup in _playerDoingGameSetups)
            {
                playerDoingGameSetup.SetArmiesToPlace(numberOfStartingInfantry);
            }
        }

        private List<Territory> AssignPlayersToTerritories()
        {
            var territories = new List<Territory>();

            var regions = _regions.GetAll()
                .Shuffle(_shuffle)
                .ToList();

            foreach (var region in regions)
            {
                var setupPlayer = _playerDoingGameSetups.Next();
                setupPlayer.SetArmiesToPlace(setupPlayer.ArmiesToPlace - 1);
                var territory = new Territory(region, setupPlayer.Player, 1);
                territories.Add(territory);
            }

            return territories;
        }

        private void PlaceArmies(IReadOnlyList<Territory> territories)
        {
            _territories = territories;

            ContinueToPlaceArmy();
        }

        private void ContinueToPlaceArmy()
        {
            _currentPlayerSetupData = _playerDoingGameSetups.Next();

            if (_currentPlayerSetupData.HasArmiesLeftToPlace())
            {
                SelectRegionToPlaceArmy(_currentPlayerSetupData, _territories);
            }
            else
            {
                SetupHasEnded();
            }
        }

        private void SetupHasEnded()
        {
            var players = _playerDoingGameSetups.Select(x => x.Player).ToList();

            var gamePlaySetup = new GamePlaySetup(players, _territories);

            _alternateGameSetupObserver.NewGamePlaySetup(gamePlaySetup);
        }

        public void PlaceArmyInRegion(IRegion selectedRegion)
        {
            var selectedTerritory = _territories
                .Where(territory => territory.Player == _currentPlayerSetupData.Player)
                .Single(x => x.Region == selectedRegion);

            _currentPlayerSetupData.PlaceArmy(selectedTerritory);

            ContinueToPlaceArmy();
        }

        private void SelectRegionToPlaceArmy(PlayerSetupData playerSetupData, IReadOnlyList<Territory> territories)
        {
            var selectableRegions = territories
                .Where(territory => territory.Player == playerSetupData.Player)
                .Select(x => x.Region)
                .ToList();

            var regionSelector = new PlaceArmyRegionSelector(this, territories, selectableRegions, playerSetupData);

            _alternateGameSetupObserver.SelectRegion(regionSelector);
        }
    }
}