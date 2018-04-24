using System.Collections.Generic;
using System.Linq;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.GameEngine.Setup.Finished;
using FluentAssertions;
using NSubstitute;
using Toore.Shuffling;
using Xunit;

namespace Tests.GameEngine.Setup
{
    public class AlternateGameSetupObserverSpec : SpecBase<AlternateGameSetupObserverSpec>
    {
        private IAlternateGameSetupObserverSpy _alternateGameSetupObserverSpy;
        private IReadOnlyList<PlayerName> _players;
        private AlternateGameSetupBootstrapper _alternateGameSetupBootstrapper;
        private IShuffler _shuffler;
        private PlayerName _player1;
        private PlayerName _player2;
        private PlayerName _player3;

        [Fact]
        public void First_player_places_army_first()
        {
            Given
                .a_new_game_with_three_players()
                .player_2_takes_turn_first_then_player_1_and_finally_player_3();

            When
                .game_setup_is_initiated();

            Then
                .player_2_is_about_to_place_an_army();
        }

        [Fact]
        public void Second_player_places_army_secondly()
        {
            Given
                .a_new_game_with_three_players()
                .player_2_takes_turn_first_then_player_1_and_finally_player_3();

            When
                .game_setup_is_initiated()
                .player_places_an_army();

            Then
                .player_1_is_about_to_place_an_army();
        }

        [Fact]
        public void When_all_player_has_placed_armies_its_once_again_the_first_players_turn()
        {
            Given
                .a_new_game_with_three_players()
                .player_2_takes_turn_first_then_player_1_and_finally_player_3();

            When
                .game_setup_is_initiated()
                .player_places_an_army()
                .player_places_an_army()
                .player_places_an_army();

            Then
                .player_2_is_about_to_place_an_army();
        }

        [Fact]
        public void When_all_armies_are_placed_the_game_is_started()
        {
            Given
                .a_new_game_with_three_players()
                .player_2_takes_turn_first_then_player_1_and_finally_player_3();

            When
                .all_armies_are_placed_by_players()
                .game_setup_is_executed();

            Then
                .game_is_started();
        }

        private AlternateGameSetupObserverSpec all_armies_are_placed_by_players()
        {
            return this;
        }

        private AlternateGameSetupObserverSpec a_new_game_with_three_players()
        {
            _shuffler = Substitute.For<IShuffler>();
            var startingInfantryCalculator = new StartingInfantryCalculator();
            var regions = new List<Region>();
            _alternateGameSetupBootstrapper = new AlternateGameSetupBootstrapper(regions, _shuffler, startingInfantryCalculator);

            _player1 = new PlayerName("player 1");
            _player2 = new PlayerName("player 2");
            _player3 = new PlayerName("player 3");
            _players = new List<PlayerName>();

            _shuffler
                .Shuffle(regions)
                .Returns(new WorldMapFactory().Create().GetAll());

            return this;
        }

        private void player_2_takes_turn_first_then_player_1_and_finally_player_3()
        {
            _shuffler.Shuffle(_players).Returns(new List<PlayerName> { _player2, _player1, _player3 });
        }

        private AlternateGameSetupObserverSpec game_setup_is_initiated()
        {
            _alternateGameSetupObserverSpy = new AlternateGameSetupObserverSpyDecorator(new NullAlternateGameSetupObserver());
            _alternateGameSetupBootstrapper.Run(_alternateGameSetupObserverSpy, _players);
            return this;
        }

        private void game_setup_is_executed()
        {
            _alternateGameSetupObserverSpy = new AlternateGameSetupObserverSpyDecorator(new AutoRespondingAlternateGameSetupObserver());
            _alternateGameSetupBootstrapper.Run(_alternateGameSetupObserverSpy, _players);
        }

        private AlternateGameSetupObserverSpec player_places_an_army()
        {
            var region = _alternateGameSetupObserverSpy.TerritorySelector.GetTerritories().First(x => x.IsSelectable).Region;
            _alternateGameSetupObserverSpy.TerritorySelector.PlaceArmyInRegion(region);
            return this;
        }

        private void player_1_is_about_to_place_an_army()
        {
            _alternateGameSetupObserverSpy.TerritorySelector.Player.Should().Be(_player1);
        }

        private void player_2_is_about_to_place_an_army()
        {
            _alternateGameSetupObserverSpy.TerritorySelector.Player.Should().Be(_player2);
        }

        private void game_is_started()
        {
            _alternateGameSetupObserverSpy.GamePlaySetup.Should().BeOfType<GamePlaySetup>();
        }
    }
}