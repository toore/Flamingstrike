using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using RISK.Application.Entities;
using RISK.Application.Extensions;

namespace RISK.Application.GamePlaying.Setup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, players still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public interface IAlternateGameSetup
    {
        IWorldMap InitializeWorldMap(ITerritorySelector territorySelector);
    }

    public class AlternateGameSetup : IAlternateGameSetup
    {
        private readonly IPlayers _players;
        private readonly IWorldMapFactory _worldMapFactory;
        private readonly IRandomSorter _randomSorter;
        private readonly IInitialArmyAssignmentCalculator _initialArmyAssignmentCalculator;
        private ITerritorySelector _territorySelector;

        public AlternateGameSetup(IPlayers players, IWorldMapFactory worldMapFactory, IRandomSorter randomSorter, IInitialArmyAssignmentCalculator initialArmyAssignmentCalculator)
        {
            _players = players;
            _worldMapFactory = worldMapFactory;
            _randomSorter = randomSorter;
            _initialArmyAssignmentCalculator = initialArmyAssignmentCalculator;
        }

        public IWorldMap InitializeWorldMap(ITerritorySelector territorySelector)
        {
            _territorySelector = territorySelector;

            var players = GetArmiesToSetup(_players.GetAll());
            var worldMap = CreateWorldMapWithRandomOccupants(players);
            
            PlaceRestOfArmies(worldMap, players);

            return worldMap;
        }

        private IList<Player> GetArmiesToSetup(IEnumerable<IPlayer> players)
        {
            var armies = _initialArmyAssignmentCalculator.Get(players.Count());

            return _randomSorter.Sort(players)
                .Select(x => new Player(x, armies))
                .ToList();
        }

        private IWorldMap CreateWorldMapWithRandomOccupants(IList<Player> players)
        {
            var worldMap = _worldMapFactory.Create();

            var territoriesInRandomOrder = _randomSorter.Sort(worldMap.GetTerritories())
                .ToList();

            var player = players.First();

            foreach (var territory in territoriesInRandomOrder)
            {
                territory.Occupant = player.GetPlayer();
                territory.Armies = 1;
                player.ArmiesToPlace--;

                player = players.GetNextOrFirst(player);
            }

            return worldMap;
        }

        private void PlaceRestOfArmies(IWorldMap worldMap, IList<Player> players)
        {
            while (AnyPlayerHaveArmiesLeftToPlace(players))
            {
                players
                    .Where(player => player.HasArmiesToPlace())
                    .Apply(player => PlaceArmy(worldMap, player));
            }
        }

        private static bool AnyPlayerHaveArmiesLeftToPlace(IEnumerable<Player> players)
        {
            return players.Any(x => x.HasArmiesToPlace());
        }

        private void PlaceArmy(IWorldMap worldMap, Player player)
        {
            var territoriesOccupiedByPlayer = worldMap.GetTerritoriesOccupiedByPlayer(player.GetPlayer());
            var selectedTerritory = _territorySelector.SelectLocation(new LocationSelectorParameter(worldMap.GetTerritories(), territoriesOccupiedByPlayer, player));

            selectedTerritory.Armies++;
            player.ArmiesToPlace--;
        }
    }
}