using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Base.Extensions;
using RISK.Domain.Entities;

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
        private readonly IPlayers _players;
        private readonly Locations _locations;
        private readonly IRandomSorter _randomSorter;
        private readonly IWorldMapFactory _worldMapFactory;
        private readonly IInitialArmyCountProvider _initialArmyCountProvider;
        private ILocationSelector _locationSelector;

        public AlternateGameSetup(
            IPlayers players,
            Locations locations,
            IRandomSorter randomSorter,
            IWorldMapFactory worldMapFactory,
            IInitialArmyCountProvider initialArmyCountProvider)
        {
            _players = players;
            _locations = locations;
            _randomSorter = randomSorter;
            _worldMapFactory = worldMapFactory;
            _initialArmyCountProvider = initialArmyCountProvider;
        }

        public IWorldMap Initialize(ILocationSelector locationSelector)
        {
            _locationSelector = locationSelector;

            var players = _players.GetAll().ToList();
            var playersDuringSetup = GetArmiesToSetup(players);

            var worldMap = CreateWorldMap(playersDuringSetup);

            PlaceArmies(worldMap, playersDuringSetup);

            return worldMap;
        }

        private IList<SetupArmies> GetArmiesToSetup(IList<IPlayer> players)
        {
            var armies = _initialArmyCountProvider.Get(players.Count());

            return _randomSorter.Sort(players)
                .Select(x => new SetupArmies(x, armies))
                .ToList();
        }

        private IWorldMap CreateWorldMap(IList<SetupArmies> setupArmies)
        {
            var worldMap = _worldMapFactory.Create();

            var locationsInRandomOrder = _randomSorter.Sort(_locations.GetAll())
                .ToList();

            var setupArmy = setupArmies.First();

            foreach (var location in locationsInRandomOrder)
            {
                var territory = worldMap.GetTerritory(location);
                territory.Occupant = setupArmy.GetPlayer();
                territory.Armies = 1;
                setupArmy.Decrease();

                setupArmy = setupArmies.GetNextOrFirst(setupArmy);
            }

            return worldMap;
        }

        private void PlaceArmies(IWorldMap worldMap, IList<SetupArmies> players)
        {
            while (players.AnyArmiesLeft())
            {
                players
                    .Where(x => x.HasArmies())
                    .Apply(x => PlaceArmy(x, worldMap));
            }
        }

        private void PlaceArmy(SetupArmies setupArmies, IWorldMap worldMap)
        {
            var player = setupArmies.GetPlayer();
            var locations = worldMap.GetTerritoriesOccupiedBy(player)
                .Select(x => x.Location)
                .ToList();

            var selectedLocation = _locationSelector.GetLocation(new LocationSelectorParameter(worldMap, locations, setupArmies));

            var territory = worldMap.GetTerritory(selectedLocation);
            territory.Armies++;
            setupArmies.Decrease();
        }
    }
}