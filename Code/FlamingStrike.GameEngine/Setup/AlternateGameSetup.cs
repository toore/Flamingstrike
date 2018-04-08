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
        private readonly IRegions _regions;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private readonly IShuffler _shuffler;

        public AlternateGameSetup(
            IAlternateGameSetupObserver alternateGameSetupObserver,
            IRegions regions,
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
            var players = Shuffle(playerNames);
            var territoriesAndPlayers = AssignPlayersToTerritories(players);

            PlaceArmies(territoriesAndPlayers);
        }

        private List<Player> Shuffle(IReadOnlyCollection<PlayerName> players)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            return players
                .Shuffle(_shuffler)
                .Select(player => new Player(player, numberOfStartingInfantry))
                .ToList();
        }

        private AlternateGameSetupData AssignPlayersToTerritories(IReadOnlyList<Player> players)
        {
            var territories = _regions.GetAll()
                .Shuffle(_shuffler)
                .Select((region, i) => new Territory(region, players[i % players.Count]))
                .ToList();

            foreach (var territory in territories)
            {
                territory.PlaceArmy();
            }

            return new AlternateGameSetupData(territories, players, players[territories.Count % players.Count]);
        }

        private void PlaceArmies(AlternateGameSetupData alternateGameSetupData)
        {
            ContinueToPlaceArmy(alternateGameSetupData);
        }

        private void ContinueToPlaceArmy(AlternateGameSetupData alternateGameSetupData)
        {
            var player = alternateGameSetupData.Players.Single(x => x.Name == alternateGameSetupData.CurrentPlayer.Name);
            if (player.HasArmiesLeftToPlace())
            {
                SelectRegionToPlaceArmy(player, alternateGameSetupData);
            }
            else
            {
                SetupHasEnded(alternateGameSetupData);
            }
        }

        private void SetupHasEnded(AlternateGameSetupData alternateGameSetupData)
        {
            var gamePlaySetup = new GamePlaySetup(
                alternateGameSetupData.Players.Select(x => x.Name).ToList(),
                alternateGameSetupData.Territories.Select(x => new Finished.Territory(x.Region, x.Player.Name, x.Armies)).ToList());

            _alternateGameSetupObserver.NewGamePlaySetup(gamePlaySetup);
        }

        public void PlaceArmyInRegion(Player currentPlayer, IRegion selectedRegion, AlternateGameSetupData alternateGameSetupData)
        {
            alternateGameSetupData
                .Territories
                .Single(x => x.Region == selectedRegion)
                .PlaceArmy();

            var nextPlayerIndex = alternateGameSetupData.Players.ToList().IndexOf(currentPlayer) + 1;
            var nextPlayer = alternateGameSetupData.Players[nextPlayerIndex % alternateGameSetupData.Players.Count];
            var gameSetupData = new AlternateGameSetupData(alternateGameSetupData.Territories, alternateGameSetupData.Players, nextPlayer);
            ContinueToPlaceArmy(gameSetupData);
        }

        private void SelectRegionToPlaceArmy(Player player, AlternateGameSetupData alternateGameSetupData)
        {
            var territorySelector = new TerritorySelector(this, alternateGameSetupData, player);

            _alternateGameSetupObserver.SelectRegion(territorySelector);
        }
    }

    public class AlternateGameSetupData
    {
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<Player> Players { get; }
        public Player CurrentPlayer { get; }

        public AlternateGameSetupData(IReadOnlyList<Territory> territories, IReadOnlyList<Player> players, Player currentPlayer)
        {
            Territories = territories;
            Players = players;
            CurrentPlayer = currentPlayer;
        }
    }
}