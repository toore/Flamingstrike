using System.Collections.Generic;
using System.Linq;
using RISK.Core;
using RISK.GameEngine.Extensions;
using Toore.Shuffling;

namespace RISK.GameEngine.Setup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, players still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public interface IAlternateGameSetup
    {
        ITerritoryResponder TerritoryResponder { get; set; }
        IGamePlaySetup Initialize();
    }

    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IEnumerable<IPlayer> _players;
        private readonly IRegions _regions;
        private readonly IShuffle _shuffle;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetup(
            IRegions regions,
            IEnumerable<IPlayer> players,
            IStartingInfantryCalculator startingInfantryCalculator,
            IShuffle shuffle)
        {
            _players = players;
            _regions = regions;
            _shuffle = shuffle;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public ITerritoryResponder TerritoryResponder { get; set; }

        public IGamePlaySetup Initialize()
        {
            var playersInOrderOfTakingTurn = ShufflePlayers();
            var territories = AssignPlayersToTerritories(playersInOrderOfTakingTurn.ToSequence());

            InitializeInfantryToPlace(playersInOrderOfTakingTurn, territories);
            PlaceArmies(TerritoryResponder, playersInOrderOfTakingTurn, territories);

            var gamePlaySetup = new GamePlaySetup(playersInOrderOfTakingTurn, territories);

            return gamePlaySetup;
        }

        private List<IPlayer> ShufflePlayers()
        {
            return _players
                .Shuffle(_shuffle)
                .ToList();
        }

        private List<Territory> AssignPlayersToTerritories(Sequence<IPlayer> players)
        {
            var territories = new List<Territory>();

            var regions = _regions.GetAll()
                .Shuffle(_shuffle)
                .ToList();

            foreach (var region in regions)
            {
                var player = players.Next();
                var territory = new Territory(region, player, 1);
                territories.Add(territory);
            }

            return territories;
        }

        private void InitializeInfantryToPlace(IList<IPlayer> players, IList<Territory> territories)
        {
            //var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.ToList().Count);
            var numberOfStartingInfantry = 21;

            foreach (var player in players)
            {
                var armiesToPlace = GetNumberOfArmiesToPlace(territories, player, numberOfStartingInfantry);
                player.SetArmiesToPlace(armiesToPlace);
            }
        }

        private static int GetNumberOfArmiesToPlace(IEnumerable<Territory> territories, IPlayer player, int numberOfStartingInfantry)
        {
            var numberOfTerritoriesAssignedToPlayer = territories.Count(territory => territory.Player == player);
            var armiesToPlace = numberOfStartingInfantry - numberOfTerritoriesAssignedToPlayer;
            return armiesToPlace;
        }

        private static void PlaceArmies(ITerritoryResponder territoryResponder, IList<IPlayer> gameSetupPlayers, IReadOnlyList<Territory> territories)
        {
            while (AnyArmiesLeftToPlace(gameSetupPlayers))
            {
                PlaceArmiesForOneRound(territoryResponder, gameSetupPlayers, territories);
            }
        }

        private static bool AnyArmiesLeftToPlace(IEnumerable<IPlayer> players)
        {
            return players.Any(x => x.HasArmiesLeftToPlace());
        }

        private static void PlaceArmiesForOneRound(ITerritoryResponder territoryResponder, IEnumerable<IPlayer> gameSetupPlayers, IReadOnlyList<Territory> territories)
        {
            var playersWithArmiesLeftToPlace = gameSetupPlayers
                .Where(x => x.HasArmiesLeftToPlace())
                .ToList();

            foreach (var gameSetupPlayer in playersWithArmiesLeftToPlace)
            {
                PlaceArmy(territoryResponder, gameSetupPlayer, territories);
            }
        }

        private static void PlaceArmy(ITerritoryResponder territoryResponder, IPlayer player, IReadOnlyList<Territory> territories)
        {
            var territoriesAssignedToPlayer = territories
                .Where(territory => territory.Player == player)
                .ToList();

            var selectedTerritory = SelectTerritory(territoryResponder, territories, player, territoriesAssignedToPlayer);

            player.PlaceArmy(selectedTerritory);
        }

        private static Territory SelectTerritory(ITerritoryResponder territoryResponder, IReadOnlyList<Territory> territories, IPlayer IPlayer, List<Territory> territoriesAssignedToPlayer)
        {
            var options = territoriesAssignedToPlayer
                .Select(x => x.Region)
                .ToList();

            var parameter = new TerritoryRequestParameter(territories, options, IPlayer);
            var selectedTerritoryId = territoryResponder.ProcessRequest(parameter);

            var selectedTerritory = territoriesAssignedToPlayer.Single(x => x.Region == selectedTerritoryId);
            return selectedTerritory;
        }
    }
}