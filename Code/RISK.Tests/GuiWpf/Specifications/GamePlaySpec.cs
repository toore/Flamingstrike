using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.RegionModels;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Interaction;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Play.GamePhases;
using RISK.GameEngine.Setup;
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
        private IInteractionStateFactory _interactionStateFactory;
        private Continents _continents;

        [Fact]
        public void First_players_draft_armies()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started();

            When
                .player_selects_north_africa();

            Then
                .player_1_should_occupy_north_africa_with_six_armies();
        }

        [Fact]
        public void Second_player_draft_armies()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_thirtyfive_armies_in_scandinavia()
                .player_ends_turn();

            When
                .player_drafts_three_armies_in_brazil();

            Then
                .player_2_should_occupy_brazil_with_4_armies();
        }

        [Fact]
        public void First_player_draft_armies_second_turn()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_thirtyfive_armies_in_scandinavia()
                .player_ends_turn()
                .player_drafts_three_armies_in_brazil();

            When
                .player_ends_turn();

            Then
                .player_1_should_take_turn();
        }

        [Fact]
        public void First_player_occupies_brazil_after_win()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_selects_north_africa()
                .player_attacks_brazil_and_wins()
                .two_additional_armies_is_sent_to_occupy_brazil();

            Then
                .player_1_should_occupy_brazil_with_five_armies()
                .player_1_should_occupy_north_africa_with_three_armies();
        }

        [Fact]
        public void Receives_a_card_when_ending_turn()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_selects_north_africa()
                .player_attacks_brazil_and_wins()
                .two_additional_armies_is_sent_to_occupy_brazil()
                .player_ends_turn();

            Then
                .player_1_should_have_a_card();
        }

        [Fact]
        public void Does_not_receive_a_card_when_ending_turn()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_ends_turn();

            Then
                .player_1_should_not_have_any_card();
        }

        [Fact]
        public void Fortifies_armies()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_and_venezuela_and_north_africa_with_one_army_each()
                .player_1_occupies_north_africa_with_five_armies()
                .player_2_occupies_brazil_and_venezuela_with_one_army_each()
                .game_is_started()
                .player_drafts_three_armies_in_north_africa()
                .player_drafts_thirtytwo_armies_in_iceland();

            When
                .player_enters_fortify_mode()
                .player_selects_north_africa()
                .player_moves_one_army_to_east_africa();

            Then
                .east_africa_should_have_two_armies();
        }

        [Fact]
        public void Game_over_when_player_occupies_all_territories()
        {
            Given
                .a_game_with_two_human_players()
                .player_1_occupies_every_territory_except_brazil_with_one_army_each()
                .player_2_occupies_brazil_with_one_army()
                .game_is_started()
                .player_drafts_thirtyfive_armies_in_north_africa();

            When.
                player_selects_north_africa().
                player_attacks_brazil_and_wins();

            Then.
                player_1_is_the_winner();
        }

        private void player_1_should_have_a_card()
        {
            _player1.Cards.Count().Should().Be(1);
        }

        private void player_1_should_not_have_any_card()
        {
            _player1.Cards.Should().BeEmpty();
        }

        private GamePlaySpec player_enters_fortify_mode()
        {
            _gameboardViewModel.EnterFortifyMode();
            return this;
        }

        private void player_moves_one_army_to_east_africa()
        {
            ClickOn(_regions.EastAfrica);
        }

        private GamePlaySpec a_game_with_two_human_players()
        {
            _continents = new Continents();
            _regions = new Regions(_continents);

            _player1 = new Player("Player 1");
            _player2 = new Player("Player 2");

            return this;
        }

        private GamePlaySpec game_is_started()
        {
            _dice = Substitute.For<IDice>();
            var dicesRoller = new DicesRoller(_dice);
            var armyDraftCalculator = new ArmyDraftCalculator(_continents);
            var battle = new Battle(dicesRoller, new ArmiesLostCalculator());
            var gameDataFactory = new GameDataFactory();
            var armyDrafter = new ArmyDrafter();
            var territoryOccupier = new TerritoryOccupier();
            var fortifier = new Fortifier();
            var attacker = new Attacker(battle);
            var gameRules = new GameRules();
            var gameStateFactory = new GameStateFactory(gameDataFactory, armyDrafter, territoryOccupier, attacker, fortifier, gameRules);
            var gameStateFsm = new GameStateFsm();
            var gameStateConductor = new GameStateConductor(gameStateFactory, armyDraftCalculator, gameDataFactory, gameStateFsm);
            var deckFactory = new DeckFactory(_regions, new FisherYatesShuffle(new RandomWrapper()));
            var gameFactory = new GameFactory(gameDataFactory, gameStateConductor, deckFactory, gameStateFsm, gameRules);

            _players = new Sequence<IPlayer>(_player1, _player2);
            var gamePlaySetup = new GamePlaySetup(_players, _territories);
            _game = gameFactory.Create(gamePlaySetup);
            var interactionStateFsm = new InteractionStateFsm();
            _interactionStateFactory = new InteractionStateFactory(interactionStateFsm);

            var regionModelFactory = new RegionModelFactory(_regions);
            var colorService = new ColorService();
            var eventAggregator = new EventAggregator();
            var regionColorSettingsFactory = new RegionColorSettingsFactory(colorService, _regions);
            var screenConfirmationService = new ScreenConfirmationService();
            var confirmViewModelFactory = new ConfirmViewModelFactory(screenConfirmationService);
            _windowManager = Substitute.For<IWindowManager>();
            var userNotifier = new UserNotifier(_windowManager, confirmViewModelFactory);
            var dialogManager = new DialogManager(userNotifier);
            var worldMapViewModelFactory = new WorldMapViewModelFactory(regionModelFactory, regionColorSettingsFactory, colorService);

            var gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _gameOverAndPlayer1IsTheWinnerViewModel = new GameOverViewModel("");
            gameOverViewModelFactory.Create(_player1.Name).Returns(_gameOverAndPlayer1IsTheWinnerViewModel);

            _gameboardViewModel = new GameboardViewModel(
                _game,
                interactionStateFsm,
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

        private GamePlaySpec player_drafts_three_armies_in_north_africa()
        {
            ClickOnRegionNumberOfTimes(_regions.NorthAfrica, 3);
            return this;
        }

        private GamePlaySpec player_drafts_thirtytwo_armies_in_iceland()
        {
            ClickOnRegionNumberOfTimes(_regions.Iceland, 32);
            return this;
        }

        private GamePlaySpec player_drafts_thirtyfive_armies_in_scandinavia()
        {
            ClickOnRegionNumberOfTimes(_regions.Scandinavia, 35);
            return this;
        }

        private GamePlaySpec player_drafts_thirtyfive_armies_in_north_africa()
        {
            ClickOnRegionNumberOfTimes(_regions.NorthAfrica, 35);
            return this;
        }

        private GamePlaySpec player_drafts_three_armies_in_brazil()
        {
            ClickOnRegionNumberOfTimes(_regions.Brazil, 3);
            return this;
        }

        private void ClickOnRegionNumberOfTimes(IRegion region, int times)
        {
            for (var i = 0; i < times; i++)
            {
                ClickOn(region);
            }
        }

        private GamePlaySpec player_1_occupies_north_africa_with_five_armies()
        {
            AddTerritoryToGameboard(_regions.NorthAfrica, _player1, 5);
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

            return this;
        }

        private GamePlaySpec player_2_occupies_brazil_and_venezuela_with_one_army_each()
        {
            AddTerritoryToGameboard(_regions.Brazil, _player2, 1);
            AddTerritoryToGameboard(_regions.Venezuela, _player2, 1);
            return this;
        }

        private GamePlaySpec player_1_occupies_every_territory_except_brazil_with_one_army_each()
        {
            GetAllTerritoriesExcept(
                _regions.Brazil).
                Apply(x => AddTerritoryToGameboard(x, _player1, 1));
            return this;
        }

        private GamePlaySpec player_2_occupies_brazil_with_one_army()
        {
            AddTerritoryToGameboard(_regions.Brazil, _player2, 1);
            return this;
        }

        private IRegion[] GetAllTerritoriesExcept(params IRegion[] excludedLocations)
        {
            return _regions.GetAll().Except(excludedLocations).ToArray();
        }

        private GamePlaySpec player_selects_north_africa()
        {
            ClickOn(_regions.NorthAfrica);
            return this;
        }

        private GamePlaySpec player_attacks_brazil_and_wins()
        {
            _dice.Roll().Returns(6, 5, 4, 5);
            ClickOn(_regions.Brazil);
            return this;
        }

        private GamePlaySpec two_additional_armies_is_sent_to_occupy_brazil()
        {
            _game.SendArmiesToOccupy(2);
            return this;
        }

        private GamePlaySpec player_ends_turn()
        {
            _gameboardViewModel.EndTurn();
            return this;
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

        private GamePlaySpec player_1_should_occupy_north_africa_with_six_armies()
        {
            _game.GetTerritory(_regions.NorthAfrica).Player.Should().Be(_player1);
            _game.GetTerritory(_regions.NorthAfrica).Armies.Should().Be(6);
            return this;
        }

        private GamePlaySpec player_2_should_occupy_brazil_with_4_armies()
        {
            _game.GetTerritory(_regions.Brazil).Player.Should().Be(_player2);
            _game.GetTerritory(_regions.Brazil).Armies.Should().Be(4);
            return this;
        }

        private GamePlaySpec player_1_should_occupy_brazil_with_five_armies()
        {
            _game.GetTerritory(_regions.Brazil).Player.Should().Be(_player1);
            _game.GetTerritory(_regions.Brazil).Armies.Should().Be(5);
            return this;
        }

        private GamePlaySpec player_1_should_occupy_north_africa_with_three_armies()
        {
            _game.GetTerritory(_regions.NorthAfrica).Player.Should().Be(_player1);
            _game.GetTerritory(_regions.NorthAfrica).Armies.Should().Be(3);
            return this;
        }

        private void east_africa_should_have_two_armies()
        {
            _game.GetTerritory(_regions.EastAfrica).Armies.Should().Be(2);
        }

        private void player_1_is_the_winner()
        {
            _windowManager.Received().ShowDialog(_gameOverAndPlayer1IsTheWinnerViewModel);
        }

        private void player_1_should_take_turn()
        {
            _game.CurrentPlayer.Should().Be(_player1);
        }
    }
}