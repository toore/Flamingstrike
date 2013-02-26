using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Domain.GamePlaying
{
    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IRandomizedPlayerRepository _randomizedPlayerRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IRandomWrapper _randomWrapper;
        private List<ILocation> _allLocations;
        private IWorldMap _worldMap;

        public AlternateGameSetup(IRandomizedPlayerRepository randomizedPlayerRepository, ILocationRepository locationRepository, IRandomWrapper randomWrapper)
        {
            _randomizedPlayerRepository = randomizedPlayerRepository;
            _locationRepository = locationRepository;
            _randomWrapper = randomWrapper;
        }

        public void Initialize(IWorldMap worldMap)
        {
            _worldMap = worldMap;

            var players = _randomizedPlayerRepository
                .GetAllInRandomizedOrder()
                .ToList();

            _allLocations = _locationRepository
                .GetAll()
                .ToList();

            while (_allLocations.Any())
            {
                players.Apply(OccupyRandomTerritory);  TODO fixa detta om ojämt antal länder och spelare
            }
        }

        private void OccupyRandomTerritory(IPlayer player)
        {
            var randomLocation = GetRandomLocation();
            _worldMap.GetTerritory(randomLocation).Owner = player;
            _allLocations.Remove(randomLocation);
        }

        private ILocation GetRandomLocation()
        {
            var randomLocationIndex = _randomWrapper.Next(_allLocations.Count - 1);
            return _allLocations.ElementAt(randomLocationIndex);
        }
    }
}