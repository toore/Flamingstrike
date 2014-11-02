using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Application.Entities;
using RISK.Application.Extensions;

namespace RISK.Application.GamePlaying.Setup
{
    public interface IAlternateGameSetup
    {
        Territories Initialize(IGameInitializerLocationSelector gameInitializerLocationSelector);
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
        private readonly Territories _territories;
        private readonly IRandomSorter _randomSorter;
        private readonly ITerritoriesFactory _territoriesFactory;
        private readonly IInitialArmyCount _initialArmyCount;
        private IGameInitializerLocationSelector _gameInitializerLocationSelector;

        public AlternateGameSetup(
            IPlayers players,
            Territories territories,
            IRandomSorter randomSorter,
            ITerritoriesFactory territoriesFactory,
            IInitialArmyCount initialArmyCount)
        {
            _players = players;
            _territories = territories;
            _randomSorter = randomSorter;
            _territoriesFactory = territoriesFactory;
            _initialArmyCount = initialArmyCount;
        }

        public Territories Initialize(IGameInitializerLocationSelector gameInitializerLocationSelector)
        {
            _gameInitializerLocationSelector = gameInitializerLocationSelector;

            var players = _players.GetAll().ToList();
            var setupPlayers = GetArmiesToSetup(players);

            var territories = CreateTerritories(setupPlayers);

            PlaceArmies(territories, setupPlayers);

            return territories;
        }

        private IList<PlayerDuringGameSetup> GetArmiesToSetup(IList<IPlayer> players)
        {
            var armies = _initialArmyCount.Get(players.Count());

            return _randomSorter.Sort(players)
                .Select(x => new PlayerDuringGameSetup(x, armies))
                .ToList();
        }

        private Territories CreateTerritories(IList<PlayerDuringGameSetup> players)
        {
            var territories = _territoriesFactory.Create();

            var territoriesInRandomOrder = _randomSorter.Sort(_territories.GetAll())
                .ToList();

            var player = players.First();

            foreach (var territory in territoriesInRandomOrder)
            {
                territory.Occupant = player.GetPlayer();
                territory.Armies = 1;
                player.ArmyHasBeenPlaced();

                player = players.GetNextOrFirst(player);
            }

            return territories;
        }

        private void PlaceArmies(Territories territories, IList<PlayerDuringGameSetup> players)
        {
            while (players.AnyArmiesLeft())
            {
                players
                    .Where(player => player.HasArmiesLeft())
                    .Apply(player => PlaceArmy(territories, player));
            }
        }

        private void PlaceArmy(Territories territories, PlayerDuringGameSetup player)
        {
            var territoriesOccupiedByPlayer = territories.GetTerritoriesOccupiedByPlayer(player.GetPlayer());
            var selectedTerritory = _gameInitializerLocationSelector.SelectLocation(new LocationSelectorParameter(territories.GetAll(), territoriesOccupiedByPlayer, player));

            selectedTerritory.Armies++;
            player.ArmyHasBeenPlaced();
        }
    }
}