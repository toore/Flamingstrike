using System;
using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.Views.WorldMapViews;
using NSubstitute;
using RISK.Application;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.DiceAndCalculation;
using RISK.Application.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Specifications
{
    public class GamePlaySpec : AcceptanceTestsBase<GamePlaySpec>
    {
        private GameboardViewModel _gameboardViewModel;
        private WorldMap _worldMap;
        private IWindowManager _windowManager;
        private HumanPlayer _player1;
        private HumanPlayer _player2;
        private IDice _dice;
        private GameOverViewModel _gameOverAndPlayer1IsTheWinnerViewModel;

        [Fact]
        public void Moves_armies_into_Brazil_after_win()
        {
            var gameboardViewModel = GameboardViewModelTestDataFactory.ViewModel;

            Given.
                a_started_game_with_two_players().
                player_1_occupies_every_territory_except_brazil_and_venezuela_with_one_army_each().
                player_1_has_5_armies_in_north_africa().
                player_2_occupies_brazil_and_venezuela_with_one_army_each();

            When.
                player_one_selects_north_africa().
                and_attacks_brazil_and_wins();

            Then.
                player_1_should_occupy_Brazil_with_4_armies();
        }

        [Fact]
        public void Game_over_after_player_occupies_all_territories()
        {
            Given.
                a_started_game_with_two_players().
                player_1_occupies_every_territory_except_iceland_with_one_army_each().
                player_1_has_2_armies_in_scandinavia().
                player_2_occupies_iceland_with_one_army();

            When.
                player_one_selects_scandinavia().
                and_attacks_iceland_and_wins();

            Then.
                player_1_is_the_winner();
        }

        [Fact]
        public void Receives_a_card_when_ending_turn()
        {
            Given.
                a_started_game_with_two_players().
                player_1_occupies_every_territory_except_brazil_and_venezuela_with_one_army_each().
                player_1_has_5_armies_in_north_africa().
                player_2_occupies_brazil_and_venezuela_with_one_army_each();

            When.
                player_one_selects_north_africa().
                and_attacks_brazil_and_wins().
                turn_ends();

            Then.
                player_1_should_have_a_card().
                player_2_should_take_turn();
        }

        [Fact]
        public void Does_not_receive_a_card_when_ending_turn()
        {
            Given.a_started_game_with_two_players();

            When.turn_ends();

            Then.
                player_1_should_not_have_any_card().
                player_2_should_take_turn();
        }

        [Fact]
        public void Fortifies_armies()
        {
            Given.
                a_started_game_with_two_players().
                player_1_occupies_every_territory_except_indonesia_with_ten_armies_each().
                player_2_occupies_indonesia().
                player_1_fortifies();

            When.
                player_1_selects_japan().
                moves_2_armies_to_kamchatka();

            Then.
                japan_should_have_8_armies().
                kamchatka_should_have_12_armies();
        }

        private void kamchatka_should_have_12_armies()
        {
            _worldMap.Kamchatka.Armies.Should().Be(12);
        }

        private GamePlaySpec japan_should_have_8_armies()
        {
            _worldMap.Japan.Armies.Should().Be(8);
            return this;
        }

        private GamePlaySpec player_1_selects_japan()
        {
            ClickOn(_worldMap.Japan);
            return this;
        }

        private void moves_2_armies_to_kamchatka()
        {
            ClickOn(_worldMap.Kamchatka);
        }

        private void player_1_fortifies()
        {
            throw new NotImplementedException();
            //_gameboardViewModel.Fortify();
        }

        private GamePlaySpec a_started_game_with_two_players()
        {
            _worldMap = new WorldMap();

            //var territoriesFactory = Substitute.For<ITerritoriesFactory>();
            //territoriesFactory.Create().Returns(_territories);

            _dice = Substitute.For<IDice>();
            _windowManager = Substitute.For<IWindowManager>();

            _player1 = new HumanPlayer("Player 1");
            _player2 = new HumanPlayer("Player 2");

            var gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _gameOverAndPlayer1IsTheWinnerViewModel = new GameOverViewModel(_player1);
            gameOverViewModelFactory.Create(_player1).Returns(_gameOverAndPlayer1IsTheWinnerViewModel);

            var locationSelector = Substitute.For<ITerritorySelector>();
            var alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            alternateGameSetup.InitializeWorldMap(locationSelector).Returns(_worldMap);
            var dices = new Dices(new CasualtiesCalculator(), _dice);
            var battleCalculator = new BattleCalculator(dices);
            var interactionStateFactory = new InteractionStateFactory(battleCalculator);
            var game = new Game(interactionStateFactory, new StateControllerFactory(), new[] { _player1, _player2 }, _worldMap, new CardFactory());

            //ObjectFactory.Inject<IPlayers>(_players);
            ////ObjectFactory.Inject(_territories);
            ////ObjectFactory.Inject(territoriesFactory);
            //ObjectFactory.Inject(_dice);
            //ObjectFactory.Inject(_windowManager);
            //ObjectFactory.Inject(gameOverViewModelFactory);
            //ObjectFactory.Inject<IGame>(game);

            //_gameboardViewModel = (GameboardViewModel)ObjectFactory
            //    .With(_worldMap.GetTerritories())
            //    .GetInstance<IGameboardViewModel>();

            return this;
        }

        private GamePlaySpec player_1_has_5_armies_in_north_africa()
        {
            UpdateWorldMap(_player1, 5, _worldMap.NorthAfrica);
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_brazil_and_venezuela_with_one_army_each()
        {
            UpdateWorldMap(_player1, 1, GetAllLocationsExcept(_worldMap.Brazil, _worldMap.Venezuela));
            return this;
        }

        private void player_2_occupies_brazil_and_venezuela_with_one_army_each()
        {
            UpdateWorldMap(_player2, 1, _worldMap.Brazil, _worldMap.Venezuela);
        }

        private GamePlaySpec player_1_occupies_every_territory_except_iceland_with_one_army_each()
        {
            UpdateWorldMap(_player1, 1, GetAllLocationsExcept(_worldMap.Iceland));
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_indonesia_with_ten_armies_each()
        {
            UpdateWorldMap(_player1, 10, GetAllLocationsExcept(_worldMap.Indonesia));
            return this;
        }

        private GamePlaySpec player_2_occupies_indonesia()
        {
            UpdateWorldMap(_player2, 1, _worldMap.Indonesia);
            return this;
        }

        private GamePlaySpec player_1_has_2_armies_in_scandinavia()
        {
            UpdateWorldMap(_player1, 2, _worldMap.Scandinavia);
            return this;
        }

        private void player_2_occupies_iceland_with_one_army()
        {
            UpdateWorldMap(_player2, 1, _worldMap.Iceland);
        }

        private ITerritory[] GetAllLocationsExcept(params ITerritory[] excludedLocations)
        {
            return _worldMap.GetTerritories().Except(excludedLocations).ToArray();
        }

        private void UpdateWorldMap(IPlayer player, int armies, params ITerritory[] territories)
        {
            territories
                .Apply(territory =>
                {
                    territory.Occupant = player;
                    territory.Armies = armies;
                });
        }

        private GamePlaySpec player_one_selects_north_africa()
        {
            ClickOn(_worldMap.NorthAfrica);
            return this;
        }

        private GamePlaySpec and_attacks_brazil_and_wins()
        {
            _dice.Roll().Returns(DiceValue.Six, DiceValue.Five, DiceValue.Four, DiceValue.Five);
            ClickOn(_worldMap.Brazil);
            return this;
        }

        private GamePlaySpec player_one_selects_scandinavia()
        {
            ClickOn(_worldMap.Scandinavia);
            return this;
        }

        private void and_attacks_iceland_and_wins()
        {
            _dice.Roll().Returns(DiceValue.Two, DiceValue.One);
            ClickOn(_worldMap.Iceland);
        }

        private void turn_ends()
        {
            _gameboardViewModel.EndTurn();
        }

        private void ClickOn(ITerritory location)
        {
            GetTerritoryViewModel(location).OnClick();
        }

        private ITerritoryLayoutViewModel GetTerritoryViewModel(ITerritory territory)
        {
            return _gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<TerritoryLayoutViewModel>()
                .Single(x => x.Territory == territory);
        }

        private void player_1_should_occupy_Brazil_with_4_armies()
        {
            _worldMap.NorthAfrica.Occupant.Should().Be(_player1, "player 1 should occupy North Africa");
            _worldMap.NorthAfrica.Armies.Should().Be(1, "North Africa should have 1 army");
            _worldMap.Brazil.Occupant.Should().Be(_player1, "player 1 should occupy Brazil");
            _worldMap.Brazil.Armies.Should().Be(4, "Brazil should have 4 armies");
            GetTerritoryViewModel(_worldMap.Brazil).IsSelected.Should().BeTrue("selected territory should be Brazil");
        }

        private void player_1_is_the_winner()
        {
            _windowManager.Received().ShowDialog(_gameOverAndPlayer1IsTheWinnerViewModel);
        }

        private GamePlaySpec player_1_should_have_a_card()
        {
            _player1.Cards.Count().Should().Be(1, "player 1 should have one card");
            return this;
        }

        private GamePlaySpec player_1_should_not_have_any_card()
        {
            _player1.Cards.Count().Should().Be(0, "player 1 should not have any card");
            return this;
        }

        private void player_2_should_take_turn()
        {
            _gameboardViewModel.Player.Should().Be(_player2);
        }
    }
}