using System.Collections.Generic;
using System.Linq;
using RISK.Application.Extensions;
using RISK.Application.GamePlay;
using RISK.Application.Shuffling;
using RISK.Application.World;
using Toore.Shuffling;

namespace RISK.Application.GameSetup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, players still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public interface IAlternateGameSetup
    {
        IGameboard Initialize(ITerritoryRequestHandler territoryRequestHandler);
    }

    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IList<IPlayerId> _playerIds;
        private readonly IWorldMap _worldMap;
        private readonly IShuffler _shuffler;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;

        public AlternateGameSetup(IList<IPlayerId> playerIds, IWorldMap worldMap, IShuffler shuffler, IStartingInfantryCalculator startingInfantryCalculator)
        {
            _playerIds = playerIds;
            _worldMap = worldMap;
            _shuffler = shuffler;
            _startingInfantryCalculator = startingInfantryCalculator;
        }

        public IGameboard Initialize(ITerritoryRequestHandler territoryRequestHandler)
        {
            var players = ShufflePlayersAndInitializeStartingInfantry(_playerIds);
            var gameSetupState = AssignPlayersToShuffledTerritories(players);
            var gameboard = PlaceAllArmies(territoryRequestHandler, gameSetupState);

            return gameboard;
        }

        private IList<Player> ShufflePlayersAndInitializeStartingInfantry(ICollection<IPlayerId> players)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            var shuffledPlayers = players
                .Shuffle(_shuffler)
                .Select(x => new Player(x, numberOfStartingInfantry))
                .ToList();
            return shuffledPlayers;
        }

        private GameSetupState AssignPlayersToShuffledTerritories(IList<Player> players)
        {
            var territories = _worldMap.GetTerritories()
                .Shuffle(_shuffler)
                .ToList();

            var playerIds = players.Select(x => x.PlayerId).ToList();
            var playerId = playerIds.First();

            var gameboardTerritories = new List<GameboardTerritory>();
            var updatedPlayers = players.ToList();

            foreach (var territory in territories)
            {
                var gameboardTerritory = new GameboardTerritory(territory, playerId, 1);
                gameboardTerritories.Add(gameboardTerritory);

                updatedPlayers = PlaceArmyAndUpdatePlayers(playerId, updatedPlayers);

                playerId = playerIds.GetNextOrFirst(playerId);
            }

            var gameSetupState = new GameSetupState(updatedPlayers, gameboardTerritories);
            return gameSetupState;
        }

        private static List<Player> PlaceArmyAndUpdatePlayers(IPlayerId playerId, List<Player> players)
        {
            var player = players.Single(x => x.PlayerId == playerId);
            var updatedPlayer = new Player(playerId, player.GetNumberOfArmiesLeftToPlace() - 1);
            var updatedPlayers = players.ReplaceFirstMatchingElement(updatedPlayer, p => p.PlayerId == player.PlayerId);

            return updatedPlayers;
        }

        //private static List<Player> UpdatePlayerCollection(List<Player> players, Player updatedPlayer)
        //{
        //    var playerIndex = players.FindIndex(x => x.PlayerId == updatedPlayer.PlayerId);
        //    var updatedPlayers = players.ToList();
        //    updatedPlayers.RemoveAt(playerIndex);
        //    updatedPlayers.Insert(playerIndex, updatedPlayer);

        //    return updatedPlayers;
        //}

        private static IGameboard PlaceAllArmies(ITerritoryRequestHandler territoryRequestHandler, GameSetupState gameSetupState)
        {
            while (AnyArmiesLeftToPlace(gameSetupState.Players))
            {
                gameSetupState = PlaceArmies(territoryRequestHandler, gameSetupState);
            }

            var gameboard = CreateGameboard(gameSetupState);
            return gameboard;
        }

        private static GameSetupState PlaceArmies(ITerritoryRequestHandler territoryRequestHandler, GameSetupState gameSetupState)
        {
            var playersWithArmiesLeftToPlace = gameSetupState.Players
                    .Where(x => x.HasArmiesLeftToPlace())
                    .ToList();

            foreach (var player in playersWithArmiesLeftToPlace)
            {
                gameSetupState = PlaceArmy(territoryRequestHandler, gameSetupState, player);
            }

            return gameSetupState;
        }

        private static bool AnyArmiesLeftToPlace(IEnumerable<Player> players)
        {
            return players.Any(x => x.HasArmiesLeftToPlace());
        }

        private static GameSetupState PlaceArmy(ITerritoryRequestHandler territoryRequestHandler, GameSetupState gameSetupState, Player player)
        {
            var territoriesAssignedToPlayer = gameSetupState.GameboardTerritories
                .Where(x => x.PlayerId == player.PlayerId)
                .ToList();

            var allowedTerritories = territoriesAssignedToPlayer
                .Select(x => x.Territory)
                .ToList();

            var gameboard = CreateGameboard(gameSetupState);

            var respondedTerritory = GetTerritoryResponse(territoryRequestHandler, player, gameboard, allowedTerritories);
            var respondedGameboardTerritory = territoriesAssignedToPlayer.Single(x => x.Territory == respondedTerritory);

            var updatedGameboardTerritory = new GameboardTerritory(respondedGameboardTerritory.Territory, respondedGameboardTerritory.PlayerId, respondedGameboardTerritory.Armies);
            var updatedGameboardTerritories = gameSetupState.GameboardTerritories
                .ReplaceFirstMatchingElement(updatedGameboardTerritory, x => x == respondedGameboardTerritory).ToList();

            var updatedPlayer = new Player(player.PlayerId, player.GetNumberOfArmiesLeftToPlace());
            var updatedPlayers = gameSetupState.Players
                .ReplaceFirstMatchingElement(updatedPlayer, p => p.PlayerId == player.PlayerId);

            var updatedGameSetupState = new GameSetupState(updatedPlayers, updatedGameboardTerritories);
            return updatedGameSetupState;
        }

        private static Gameboard CreateGameboard(GameSetupState gameSetupState)
        {
            var gamePlayTerritories = gameSetupState.GameboardTerritories
                .Select(x => new GamePlay.GameboardTerritory(x.Territory, x.PlayerId, x.Armies))
                .ToList();

            var gameboard = new Gameboard(gamePlayTerritories);
            return gameboard;
        }

        private static ITerritory GetTerritoryResponse(ITerritoryRequestHandler territoryRequestHandler, Player player, IGameboard gameboard, IEnumerable<ITerritory> allowedTerritories)
        {
            var parameter = new TerritoryRequestParameter(gameboard, allowedTerritories, player);
            var territory = territoryRequestHandler.ProcessRequest(parameter);
            return territory;
        }
    }
}