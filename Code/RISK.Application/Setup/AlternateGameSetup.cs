using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Toore.Shuffling;

namespace RISK.Application.Setup
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
        private readonly IWorldMap _worldMap;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetup(
            IWorldMap worldMap,
            IEnumerable<IPlayer> players,
            IStartingInfantryCalculator startingInfantryCalculator,
            IShuffler shuffler)
        {
            _players = players;
            _worldMap = worldMap;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public ITerritoryResponder TerritoryResponder { get; set; }

        public IGamePlaySetup Initialize()
        {
            var playersInTakingTurnOrder = ShufflePlayers();
            var territories = AssignPlayersToTerritories(playersInTakingTurnOrder);
            var gameSetupPlayers = InitializeInfantryToPlace(playersInTakingTurnOrder, territories);

            PlaceArmies(TerritoryResponder, gameSetupPlayers, territories);

            var gamePlaySetup = new GamePlaySetup(playersInTakingTurnOrder, territories);
            return gamePlaySetup;
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

        private List<IPlayer> ShufflePlayers()
        {
            var shuffledPlayers = _players
                .Shuffle(_shuffler)
                .ToList();
            return shuffledPlayers;
        }

        private List<Territory> AssignPlayersToTerritories(IList<IPlayer> players)
        {
            var territories = new List<Territory>();

            var territoryIds = _worldMap.GetAll()
                .Shuffle(_shuffler)
                .ToList();

            var player = players.First();

            foreach (var territoryId in territoryIds)
            {
                var territory = new Territory(territoryId, player, 1);
                territories.Add(territory);

                player = players.GetNextOrFirst(player);
            }

            return territories;
        }

        private static void PlaceArmies(ITerritoryResponder territoryResponder, IList<InSetupPlayer> gameSetupPlayers, IList<Territory> territories)
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

        private static void PlaceArmiesForOneRound(ITerritoryResponder territoryResponder, IEnumerable<InSetupPlayer> gameSetupPlayers, IList<Territory> territories)
        {
            var playersWithArmiesLeftToPlace = gameSetupPlayers
                .Where(x => x.HasArmiesLeftToPlace())
                .ToList();

            foreach (var gameSetupPlayer in playersWithArmiesLeftToPlace)
            {
                PlaceArmy(territoryResponder, gameSetupPlayer, territories);
            }
        }

        private static void PlaceArmy(ITerritoryResponder territoryResponder, InSetupPlayer inSetupPlayer, IList<Territory> territories)
        {
            var territoriesDuringGamePlay = CreateGameboard(territories);

            var territoriesAssignedToPlayer = territories
                .Where(x => x.Player == inSetupPlayer.Player)
                .ToList();

            var selectedTerritory = SelectTerritory(territoryResponder, territoriesDuringGamePlay, inSetupPlayer, territoriesAssignedToPlayer);

            selectedTerritory.Armies++;
            inSetupPlayer.ArmiesToPlace--;
        }

        private static List<Play.Territory> CreateGameboard(IEnumerable<Territory> territories)
        {
            var gameboardTerritories = territories
                .Select(x => new Play.Territory(x.TerritoryGeography, x.Player, x.Armies))
                .ToList();

            return gameboardTerritories;
        }

        private static Territory SelectTerritory(ITerritoryResponder territoryResponder, IReadOnlyList<Play.Territory> territories, InSetupPlayer inSetupPlayer, List<Territory> territoriesAssignedToPlayer)
        {
            var options = territoriesAssignedToPlayer
                .Select(x => x.TerritoryGeography)
                .ToList();

            var parameter = new TerritoryRequestParameter(territories, options, inSetupPlayer);
            var selectedTerritoryId = territoryResponder.ProcessRequest(parameter);

            var selectedTerritory = territoriesAssignedToPlayer.Single(x => x.TerritoryGeography == selectedTerritoryId);
            return selectedTerritory;
        }
    }
}