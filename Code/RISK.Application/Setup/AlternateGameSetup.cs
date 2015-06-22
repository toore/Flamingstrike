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
        IGameSetup Initialize(ITerritoryRequestHandler territoryRequestHandler);
    }

    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IEnumerable<IPlayer> _players;
        private readonly IWorldMap _worldMap;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetup(IWorldMap worldMap, IEnumerable<IPlayer> players, IStartingInfantryCalculator startingInfantryCalculator, IShuffler shuffler)
        {
            _players = players;
            _worldMap = worldMap;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public IGameSetup Initialize(ITerritoryRequestHandler territoryRequestHandler)
        {
            var playersInTurnOrder = ShufflePlayers();
            var gameboardTerritories = AssignPlayersToTerritories(playersInTurnOrder);
            var gameSetupPlayers = InitializeInfantryToPlace(playersInTurnOrder, gameboardTerritories);
            PlaceArmies(territoryRequestHandler, gameSetupPlayers, gameboardTerritories);

            var gameSetup = new GameSetup(playersInTurnOrder, gameboardTerritories);
            return gameSetup;
        }

        private IList<GameSetupPlayer> InitializeInfantryToPlace(IReadOnlyCollection<IPlayer> players, IEnumerable<GameboardSetupTerritory> gameboardTerritories)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            var gameSetupPlayers = players
                .Select(player => CreatePlayer(player, numberOfStartingInfantry, gameboardTerritories))
                .ToList();
            return gameSetupPlayers;
        }

        private static GameSetupPlayer CreatePlayer(IPlayer player, int numberOfStartingInfantry, IEnumerable<GameboardSetupTerritory> gameboardTerritories)
        {
            var numberOfTerritoriesAssignedToPlayer = gameboardTerritories.Count(t => t.Player == player);
            var armiesToPlace = numberOfStartingInfantry - numberOfTerritoriesAssignedToPlayer;
            var gameSetupPlayer = new GameSetupPlayer(player, armiesToPlace);
            return gameSetupPlayer;
        }

        private List<IPlayer> ShufflePlayers()
        {
            var shuffledPlayers = _players
                .Shuffle(_shuffler)
                .ToList();
            return shuffledPlayers;
        }

        private List<GameboardSetupTerritory> AssignPlayersToTerritories(IList<IPlayer> players)
        {
            var gameboardTerritories = new List<GameboardSetupTerritory>();

            var territories = _worldMap.GetTerritories()
                .Shuffle(_shuffler)
                .ToList();

            var player = players.First();

            foreach (var territory in territories)
            {
                var gameboardTerritory = new GameboardSetupTerritory(territory, player, 1);
                gameboardTerritories.Add(gameboardTerritory);

                player = players.GetNextOrFirst(player);
            }

            return gameboardTerritories;
        }

        private static void PlaceArmies(ITerritoryRequestHandler territoryRequestHandler, IList<GameSetupPlayer> gameSetupPlayers, IList<GameboardSetupTerritory> territories)
        {
            while (AnyArmiesLeftToPlace(gameSetupPlayers))
            {
                PlaceArmiesForOneRound(territoryRequestHandler, gameSetupPlayers, territories);
            }
        }

        private static bool AnyArmiesLeftToPlace(IEnumerable<GameSetupPlayer> players)
        {
            return players.Any(x => x.HasArmiesLeftToPlace());
        }

        private static void PlaceArmiesForOneRound(ITerritoryRequestHandler territoryRequestHandler, IList<GameSetupPlayer> gameSetupPlayers, IList<GameboardSetupTerritory> territories)
        {
            var playersWithArmiesLeftToPlace = gameSetupPlayers
                .Where(x => x.HasArmiesLeftToPlace())
                .ToList();

            foreach (var gameSetupPlayer in playersWithArmiesLeftToPlace)
            {
                PlaceArmy(territoryRequestHandler, gameSetupPlayer, territories);
            }
        }

        private static void PlaceArmy(ITerritoryRequestHandler territoryRequestHandler, GameSetupPlayer gameSetupPlayer, IList<GameboardSetupTerritory> gameboardTerritories)
        {
            var gameboard = CreateGameboard(gameboardTerritories);

            var territoriesAssignedToPlayer = gameboardTerritories
                .Where(x => x.Player == gameSetupPlayer.Player)
                .ToList();

            var respondedGameboardTerritory = GetTerritoryResponse(territoryRequestHandler, gameboard, gameSetupPlayer, territoriesAssignedToPlayer);

            respondedGameboardTerritory.Armies++;
            gameSetupPlayer.ArmiesToPlace--;
        }

        private static Gameboard CreateGameboard(IEnumerable<GameboardSetupTerritory> gameboardTerritories)
        {
            var gamePlayTerritories = gameboardTerritories
                .Select(x => new GameboardTerritory(x.Territory, x.Player, x.Armies))
                .ToList();

            var gameboard = new Gameboard(gamePlayTerritories);
            return gameboard;
        }

        private static GameboardSetupTerritory GetTerritoryResponse(ITerritoryRequestHandler territoryRequestHandler, IGameboard gameboard, GameSetupPlayer gameSetupPlayer, List<GameboardSetupTerritory> territoriesAssignedToPlayer)
        {
            var selectableTerritories = territoriesAssignedToPlayer
                .Select(x => x.Territory)
                .ToList();

            var parameter = new TerritoryRequestParameter(gameboard, selectableTerritories, gameSetupPlayer);

            var territory = territoryRequestHandler.ProcessRequest(parameter);
            var gameboardTerritory = territoriesAssignedToPlayer.Single(x => x.Territory == territory);

            return gameboardTerritory;
        }
    }
}