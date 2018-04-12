using System.Collections.Generic;
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
        private readonly List<PlayerName> _players;
        private readonly IStartingInfantryCalculator _startingInfantryCalculator;
        private readonly IShuffler _shuffler;
        private readonly PlayerName _player1;
        private readonly PlayerName _player2;
        private readonly List<Region> _regions;

        public AlternateGameSetupTests()
        {
            _alternateGameSetupObserver = new AlternateGameSetupObserverSpyDecorator(new AutoRespondingAlternateGameSetupObserver());
            _players = new List<PlayerName> { null, null };
            _startingInfantryCalculator = Substitute.For<IStartingInfantryCalculator>();
            _shuffler = Substitute.For<IShuffler>();

            _player1 = new PlayerName("player 1");
            _player2 = new PlayerName("player 2");

            _shuffler.Shuffle(_players).Returns(new List<PlayerName> { _player1, _player2 });

            _regions = new List<Region> { Region.Alaska };
        }

        [Fact]
        public void Shuffles_player_order()
        {
            _shuffler.Shuffle(_regions).Returns(new List<Region> { Region.Afghanistan, Region.Alaska });
            _startingInfantryCalculator.Get(2).Returns(1);

            RunSetup();

            _alternateGameSetupObserver.GamePlaySetup.GetPlayers().Should().ContainInOrder(_player1, _player2);
        }

        [Fact]
        public void Shuffles_and_assigns_territories_to_players()
        {
            _shuffler.Shuffle(_regions).Returns(new List<Region> { Region.Afghanistan, Region.Alaska, Region.Alberta });
            _startingInfantryCalculator.Get(2).Returns(3);

            RunSetup();

            _alternateGameSetupObserver.GamePlaySetup.GetTerritories().Should()
                .BeEquivalentTo(
                    new[]
                        {
                            new Territory(Region.Afghanistan, _player1, 2),
                            new Territory(Region.Alaska, _player2, 3),
                            new Territory(Region.Alberta, _player1, 1)
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