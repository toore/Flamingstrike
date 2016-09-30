﻿using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Setup;
using RISK.Tests.Builders;
using Toore.Shuffling;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace RISK.Tests.GameEngine.Setup
{
    public class AlternateGameSetupTests
    {
        private readonly GameSetupObserverSpyDecorator _gameSetupObserver;
        private readonly IRegions _regions;
        private readonly List<IPlayer> _players;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private readonly IShuffle _shuffler;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private List<IRegion> _regionElements;

        public AlternateGameSetupTests()
        {
            _gameSetupObserver = new GameSetupObserverSpyDecorator(new AutoRespondingGameObserver());
            _regions = Substitute.For<IRegions>();
            _players = new List<IPlayer> { null };
            _startingInfantryCalculator = Substitute.For<IStartingInfantryCalculator>();
            _shuffler = Substitute.For<IShuffle>();

            _player1 = Make.Player.Name("player 1").Build();
            _player2 = Make.Player.Name("player 2").Build();
            
            _shuffler.Shuffle(_players).Returns(new[] { _player1, _player2 });

            _regionElements = new List<IRegion> { null };
            _regions.GetAll().Returns(_regionElements);

            
        }

        [Fact]
        public void Shuffles_player_order()
        {
            var region1 = Substitute.For<IRegion>();
            var region2 = Substitute.For<IRegion>();
            _shuffler.Shuffle(_regionElements).Returns(new[] { region1, region2 });
            _startingInfantryCalculator.Get(2).Returns(1);

            Create();

            _gameSetupObserver.GamePlaySetup.Players.Should().ContainInOrder(_player1, _player2);
        }

        [Fact]
        public void Shuffles_and_assigns_territories_to_players()
        {
            var region1 = Substitute.For<IRegion>();
            var region2 = Substitute.For<IRegion>();
            var region3 = Substitute.For<IRegion>();
            _shuffler.Shuffle(_regionElements).Returns(new[] { region1, region2, region3 });
            _startingInfantryCalculator.Get(2).Returns(3);

            Create();

            _gameSetupObserver.GamePlaySetup.Territories.ShouldAllBeEquivalentTo(new[]
                {
                    new Territory(region1, _player1, 2),
                    new Territory(region2, _player2, 3),
                    new Territory(region3, _player1, 1)
                });
        }

        private AlternateGameSetup Create()
        {
            return new AlternateGameSetup(_gameSetupObserver, _regions, _players, _startingInfantryCalculator, _shuffler);
        }
    }

    public class AutoRespondingGameObserver : IGameSetupObserver
    {
        public void SelectRegion(IRegionSelector regionSelector)
        {
            var selectedRegion = regionSelector.SelectableRegions.First();
            regionSelector.SelectRegion(selectedRegion);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup) {}
    }

    public class GameSetupObserverSpyDecorator : IGameSetupObserver
    {
        private readonly IGameSetupObserver _gameSetupObserverToBeDecorated;

        public GameSetupObserverSpyDecorator(IGameSetupObserver gameSetupObserverToBeDecorated)
        {
            _gameSetupObserverToBeDecorated = gameSetupObserverToBeDecorated;
        }

        public IGamePlaySetup GamePlaySetup { get; private set; }

        public void SelectRegion(IRegionSelector regionSelector)
        {
            _gameSetupObserverToBeDecorated.SelectRegion(regionSelector);
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            GamePlaySetup = gamePlaySetup;
            _gameSetupObserverToBeDecorated.NewGamePlaySetup(gamePlaySetup);
        }
    }
}