using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.TerritoryModels;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Play.Attacking;
using RISK.Application.World;
using Toore.Shuffling;
using Xunit;
using Territory = RISK.Application.Play.Territory;

namespace RISK.Tests.GuiWpf.Specifications
{
    public class GamePlaySpec : SpecBase<GamePlaySpec>
    {
        private GameboardViewModel _gameboardViewModel;
        private WorldMap _worldMap;
        private IWindowManager _windowManager;
        private IPlayer _playerId1;
        private IPlayer _playerId2;
        private IDice _dice;
        private GameOverViewModel _gameOverAndPlayer1IsTheWinnerViewModel;
        private List<IInGameplayPlayer> _players;
        private List<Territory> _territories;
        private IGame _game;
        private IStateControllerFactory _stateControllerFactory;
        private IInteractionStateFactory _interactionStateFactory;

        [Fact]
        public void Moves_armies_into_Brazil_after_win()
        {
            Given.
                a_game_with_two_players().
                player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each().
                player_1_has_5_armies_in_north_africa().
                player_2_occupies_brazil_and_venezuela_with_one_army_each().
                game_is_started();

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
                a_game_with_two_players().
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
                a_game_with_two_players().
                //player_1_occupies_every_territory_except_brazil_and_venezuela_with_one_army_each().
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
            Given.
                a_game_with_two_players();

            When.
                turn_ends();

            Then.
                player_1_should_not_have_any_card().
                player_2_should_take_turn();
        }

        [Fact]
        public void Fortifies_armies()
        {
            Given.
                a_game_with_two_players().
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
            //_worldMap.Kamchatka.Armies.Should().Be(12);
        }

        private GamePlaySpec japan_should_have_8_armies()
        {
            //_worldMap.Japan.Armies.Should().Be(8);
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
            _gameboardViewModel.Fortify();
        }

        private GamePlaySpec a_game_with_two_players()
        {
            _worldMap = new WorldMap();

            _dice = Substitute.For<IDice>();
            _windowManager = Substitute.For<IWindowManager>();

            _playerId1 = new Player("Player 1");
            _playerId2 = new Player("Player 2");
            var player1 = new InGameplayPlayer(_playerId1);
            var player2 = new InGameplayPlayer(_playerId2);

            _players = new List<IInGameplayPlayer> { player1, player2 };

            _territories = new List<Territory>();

            return this;
        }

        private GamePlaySpec game_is_started()
        {
            var gameRules = new GameRules();
            var cardFactory = new CardFactory();
            var randomWrapper = new RandomWrapper();
            var dice = new Dice(randomWrapper);
            var dicesRoller = new DicesRoller(dice);
            var battle = new Battle(dicesRoller, new BattleCalculator());
            _game = new Game(_players, _territories, gameRules, cardFactory, battle);
            _stateControllerFactory = new StateControllerFactory();
            _interactionStateFactory = new InteractionStateFactory();

            var worldMapModelFactory = new WorldMapModelFactory();
            var colorService = new ColorService();
            var eventAggregator = new EventAggregator();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService, _worldMap);
            var screenService = new ScreenService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenService);
            var userNotifier = new UserNotifier(_windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(_worldMap, worldMapModelFactory, territoryColorsFactory, colorService);

            var gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _gameOverAndPlayer1IsTheWinnerViewModel = new GameOverViewModel("");
            gameOverViewModelFactory.Create(_playerId1.Name).Returns(_gameOverAndPlayer1IsTheWinnerViewModel);

            _gameboardViewModel = new GameboardViewModel(
                _game,
                _stateControllerFactory,
                _interactionStateFactory,
                _worldMap,
                worldMapViewModelFactory,
                _windowManager,
                gameOverViewModelFactory,
                dialogManager,
                eventAggregator);

            _gameboardViewModel.Activate();

            return this;
        }

        private GamePlaySpec player_1_has_5_armies_in_north_africa()
        {
            AddTerritoryToGameboard(_worldMap.NorthAfrica, _playerId1, 5);
            //UpdateWorldMap(_player1, 5, _worldMap.NorthAfrica);
            return this;
        }

        private void AddTerritoryToGameboard(ITerritoryGeography territoryGeography, IPlayer player, int armies)
        {
            _territories.Add(new Territory(territoryGeography, player, armies));
        }

        private GamePlaySpec player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
        {
            GetAllTerritoriesExcept(
                _worldMap.Brazil,
                _worldMap.Venezuela,
                _worldMap.NorthAfrica).
                Apply(x => AddTerritoryToGameboard(x, _playerId1, 1));

            //UpdateWorldMap(_player1, 1, GetAllLocationsExcept(_worldMap.Brazil, _worldMap.Venezuela));
            return this;
        }

        private GamePlaySpec player_2_occupies_brazil_and_venezuela_with_one_army_each()
        {
            AddTerritoryToGameboard(_worldMap.Brazil, _playerId2, 1);
            AddTerritoryToGameboard(_worldMap.Venezuela, _playerId2, 1);
            //UpdateWorldMap(_player2, 1, _worldMap.Brazil, _worldMap.Venezuela);
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_iceland_with_one_army_each()
        {
            UpdateWorldMap(_playerId1, 1, GetAllTerritoriesExcept(_worldMap.Iceland));
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_indonesia_with_ten_armies_each()
        {
            UpdateWorldMap(_playerId1, 10, GetAllTerritoriesExcept(_worldMap.Indonesia));
            return this;
        }

        private GamePlaySpec player_2_occupies_indonesia()
        {
            UpdateWorldMap(_playerId2, 1, _worldMap.Indonesia);
            return this;
        }

        private GamePlaySpec player_1_has_2_armies_in_scandinavia()
        {
            UpdateWorldMap(_playerId1, 2, _worldMap.Scandinavia);
            return this;
        }

        private void player_2_occupies_iceland_with_one_army()
        {
            UpdateWorldMap(_playerId2, 1, _worldMap.Iceland);
        }

        private ITerritoryGeography[] GetAllTerritoriesExcept(params ITerritoryGeography[] excludedLocations)
        {
            return _worldMap.GetAll().Except(excludedLocations).ToArray();
        }

        private void UpdateWorldMap(IPlayer player, int armies, params ITerritoryGeography[] territoriesGeography)
        {
            //territories
            //    .Apply(territory =>
            //    {
            //        territory.Occupant = playerId;
            //        territory.Armies = armies;
            //    });
        }

        private GamePlaySpec player_one_selects_north_africa()
        {
            ClickOn(_worldMap.NorthAfrica);
            return this;
        }

        private GamePlaySpec and_attacks_brazil_and_wins()
        {
            _dice.Roll().Returns(6, 5, 4, 5);
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
            _dice.Roll().Returns(2, 1);
            ClickOn(_worldMap.Iceland);
        }

        private void turn_ends()
        {
            _gameboardViewModel.EndTurn();
        }

        private void ClickOn(ITerritoryGeography location)
        {
            GetTerritoryViewModel(location).OnClick();
        }

        private ITerritoryLayoutViewModel GetTerritoryViewModel(ITerritoryGeography territoryGeography)
        {
            return _gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<TerritoryViewModel>()
                .Single(x => x.TerritoryGeography == territoryGeography);
        }

        private void player_1_should_occupy_Brazil_with_4_armies()
        {
            //_worldMap.NorthAfrica.Occupant.Should().Be(_player1, "player 1 should occupy North Africa");
            //_worldMap.NorthAfrica.Armies.Should().Be(1, "North Africa should have 1 army");
            //_worldMap.Brazil.Occupant.Should().Be(_player1, "player 1 should occupy Brazil");
            //_worldMap.Brazil.Armies.Should().Be(4, "Brazil should have 4 armies");
            //_gameAdapter.SelectedTerritory.Should().Be(_worldMap.Brazil);
        }

        private void player_1_is_the_winner()
        {
            _windowManager.Received().ShowDialog(_gameOverAndPlayer1IsTheWinnerViewModel);
        }

        private GamePlaySpec player_1_should_have_a_card()
        {
            //_player1.Cards.Count().Should().Be(1, "player 1 should have one card");
            return this;
        }

        private GamePlaySpec player_1_should_not_have_any_card()
        {
            //_player1.Cards.Count().Should().Be(0, "player 1 should not have any card");
            return this;
        }

        private void player_2_should_take_turn()
        {
            _gameboardViewModel.PlayerName.Should().Be(_playerId2.Name);
        }
    }
}