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
        private readonly IEnumerable<IPlayer> _players;
        private readonly IRegions _regions;
        private readonly IShuffle _shuffle;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private CircularBuffer<IPlayer> _circularBufferOfPlayers;
        private IReadOnlyList<Territory> _territories;
        private IPlayer _currentPlayer;

        public AlternateGameSetup(
            IAlternateGameSetupObserver alternateGameSetupObserver,
            IRegions regions,
            IEnumerable<IPlayer> players,
            IStartingInfantryCalculator startingInfantryCalculator,
            IShuffle shuffle)
        {
            _alternateGameSetupObserver = alternateGameSetupObserver;
            _players = players;
            _regions = regions;
            _shuffle = shuffle;
            _startingInfantryCalculator = startingInfantryCalculator;

            Initialize();
        }

        private void Initialize()
        {
            var playersInOrderOfTakingTurn = ShufflePlayers();
            InitializeArmiesToPlace(playersInOrderOfTakingTurn);

            var sequenceOfPlayers = playersInOrderOfTakingTurn.ToCircularBuffer();

            var territories = AssignPlayersToTerritories(sequenceOfPlayers);

            PlaceArmies(sequenceOfPlayers, territories);
        }

        private IList<IPlayer> ShufflePlayers()
        {
            return _players
                .Shuffle(_shuffle);
        }

        private void InitializeArmiesToPlace(ICollection<IPlayer> players)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            foreach (var player in players)
            {
                player.SetArmiesToPlace(numberOfStartingInfantry);
            }
        }

        private List<Territory> AssignPlayersToTerritories(CircularBuffer<IPlayer> players)
        {
            var territories = new List<Territory>();

            var regions = _regions.GetAll()
                .Shuffle(_shuffle)
                .ToList();

            foreach (var region in regions)
            {
                var player = players.Next();
                player.SetArmiesToPlace(player.ArmiesToPlace - 1);
                var territory = new Territory(region, player, 1);
                territories.Add(territory);
            }

            return territories;
        }

        private void PlaceArmies(CircularBuffer<IPlayer> players, IReadOnlyList<Territory> territories)
        {
            _circularBufferOfPlayers = players;
            _territories = territories;

            ContinueToPlaceArmy();
        }

        private void ContinueToPlaceArmy()
        {
            _currentPlayer = _circularBufferOfPlayers.Next();

            if (_currentPlayer.HasArmiesLeftToPlace())
            {
                SelectRegionToPlaceArmy(_currentPlayer, _territories);
            }
            else
            {
                SetupHasEnded();
            }
        }

        private void SetupHasEnded()
        {
            var gamePlaySetup = new GamePlaySetup(_circularBufferOfPlayers.ToList(), _territories);

            _alternateGameSetupObserver.NewGamePlaySetup(gamePlaySetup);
        }

        public void PlaceArmyInRegion(IRegion selectedRegion)
        {
            var selectedTerritory = _territories
                .Where(territory => territory.Player == _currentPlayer)
                .Single(x => x.Region == selectedRegion);

            _currentPlayer.PlaceArmy(selectedTerritory);

            ContinueToPlaceArmy();
        }

        private void SelectRegionToPlaceArmy(IPlayer player, IReadOnlyList<Territory> territories)
        {
            var selectableRegions = territories
                .Where(territory => territory.Player == player)
                .Select(x => x.Region)
                .ToList();

            var regionSelector = new PlaceArmyRegionSelector(this, territories, selectableRegions, player);

            _alternateGameSetupObserver.SelectRegion(regionSelector);
        }
    }
}