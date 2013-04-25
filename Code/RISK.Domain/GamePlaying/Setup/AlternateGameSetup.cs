using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying.Setup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, players still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILocationProvider _locationProvider;
        private readonly IRandomSorter _randomSorter;
        private readonly IWorldMapFactory _worldMapFactory;
        private readonly IInitialArmyCountProvider _initialArmyCountProvider;
        private ILocationSelector _locationSelector;

        public AlternateGameSetup(
            IPlayerRepository playerRepository,
            ILocationProvider locationProvider,
            IRandomSorter randomSorter,
            IWorldMapFactory worldMapFactory,
            IInitialArmyCountProvider initialArmyCountProvider)
        {
            _playerRepository = playerRepository;
            _locationProvider = locationProvider;
            _randomSorter = randomSorter;
            _worldMapFactory = worldMapFactory;
            _initialArmyCountProvider = initialArmyCountProvider;
        }

        public IWorldMap Initialize(ILocationSelector locationSelector)
        {
            _locationSelector = locationSelector;

            var players = _playerRepository.GetAll()
                .ToList();
            var playerInSetup = GetPlayersInSetup(players);

            var worldMap = CreateWorldMap(playerInSetup);

            PlaceArmies(worldMap, playerInSetup);

            return worldMap;
        }

        private IList<PlayerInSetup> GetPlayersInSetup(IList<IPlayer> players)
        {
            var armies = _initialArmyCountProvider.Get(players.Count());

            return _randomSorter.Sort(players)
                .Select(x => new PlayerInSetup(x, armies))
                .ToList();
        }

        private IWorldMap CreateWorldMap(IList<PlayerInSetup> players)
        {
            var worldMap = _worldMapFactory.Create();

            var locationsInRandomOrder = _randomSorter.Sort(_locationProvider.GetAll())
                .ToList();

            var player = players.First();

            foreach (var location in locationsInRandomOrder)
            {
                var territory = worldMap.GetTerritory(location);
                territory.AssignedToPlayer = player.GetInGamePlayer();
                territory.Armies = 1;
                player.Armies--;

                player = players.GetNextOrFirst(player);
            }

            return worldMap;
        }

        private void PlaceArmies(IWorldMap worldMap, IList<PlayerInSetup> players)
        {
            while (players.Any(x => x.Armies > 0))
            {
                players
                    .Where(x => x.Armies > 0)
                    .Apply(x => PlaceArmy(x, worldMap));
            }
        }

        private void PlaceArmy(PlayerInSetup playerInSetup, IWorldMap worldMap)
        {
            var player = playerInSetup.GetInGamePlayer();
            var locations = worldMap.GetTerritoriesAssignedTo(player)
                .Select(x => x.Location);
            var selectedLocation = _locationSelector.Select(locations);

            var territory = worldMap.GetTerritory(selectedLocation);
            territory.Armies++;
            playerInSetup.Armies--;
        }
    }
}