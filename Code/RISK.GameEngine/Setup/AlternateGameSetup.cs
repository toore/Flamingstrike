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
            var gameSetupPlayers = InitializeInfantryToPlace(playersInOrderOfTakingTurn, territories);

            PlaceArmies(TerritoryResponder, gameSetupPlayers, territories);

            var gamePlaySetup = new GamePlaySetup(playersInOrderOfTakingTurn.ToSequence(), territories);
            return gamePlaySetup;
        }

        private List<IPlayer> ShufflePlayers()
        {
            var shuffledPlayers = _players
                .Shuffle(_shuffle)
                .ToList();

            return shuffledPlayers;
        }

        private List<Territory> AssignPlayersToTerritories(Sequence<IPlayer> players)
        {
            var territories = new List<Territory>();

            var territoryIds = _regions.GetAll()
                .Shuffle(_shuffle)
                .ToList();

            foreach (var territoryId in territoryIds)
            {
                var player = players.Next();
                var territory = new Territory(territoryId, player, 1);
                territories.Add(territory);
            }

            return territories;
        }

        private IList<InSetupPlayer> InitializeInfantryToPlace(IReadOnlyCollection<IPlayer> playerIds, IEnumerable<Territory> territories)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(playerIds.Count);

            var players = playerIds
                .Select(player => CreatePlayer(player, numberOfStartingInfantry, territories))
                .ToList();
            return players;
        }

        private static InSetupPlayer CreatePlayer(IPlayer player, int numberOfStartingInfantry, IEnumerable<Territory> territories)
        {
            var numberOfTerritoriesAssignedToPlayer = territories.Count(t => t.Player == player);
            var armiesToPlace = numberOfStartingInfantry - numberOfTerritoriesAssignedToPlayer;
            var inSetupPlayer = new InSetupPlayer(player, armiesToPlace);
            return inSetupPlayer;
        }

        private static void PlaceArmies(ITerritoryResponder territoryResponder, IList<InSetupPlayer> gameSetupPlayers, IReadOnlyList<Territory> territories)
        {
            while (AnyArmiesLeftToPlace(gameSetupPlayers))
            {
                PlaceArmiesForOneRound(territoryResponder, gameSetupPlayers, territories);
            }
        }

        private static bool AnyArmiesLeftToPlace(IEnumerable<InSetupPlayer> players)
        {
            return players.Any(x => x.HasArmiesLeftToPlace());
        }

        private static void PlaceArmiesForOneRound(ITerritoryResponder territoryResponder, IEnumerable<InSetupPlayer> gameSetupPlayers, IReadOnlyList<Territory> territories)
        {
            var playersWithArmiesLeftToPlace = gameSetupPlayers
                .Where(x => x.HasArmiesLeftToPlace())
                .ToList();

            foreach (var gameSetupPlayer in playersWithArmiesLeftToPlace)
            {
                PlaceArmy(territoryResponder, gameSetupPlayer, territories);
            }
        }

        private static void PlaceArmy(ITerritoryResponder territoryResponder, InSetupPlayer inSetupPlayer, IReadOnlyList<Territory> territories)
        {
            var territoriesAssignedToPlayer = territories
                .Where(x => x.Player == inSetupPlayer.Player)
                .ToList();

            var selectedTerritory = SelectTerritory(territoryResponder, territories, inSetupPlayer, territoriesAssignedToPlayer);

            selectedTerritory.Armies++;
            inSetupPlayer.ArmiesToPlace--;
        }

        private static Territory SelectTerritory(ITerritoryResponder territoryResponder, IReadOnlyList<Territory> territories, InSetupPlayer inSetupPlayer, List<Territory> territoriesAssignedToPlayer)
        {
            var options = territoriesAssignedToPlayer
                .Select(x => x.Region)
                .ToList();

            var parameter = new TerritoryRequestParameter(territories, options, inSetupPlayer);
            var selectedTerritoryId = territoryResponder.ProcessRequest(parameter);

            var selectedTerritory = territoriesAssignedToPlayer.Single(x => x.Region == selectedTerritoryId);
            return selectedTerritory;
        }
    }
}