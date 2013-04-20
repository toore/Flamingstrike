using System.Linq;
using RISK.Domain.Extensions;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
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
        private readonly IRandomOrderer _randomOrderer;
        private readonly IWorldMapFactory _worldMapFactory;

        public AlternateGameSetup(IPlayerRepository playerRepository, ILocationProvider locationProvider, IRandomOrderer randomOrderer, IWorldMapFactory worldMapFactory)
        {
            _playerRepository = playerRepository;
            _locationProvider = locationProvider;
            _randomOrderer = randomOrderer;
            _worldMapFactory = worldMapFactory;
        }

        public IWorldMap Initialize()
        {
            var worldMap = _worldMapFactory.Create();

            var players = _playerRepository.GetAll();
            var playersInRandomizedOrder = _randomOrderer.OrderByRandomOrder(players)
                .ToList();

            var locations = _locationProvider.GetAll();
            var locationsInRandomizedOrder = _randomOrderer.OrderByRandomOrder(locations);

            var player = playersInRandomizedOrder.First();

            foreach (var location in locationsInRandomizedOrder)
            {
                worldMap.GetTerritory(location).Owner = player;

                player = playersInRandomizedOrder.GetNextOrFirst(player);
            }

            return worldMap;
        }
    }
}