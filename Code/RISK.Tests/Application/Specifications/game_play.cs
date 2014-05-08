using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using RISK.Domain.GamePlaying.Setup;
using StructureMap;
using Xunit;

namespace RISK.Tests.Application.Specifications
{
    public class game_play : AcceptanceTestsBase<game_play>
    {
        private GameboardViewModel _gameboardViewModel;
        private Players _players;
        private Locations _locations;
        private WorldMap _worldMap;
        private IWindowManager _windowManager;
        private HumanPlayer _player1;
        private HumanPlayer _player2;
        private IDice _dice;
        private GameOverViewModel _gameOverAndPlayer1IsTheWinner;

        public game_play()
        {
            new PluginConfiguration().Configure();
        }

        [Fact]
        public void Moves_armies_into_Brazil_after_win()
        {
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

        private game_play a_started_game_with_two_players()
        {
            _players = new Players();
            _locations = new Locations(new Continents());

            _worldMap = new WorldMap(_locations);
            var worldMapFactory = Substitute.For<IWorldMapFactory>();
            worldMapFactory.Create().Returns(_worldMap);

            _dice = Substitute.For<IDice>();
            _windowManager = Substitute.For<IWindowManager>();

            _player1 = new HumanPlayer("Player 1");
            _player2 = new HumanPlayer("Player 2");
            _players.SetPlayers(new[] { _player1, _player2 });

            var gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _gameOverAndPlayer1IsTheWinner = new GameOverViewModel(_player1);
            gameOverViewModelFactory.Create(_player1).Returns(_gameOverAndPlayer1IsTheWinner);

            var locationSelector = Substitute.For<IGameInitializerLocationSelector>();
            var alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            alternateGameSetup.Initialize(locationSelector).Returns(_worldMap);
            var game = new Game(new TurnFactory(new BattleCalculator(new Dices(new CasualtyEvaluator(), _dice)), new CardFactory()), _players, alternateGameSetup, locationSelector);

            ObjectFactory.Inject<IPlayers>(_players);
            ObjectFactory.Inject(_locations);
            ObjectFactory.Inject(worldMapFactory);
            ObjectFactory.Inject(_dice);
            ObjectFactory.Inject(_windowManager);
            ObjectFactory.Inject(gameOverViewModelFactory);
            ObjectFactory.Inject<IGame>(game);

            _gameboardViewModel = (GameboardViewModel)ObjectFactory
                .With(_locations.GetAll())
                .GetInstance<IGameboardViewModel>();

            return this;
        }

        private game_play player_1_has_5_armies_in_north_africa()
        {
            UpdateWorldMap(_player1, 5, _locations.NorthAfrica);
            return this;
        }

        private game_play player_1_occupies_every_territory_except_brazil_and_venezuela_with_one_army_each()
        {
            UpdateWorldMap(_player1, 1, GetAllLocationsExcept(_locations.Brazil, _locations.Venezuela));
            return this;
        }

        private game_play player_2_occupies_brazil_and_venezuela_with_one_army_each()
        {
            UpdateWorldMap(_player2, 1, _locations.Brazil, _locations.Venezuela);
            return this;
        }

        private game_play player_1_occupies_every_territory_except_iceland_with_one_army_each()
        {
            UpdateWorldMap(_player1, 1, GetAllLocationsExcept(_locations.Iceland));
            return this;
        }

        private game_play player_1_has_2_armies_in_scandinavia()
        {
            UpdateWorldMap(_player1, 2, _locations.Scandinavia);
            return this;
        }

        private game_play player_2_occupies_iceland_with_one_army()
        {
            UpdateWorldMap(_player2, 1, _locations.Iceland);
            return this;
        }

        private ILocation[] GetAllLocationsExcept(params ILocation[] excludedLocations)
        {
            return _locations.GetAll().Except(excludedLocations).ToArray();
        }

        private void UpdateWorldMap(IPlayer player, int armies, params ILocation[] locations)
        {
            locations
                .Select(x => _worldMap.GetTerritory(x))
                .Apply(territory =>
                {
                    territory.Occupant = player;
                    territory.Armies = armies;
                });
        }

        private game_play player_one_selects_north_africa()
        {
            ClickOn(_locations.NorthAfrica);
            return this;
        }

        private game_play and_attacks_brazil_and_wins()
        {
            _dice.Roll().Returns(DiceValue.Six, DiceValue.Five, DiceValue.Four, DiceValue.Five);
            ClickOn(_locations.Brazil);
            return this;
        }

        private game_play player_one_selects_scandinavia()
        {
            ClickOn(_locations.Scandinavia);
            return this;
        }

        private game_play and_attacks_iceland_and_wins()
        {
            _dice.Roll().Returns(DiceValue.Two, DiceValue.One);
            ClickOn(_locations.Iceland);
            return this;
        }

        private void turn_ends()
        {
            _gameboardViewModel.EndTurn();
        }

        private void ClickOn(ILocation location)
        {
            GetTerritoryViewModel(location).OnClick();
        }

        private ITerritoryLayoutViewModel GetTerritoryViewModel(ILocation location)
        {
            return _gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<ITerritoryLayoutViewModel>()
                .Single(x => x.Location == location);
        }

        private void player_1_should_occupy_Brazil_with_4_armies()
        {
            _worldMap.GetTerritory(_locations.NorthAfrica).Occupant.Should().Be(_player1, "player 1 should occupy North Africa");
            _worldMap.GetTerritory(_locations.NorthAfrica).Armies.Should().Be(1, "North Africa should have 1 army");
            _worldMap.GetTerritory(_locations.Brazil).Occupant.Should().Be(_player1, "player 1 should occupy Brazil");
            _worldMap.GetTerritory(_locations.Brazil).Armies.Should().Be(4, "Brazil should have 4 armies");
            GetTerritoryViewModel(_locations.Brazil).IsSelected.Should().BeTrue("selected territory should be Brazil");
        }

        private void player_1_is_the_winner()
        {
            _windowManager.Received().ShowDialog(_gameOverAndPlayer1IsTheWinner);
        }

        private game_play player_1_should_have_a_card()
        {
            _player1.Cards.Count().Should().Be(1, "player 1 should have one card");
            return this;
        }

        private game_play player_1_should_not_have_any_card()
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