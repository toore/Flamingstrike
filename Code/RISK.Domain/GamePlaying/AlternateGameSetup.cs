using System.Linq;
using RISK.Domain.Extensions;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IRandomizeOrderer _randomizeOrderer;
        private IWorldMap _worldMap;

        public AlternateGameSetup(IPlayerRepository playerRepository, ILocationRepository locationRepository, IRandomizeOrderer randomizeOrderer)
        {
            _playerRepository = playerRepository;
            _locationRepository = locationRepository;
            _randomizeOrderer = randomizeOrderer;
        }

        public void Initialize(IWorldMap worldMap)
        {
            _worldMap = worldMap;

            var players = _playerRepository.GetAll();
            var playersInRandomizedOrder = _randomizeOrderer.OrderByRandomOrder(players)
                .ToList();

            var locations = _locationRepository.GetAll();
            var locationsInRandomizedOrder = _randomizeOrderer.OrderByRandomOrder(locations);

            var player = playersInRandomizedOrder.First();

            foreach (var location in locationsInRandomizedOrder)
            {
                _worldMap.GetTerritory(location).Owner = player;

                player = playersInRandomizedOrder.GetNextOrFirst(player);
            }
        }
    }
}