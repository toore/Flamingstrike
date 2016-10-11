﻿using System;
using System.Collections.Generic;
using System.Linq;
using RISK.GameEngine.Shuffling;

namespace RISK.GameEngine.Setup
{
    /* Alternate
     * An alternate and quicker method of setup from the original French rules is to deal out the entire deck of Risk cards (minus the wild cards), 
     * assigning players to the territories on their cards.[1] As in a standard game, playerSetupDatas still count out the same number of starting infantry 
     * and take turns placing their armies. The original rules from 1959 state that the entire deck of Risk cards (minus the wild cards) is dealt out, 
     * assigning players to the territories on their cards. One and only one army is placed on each territory before the game commences.
     */

    public interface IAlternateGameSetupObserver
    {
        void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector);
        void NewGamePlaySetup(IGamePlaySetup gamePlaySetup);
    }

    public interface IAlternateGameSetup {}

    public interface IArmyPlacer
    {
        void PlaceArmyInRegion(IPlayer currentPlayer, IRegion selectedRegion, TerritoriesAndPlayers territoriesAndPlayers);
    }

    public class AlternateGameSetup : IAlternateGameSetup, IArmyPlacer
    {
        private readonly IAlternateGameSetupObserver _alternateGameSetupObserver;
        private readonly IRegions _regions;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private readonly IShuffle _shuffle;

        public AlternateGameSetup(
            IAlternateGameSetupObserver alternateGameSetupObserver,
            IRegions regions,
            ICollection<IPlayer> players,
            IStartingInfantryCalculator startingInfantryCalculator,
            IShuffle shuffle)
        {
            _alternateGameSetupObserver = alternateGameSetupObserver;
            _regions = regions;
            _shuffle = shuffle;
            _startingInfantryCalculator = startingInfantryCalculator;

            var playerSetupDatas = Shuffle(players);
            var territoriesAndPlayers = AssignPlayersToTerritories(playerSetupDatas);

            PlaceArmies(territoriesAndPlayers);
        }

        private IReadOnlyList<PlayerSetupData> Shuffle(ICollection<IPlayer> players)
        {
            var numberOfStartingInfantry = _startingInfantryCalculator.Get(players.Count);

            return players
                .Shuffle(_shuffle)
                .Select(player => new PlayerSetupData(player, numberOfStartingInfantry))
                .ToList();
        }

        private TerritoriesAndPlayers AssignPlayersToTerritories(IReadOnlyList<PlayerSetupData> playerSetupDatas)
        {
            var territories = new List<Territory>();

            var regions = _regions.GetAll()
                .Shuffle(_shuffle)
                .ToList();

            var updatedPlayerSetupDatas = playerSetupDatas.ToList();
            var players = playerSetupDatas.Select(x => x.Player).ToList();
            var currentPlayer = players.First();

            foreach (var region in regions)
            {
                const int armies = 1;

                updatedPlayerSetupDatas = UpdatePlayerArmies(updatedPlayerSetupDatas, currentPlayer, x => x.ArmiesToPlace - armies).ToList();

                var territory = new Territory(region, currentPlayer, armies);
                territories.Add(territory);

                currentPlayer = players.GetNext(currentPlayer);
            }

            return new TerritoriesAndPlayers(territories, updatedPlayerSetupDatas, currentPlayer);
        }

        private void PlaceArmies(TerritoriesAndPlayers territoriesAndPlayers)
        {
            ContinueToPlaceArmy(territoriesAndPlayers);
        }

        private void ContinueToPlaceArmy(TerritoriesAndPlayers territoriesAndPlayers)
        {
            var playerSetupData = territoriesAndPlayers.PlayerSetupDatas.Single(x => x.Player == territoriesAndPlayers.CurrentPlayer);
            if (playerSetupData.HasArmiesLeftToPlace())
            {
                SelectRegionToPlaceArmy(playerSetupData, territoriesAndPlayers);
            }
            else
            {
                SetupHasEnded(territoriesAndPlayers);
            }
        }

        private void SetupHasEnded(TerritoriesAndPlayers territoriesAndPlayers)
        {
            var gamePlaySetup = new GamePlaySetup(
                territoriesAndPlayers.PlayerSetupDatas.Select(x => x.Player).ToList(),
                territoriesAndPlayers.Territories);

            _alternateGameSetupObserver.NewGamePlaySetup(gamePlaySetup);
        }

        public void PlaceArmyInRegion(IPlayer currentPlayer, IRegion selectedRegion, TerritoriesAndPlayers territoriesAndPlayers)
        {
            const int armiesToAdd = 1;

            var playerSetupDatas = UpdatePlayerArmies(territoriesAndPlayers.PlayerSetupDatas, currentPlayer, x => x.ArmiesToPlace - armiesToAdd).ToList();
            var territories = UpdateArmiesInTerritory(territoriesAndPlayers.Territories, selectedRegion, x => x.Armies + armiesToAdd);

            var nextPlayer = territoriesAndPlayers.PlayerSetupDatas.Select(x => x.Player).ToList().GetNext(currentPlayer);

            ContinueToPlaceArmy(new TerritoriesAndPlayers(territories.ToList(), playerSetupDatas.ToList(), nextPlayer));
        }

        private static IEnumerable<PlayerSetupData> UpdatePlayerArmies(IReadOnlyList<PlayerSetupData> playerSetupDatas, IPlayer playerArmiesToUpdate, Func<PlayerSetupData, int> armies)
        {
            var playerToUpdate = playerSetupDatas
                .Single(x => x.Player == playerArmiesToUpdate);

            var updatedPlayer = new PlayerSetupData(playerToUpdate.Player, armies(playerToUpdate));

            return playerSetupDatas.Replace(playerToUpdate, updatedPlayer);
        }

        private static IEnumerable<Territory> UpdateArmiesInTerritory(IReadOnlyList<Territory> territories, IRegion regionToUpdate, Func<ITerritory, int> armies)
        {
            var territoryToUpdate = territories
                .Single(x => x.Region == regionToUpdate);

            var updatedTerritory = new Territory(regionToUpdate, territoryToUpdate.Player, armies(territoryToUpdate));

            return territories.Replace(territoryToUpdate, updatedTerritory);
        }

        private void SelectRegionToPlaceArmy(PlayerSetupData playerSetupData, TerritoriesAndPlayers territoriesAndPlayers)
        {
            var selectableRegions = territoriesAndPlayers.Territories
                .Where(territory => territory.Player == playerSetupData.Player)
                .Select(x => x.Region)
                .ToList();

            var regionSelector = new PlaceArmyRegionSelector(this, territoriesAndPlayers, selectableRegions, playerSetupData);

            _alternateGameSetupObserver.SelectRegion(regionSelector);
        }
    }

    public class TerritoriesAndPlayers
    {
        public IReadOnlyList<Territory> Territories { get; }
        public IReadOnlyList<PlayerSetupData> PlayerSetupDatas { get; }
        public IPlayer CurrentPlayer { get; }

        public TerritoriesAndPlayers(IReadOnlyList<Territory> territories, IReadOnlyList<PlayerSetupData> playerSetupDatas, IPlayer currentPlayer)
        {
            Territories = territories;
            PlayerSetupDatas = playerSetupDatas;
            CurrentPlayer = currentPlayer;
        }
    }
}