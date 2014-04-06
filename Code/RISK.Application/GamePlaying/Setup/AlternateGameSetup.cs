﻿using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;

namespace RISK.Domain.GamePlaying.Setup
{
    public interface IAlternateGameSetup
    {
        IWorldMap Initialize(IGameInitializerLocationSelector gameInitializerLocationSelector);
    }

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
        private readonly IInitialArmyCount _initialArmyCount;
        private IGameInitializerLocationSelector _gameInitializerLocationSelector;

        public AlternateGameSetup(
            IPlayers players,
            Locations locations,
            IRandomSorter randomSorter,
            IWorldMapFactory worldMapFactory,
            IInitialArmyCount initialArmyCount)
        {
            _players = players;
            _locations = locations;
            _randomSorter = randomSorter;
            _worldMapFactory = worldMapFactory;
            _initialArmyCount = initialArmyCount;
        }

        public IWorldMap Initialize(IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            _gameInitializerLocationSelector = gameInitializerLocationSelector;

            var players = _players.GetAll().ToList();
            var setupPlayers = GetArmiesToSetup(players);

            var worldMap = CreateWorldMap(setupPlayers);

            PlaceArmies(worldMap, setupPlayers);

            return worldMap;
        }

        private IList<SetupPlayer> GetArmiesToSetup(IList<IPlayer> players)
        {
            var armies = _initialArmyCount.Get(players.Count());

            return _randomSorter.Sort(players)
                .Select(x => new SetupPlayer(x, armies))
                .ToList();
        }

        private IWorldMap CreateWorldMap(IList<SetupPlayer> setupArmies)
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
                setupArmy.DecreaseArmies();

                setupArmy = setupArmies.GetNextOrFirst(setupArmy);
            }

            return worldMap;
        }

        private void PlaceArmies(IWorldMap worldMap, IList<SetupPlayer> players)
        {
            while (players.AnyArmiesLeft())
            {
                players
                    .Where(x => x.HasArmiesLeft())
                    .Apply(x => PlaceArmy(x, worldMap));
            }
        }

        private void PlaceArmy(SetupPlayer setupPlayer, IWorldMap worldMap)
        {
            var player = setupPlayer.GetPlayer();
            var locations = worldMap.GetTerritoriesOccupiedBy(player)
                .Select(x => x.Location)
                .ToList();

            var selectedLocation = _gameInitializerLocationSelector.SelectLocation(new LocationSelectorParameter(worldMap, locations, setupPlayer));

            var territory = worldMap.GetTerritory(selectedLocation);
            territory.Armies++;
            setupPlayer.DecreaseArmies();
        }
    }
}