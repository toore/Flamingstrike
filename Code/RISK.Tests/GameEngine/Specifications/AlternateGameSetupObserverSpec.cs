using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Setup;
using RISK.Tests.Builders;
using Toore.Shuffling;
using Xunit;

namespace RISK.Tests.GameEngine.Specifications
{
    public class AlternateGameSetupObserverSpec : SpecBase<AlternateGameSetupObserverSpec>
    {
        private readonly AlternateGameSetupObserverSpy _alternateGameSetupObserverSpy = new AlternateGameSetupObserverSpy();
        private IReadOnlyList<IPlayer> _players;
        private AlternateGameSetupFactory _alternateGameSetupFactory;
        private IShuffle _shuffle;
        private IPlayer _player1;
        private IPlayer _player2;
        private IPlayer _player3;

        [Fact]
        public void Player_2_places_army_first()
        {
            Given
                .a_new_game_with_three_players()
                .player_2_takes_turn_first();

            When
                .the_game_setup_is_started();

            Then
                .player_2_place_army_first_of_all();
        }

        private AlternateGameSetupObserverSpec a_new_game_with_three_players()
        {
            var continents = new Continents();
            var regions = new Regions(continents);
            _shuffle = Substitute.For<IShuffle>();
            var startingInfantryCalculator = new StartingInfantryCalculator();
            _alternateGameSetupFactory = new AlternateGameSetupFactory(regions, _shuffle, startingInfantryCalculator);

            _player1 = Make.Player.Name("player 1").Build();
            _player2 = Make.Player.Name("player 2").Build();
            _player3 = Make.Player.Name("player 3").Build();
            _players = new[] { _player1, _player2, _player3 };

            return this;
        }

        private void player_2_takes_turn_first()
        {
            _shuffle.Shuffle(_players).Returns(new[] { _player2, _player1, _player3 });
        }

        private void the_game_setup_is_started()
        {
            var alternateGameSetup = _alternateGameSetupFactory.Create(_alternateGameSetupObserverSpy, _players);
        }

        private void player_2_place_army_first_of_all()
        {
            _alternateGameSetupObserverSpy.PlaceArmyRegionSelector.PlayerName.Should().Be("player 2");
        }
    }

    internal class AlternateGameSetupObserverSpy : IAlternateGameSetupObserver
    {
        public IPlaceArmyRegionSelector PlaceArmyRegionSelector { get; private set; }
        public IGamePlaySetup GamePlaySetup { get; private set; }

        public void SelectRegion(IPlaceArmyRegionSelector placeArmyRegionSelector)
        {
            PlaceArmyRegionSelector = placeArmyRegionSelector;
        }

        public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
        {
            GamePlaySetup = gamePlaySetup;
        }
    }
}