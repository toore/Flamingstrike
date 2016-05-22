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
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using Toore.Shuffling;
using Xunit;
using IPlayer = RISK.GameEngine.IPlayer;

namespace RISK.Tests.GuiWpf.Specifications
{
    public class GamePlaySpec : SpecBase<GamePlaySpec>
    {
        private GameboardViewModel _gameboardViewModel;
        private Regions _regions;
        private IWindowManager _windowManager;
        private IPlayer _player1;
        private IPlayer _player2;
        private IDice _dice;
        private GameOverViewModel _gameOverAndPlayer1IsTheWinnerViewModel;
        private Sequence<IPlayer> _players;
        private readonly List<Territory> _territories = new List<Territory>();
        private IGame _game;
        private IStateControllerFactory _stateControllerFactory;
        private IInteractionStateFactory _interactionStateFactory;
        private Continents _continents;

        [Fact]
        public void Armies_are_drafted()
        {
            Given.
                a_game_with_two_players().
                player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each().
                player_1_has_5_armies_in_north_africa().
                player_2_occupies_brazil_and_venezuela_with_one_army_each().
                game_is_started();

            When.
                player_1_selects_north_africa();

            Then.
                player_1_should_occupy_north_africa_with_6_armies();
        }

        [Fact]
        public void Occupies_brazil_after_win()
        {
            Given.
                a_game_with_two_players().
                player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each().
                player_1_has_5_armies_in_north_africa().
                player_2_occupies_brazil_and_venezuela_with_one_army_each().
                game_is_started();

            When.
                player_1_selects_north_africa().
                and_attacks_brazil_and_wins().
                one_additional_army_is_sent_to_occupy_brazil();

            Then.
                player_1_should_occupy_brazil_with_4_armies().
                player_1_has_one_army_in_north_africa();
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
                player_1_selects_north_africa().
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
            ClickOn(_regions.Japan);
            return this;
        }

        private void moves_2_armies_to_kamchatka()
        {
            ClickOn(_regions.Kamchatka);
        }

        private void player_1_fortifies()
        {
            _gameboardViewModel.Fortify();
        }

        private GamePlaySpec a_game_with_two_players()
        {
            _continents = new Continents();
            _regions = new Regions(_continents);

            _dice = Substitute.For<IDice>();
            _windowManager = Substitute.For<IWindowManager>();

            _player1 = new Player("Player 1");
            _player2 = new Player("Player 2");

            _players = new Sequence<IPlayer>(_player1, _player2);

            return this;
        }

        private GamePlaySpec game_is_started()
        {
            var randomWrapper = new RandomWrapper();
            var dice = new Dice(randomWrapper);
            var dicesRoller = new DicesRoller(dice);
            var armyDraftCalculator = new ArmyDraftCalculator(_continents);
            var battle = new Battle(dicesRoller, new ArmiesLostCalculator());
            var gameDataFactory = new GameDataFactory();
            var armyDrafter = new ArmyDrafter();
            var territoryOccupier = new TerritoryOccupier();
            var fortifier = new Fortifier();
            var attacker = new Attacker(battle);
            var gameStateFactory = new GameStateFactory(gameDataFactory, armyDrafter, territoryOccupier, attacker, fortifier);
            var gameStateFsm = new GameStateFsm();
            var gameStateConductor = new GameStateConductor(gameStateFactory, armyDraftCalculator, gameDataFactory, gameStateFsm);
            _game = new Game(gameDataFactory, gameStateConductor, gameStateFsm, _players, _territories, null);
            _stateControllerFactory = new StateControllerFactory();
            _interactionStateFactory = new InteractionStateFactory();

            var regionModelFactory = new RegionModelFactory(_regions);
            var colorService = new ColorService();
            var eventAggregator = new EventAggregator();
            var territoryColorsFactory = new TerritoryColorsFactory(colorService, _regions);
            var screenService = new ScreenService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenService);
            var userNotifier = new UserNotifier(_windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, territoryColorsFactory, colorService);

            var gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _gameOverAndPlayer1IsTheWinnerViewModel = new GameOverViewModel("");
            gameOverViewModelFactory.Create(_player1.Name).Returns(_gameOverAndPlayer1IsTheWinnerViewModel);

            _gameboardViewModel = new GameboardViewModel(
                _game,
                _stateControllerFactory,
                _interactionStateFactory,
                _regions,
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
            AddTerritoryToGameboard(_regions.NorthAfrica, _player1, 5);
            //UpdateWorldMap(_player1, 5, _worldMap.NorthAfrica);
            return this;
        }

        private void AddTerritoryToGameboard(IRegion region, IPlayer player, int armies)
        {
            _territories.Add(new Territory(region, player, armies));
        }

        private GamePlaySpec player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
        {
            GetAllTerritoriesExcept(
                _regions.Brazil,
                _regions.Venezuela,
                _regions.NorthAfrica).
                Apply(x => AddTerritoryToGameboard(x, _player1, 1));

            //UpdateWorldMap(_player1, 1, GetAllLocationsExcept(_worldMap.Brazil, _worldMap.Venezuela));
            return this;
        }

        private GamePlaySpec player_2_occupies_brazil_and_venezuela_with_one_army_each()
        {
            AddTerritoryToGameboard(_regions.Brazil, _player2, 1);
            AddTerritoryToGameboard(_regions.Venezuela, _player2, 1);
            //UpdateWorldMap(_player2, 1, _worldMap.Brazil, _worldMap.Venezuela);
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_iceland_with_one_army_each()
        {
            UpdateWorldMap(_player1, 1, GetAllTerritoriesExcept(_regions.Iceland));
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_indonesia_with_ten_armies_each()
        {
            UpdateWorldMap(_player1, 10, GetAllTerritoriesExcept(_regions.Indonesia));
            return this;
        }

        private GamePlaySpec player_2_occupies_indonesia()
        {
            UpdateWorldMap(_player2, 1, _regions.Indonesia);
            return this;
        }

        private GamePlaySpec player_1_has_2_armies_in_scandinavia()
        {
            UpdateWorldMap(_player1, 2, _regions.Scandinavia);
            return this;
        }

        private void player_2_occupies_iceland_with_one_army()
        {
            UpdateWorldMap(_player2, 1, _regions.Iceland);
        }

        private IRegion[] GetAllTerritoriesExcept(params IRegion[] excludedLocations)
        {
            return _regions.GetAll().Except(excludedLocations).ToArray();
        }

        private void UpdateWorldMap(IPlayer player, int armies, params IRegion[] territoriesGeography)
        {
            //territories
            //    .Apply(territory =>
            //    {
            //        territory.Occupant = playerId;
            //        territory.Armies = armies;
            //    });
        }

        private GamePlaySpec player_1_selects_north_africa()
        {
            ClickOn(_regions.NorthAfrica);
            return this;
        }

        private GamePlaySpec and_attacks_brazil_and_wins()
        {
            _dice.Roll().Returns(6, 5, 4, 5);
            ClickOn(_regions.Brazil);
            return this;
        }

        private GamePlaySpec one_additional_army_is_sent_to_occupy_brazil()
        {
            _game.SendArmiesToOccupy(1);
            return this;
        }

        private GamePlaySpec player_one_selects_scandinavia()
        {
            ClickOn(_regions.Scandinavia);
            return this;
        }

        private void and_attacks_iceland_and_wins()
        {
            _dice.Roll().Returns(2, 1);
            ClickOn(_regions.Iceland);
        }

        private void turn_ends()
        {
            _gameboardViewModel.EndTurn();
        }

        private void ClickOn(IRegion region)
        {
            GetTerritoryViewModel(region).OnClick();
        }

        private IRegionViewModel GetTerritoryViewModel(IRegion region)
        {
            return _gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<RegionViewModel>()
                .Single(x => x.Region == region);
        }

        private GamePlaySpec player_1_should_occupy_north_africa_with_6_armies()
        {
            _game.GetTerritory(_regions.NorthAfrica).Player.Should().Be(_player1, "player 1 should occupy North Africa");
            _game.GetTerritory(_regions.NorthAfrica).Armies.Should().Be(6, "North Africa should have 6 armies");
            return this;
        }

        private GamePlaySpec player_1_should_occupy_brazil_with_4_armies()
        {
            _game.GetTerritory(_regions.Brazil).Player.Should().Be(_player1, "player 1 should occupy Brazil");
            _game.GetTerritory(_regions.Brazil).Armies.Should().Be(4, "Brazil should have 4 armies");
            return this;
        }

        private GamePlaySpec player_1_has_one_army_in_north_africa()
        {
            _game.GetTerritory(_regions.NorthAfrica).Player.Should().Be(_player1, "player 1 should occupy North Africa");
            _game.GetTerritory(_regions.NorthAfrica).Armies.Should().Be(1, "North Africa should have 1 army");
            return this;
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
            _gameboardViewModel.PlayerName.Should().Be(_player2.Name);
        }
    }
}