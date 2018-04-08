using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup;
using FluentAssertions;
using NSubstitute;
using Toore.Shuffling;
using Xunit;
using Territory = FlamingStrike.GameEngine.Setup.Finished.Territory;

namespace Tests.GameEngine.Setup
{
    public class AlternateGameSetupTests
    {
        private readonly AlternateGameSetupObserverSpyDecorator _alternateGameSetupObserver;
        private readonly IRegions _regions;
        private readonly List<PlayerName> _players;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private readonly IShuffler _shuffler;
        private readonly PlayerName _player1;
        private readonly PlayerName _player2;
        private readonly List<IRegion> _regionElements;

        public AlternateGameSetupTests()
        {
            _alternateGameSetupObserver = new AlternateGameSetupObserverSpyDecorator(new AutoRespondingAlternateGameSetupObserver());
            _regions = Substitute.For<IRegions>();
            _players = new List<PlayerName> { null, null };
            _startingInfantryCalculator = Substitute.For<IStartingInfantryCalculator>();
            _shuffler = Substitute.For<IShuffler>();

            _player1 = new PlayerName("player 1");
            _player2 = new PlayerName("player 2");

            _shuffler.Shuffle(_players).Returns(new List<PlayerName> { _player1, _player2 });

            _regionElements = new List<IRegion> { null };
            _regions.GetAll().Returns(_regionElements);
        }

        [Fact]
        public void Shuffles_player_order()
        {
            var region1 = Substitute.For<IRegion>();
            var region2 = Substitute.For<IRegion>();
            _shuffler.Shuffle(_regionElements).Returns(new List<IRegion> { region1, region2 });
            _startingInfantryCalculator.Get(2).Returns(1);

            RunSetup();

            _alternateGameSetupObserver.GamePlaySetup.GetPlayers().Should().ContainInOrder(_player1, _player2);
        }

        [Fact]
        public void Shuffles_and_assigns_territories_to_players()
        {
            var region1 = Substitute.For<IRegion>();
            var region2 = Substitute.For<IRegion>();
            var region3 = Substitute.For<IRegion>();
            _shuffler.Shuffle(_regionElements).Returns(new List<IRegion> { region1, region2, region3 });
            _startingInfantryCalculator.Get(2).Returns(3);

            RunSetup();

            _alternateGameSetupObserver.GamePlaySetup.GetTerritories().Should()
                .BeEquivalentTo(
                    new[]
                        {
                            new Territory(region1, _player1, 2),
                            new Territory(region2, _player2, 3),
                            new Territory(region3, _player1, 1)
                        }, options => options.WithStrictOrdering());
        }

        private AlternateGameSetup RunSetup()
        {
            var alternateGameSetup = new AlternateGameSetup(_alternateGameSetupObserver, _regions, _startingInfantryCalculator, _shuffler);
            alternateGameSetup.Run(_players);

            return alternateGameSetup;
        }
    }
}