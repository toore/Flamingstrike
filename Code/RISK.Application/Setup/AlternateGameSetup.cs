using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.Play;
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
        IGamePlaySetup Initialize(ITerritoryRequestHandler territoryRequestHandler);
    }

    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IEnumerable<IPlayerId> _players;
        private readonly IWorldMap _worldMap;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetup(IWorldMap worldMap, IEnumerable<IPlayerId> players, IStartingInfantryCalculator startingInfantryCalculator, IShuffler shuffler)
        {
            _players = players;
            _worldMap = worldMap;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public IGamePlaySetup Initialize(ITerritoryRequestHandler territoryRequestHandler)
        {
            var playersInTurnOrder = ShufflePlayers();
            var gameboardTerritories = AssignPlayersToTerritories(playersInTurnOrder);
            var gameSetupPlayers = InitializeInfantryToPlace(playersInTurnOrder, gameboardTerritories);
            PlaceArmies(territoryRequestHandler, gameSetupPlayers, gameboardTerritories);

            var gameSetup = new GamePlaySetup(playersInTurnOrder, gameboardTerritories);
            return gameSetup;
        }

        private IList<Player> InitializeInfantryToPlace(IReadOnlyCollection<IPlayerId> players, IEnumerable<Territory> gameboardTerritories)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            var gameSetupPlayers = players
                .Select(player => CreatePlayer(player, numberOfStartingInfantry, gameboardTerritories))
                .ToList();
            return gameSetupPlayers;
        }

        private static Player CreatePlayer(IPlayerId playerId, int numberOfStartingInfantry, IEnumerable<Territory> gameboardTerritories)
        {
            var numberOfTerritoriesAssignedToPlayer = gameboardTerritories.Count(t => t.PlayerId == playerId);
            var armiesToPlace = numberOfStartingInfantry - numberOfTerritoriesAssignedToPlayer;
            var gameSetupPlayer = new Player(playerId, armiesToPlace);
            return gameSetupPlayer;
        }

        private List<IPlayerId> ShufflePlayers()
        {
            var shuffledPlayers = _players
                .Shuffle(_shuffler)
                .ToList();
            return shuffledPlayers;
        }

        private List<Territory> AssignPlayersToTerritories(IList<IPlayerId> players)
        {
            var gameboardTerritories = new List<Territory>();

            var territories = _worldMap.GetAll()
                .Shuffle(_shuffler)
                .ToList();

            var player = players.First();

            foreach (var territory in territories)
            {
                var gameboardTerritory = new Territory(territory, player, 1);
                gameboardTerritories.Add(gameboardTerritory);

                player = players.GetNextOrFirst(player);
            }

            return gameboardTerritories;
        }

        private static void PlaceArmies(ITerritoryRequestHandler territoryRequestHandler, IList<Player> gameSetupPlayers, IList<Territory> territories)
        {
            while (AnyArmiesLeftToPlace(gameSetupPlayers))
            {
                PlaceArmiesForOneRound(territoryRequestHandler, gameSetupPlayers, territories);
            }
        }

        private static bool AnyArmiesLeftToPlace(IEnumerable<Player> players)
        {
            return players.Any(x => x.HasArmiesLeftToPlace());
        }

        private static void PlaceArmiesForOneRound(ITerritoryRequestHandler territoryRequestHandler, IList<Player> gameSetupPlayers, IList<Territory> territories)
        {
            var playersWithArmiesLeftToPlace = gameSetupPlayers
                .Where(x => x.HasArmiesLeftToPlace())
                .ToList();

            foreach (var gameSetupPlayer in playersWithArmiesLeftToPlace)
            {
                PlaceArmy(territoryRequestHandler, gameSetupPlayer, territories);
            }
        }

        private static void PlaceArmy(ITerritoryRequestHandler territoryRequestHandler, Player player, IList<Territory> gameboardSetupTerritories)
        {
            var gameboardTerritories = CreateGameboard(gameboardSetupTerritories);

            var territoriesAssignedToPlayer = gameboardSetupTerritories
                .Where(x => x.PlayerId == player.PlayerId)
                .ToList();

            var respondedGameboardTerritory = GetTerritoryResponse(territoryRequestHandler, gameboardTerritories, player, territoriesAssignedToPlayer);

            respondedGameboardTerritory.Armies++;
            player.ArmiesToPlace--;
        }

        private static List<Play.Territory> CreateGameboard(IEnumerable<Territory> gameboardSetupTerritories)
        {
            var gameboardTerritories = gameboardSetupTerritories
                .Select(x => new Play.Territory(x.TerritoryId, x.PlayerId, x.Armies))
                .ToList();

            return gameboardTerritories;
        }

        private static Territory GetTerritoryResponse(ITerritoryRequestHandler territoryRequestHandler, List<Play.Territory> gameboardTerritories, Player player, List<Territory> territoriesAssignedToPlayer)
        {
            var selectableTerritories = territoriesAssignedToPlayer
                .Select(x => x.TerritoryId)
                .ToList();

            var parameter = new TerritoryRequestParameter(gameboardTerritories, selectableTerritories, player);

            var selectedTerritory = territoryRequestHandler.ProcessRequest(parameter);
            var selectedGameboardSetupTerritory = territoriesAssignedToPlayer.Single(x => x.TerritoryId == selectedTerritory);

            return selectedGameboardSetupTerritory;
        }
    }
}