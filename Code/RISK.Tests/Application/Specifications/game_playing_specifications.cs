using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Base.Extensions;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.DiceAndCalculation;
using RISK.Domain.GamePlaying.Setup;
using StructureMap;

namespace RISK.Tests.Application.Specifications
{
    public class game_playing_specifications : NSpecDebuggerShim
    {
        private Locations _locations;
        private IPlayer _player1;
        private IPlayer _player2;
        private IMainGameViewModel _mainGameViewModel;
        private IWorldMap _worldMap;
        private Players _players;
        private IWindowManager _windowManager;
        private IGameboardViewModel _gameboardViewModel;
        private GameOverViewModel _gameOverViewModel;

        public void before_all()
        {
            new PluginConfiguration().Configure();
        }

        public void game_is_setup_and_started()
        {
            before = () =>
            {
                InjectPlayerRepository();
                InjectLocationProvider();
                InjectWorldMapFactory();

                _mainGameViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

                SelectTwoHumanPlayersAndConfirm();

                _player1 = _players.GetAll().First();
                _player2 = _players.GetAll().Second();
            };

            it["game board is shown"] = () => _mainGameViewModel.MainViewModel.Should().BeOfType<GameSetupViewModel>();

            context["Armies are placed"] = () =>
            {
                act = () => PlaceArmies();
                it["game board view model should be visible"] = () => _mainGameViewModel.MainViewModel.Should().BeOfType<GameboardViewModel>();
            };
        }

        public void selecting_North_Africa_and_attacking_Brazil_and_win_moves_armies_into_territory_and_flags_that_user_should_receive_a_card_when_turn_ends()
        {
            before = () =>
            {
                InjectPlayerRepository();
                InjectLocationProvider();
                InjectWorldMapFactory();
                InjectDiceRollerWithReturningSixFiveFourAndThenFiveForTwoAttacks();
                InjectWindowManager();
                StubPlayerRepositoryWithTwoHumanPlayers();
                InjectGameOverViewModelFactory();

                InjectGame();

                _gameboardViewModel = ObjectFactory
                    .With(_locations.GetAll())
                    .GetInstance<IGameboardViewModel>();

                PlayerOneOccupiesNorthAfricaWithFiveArmies();
                PlayerTwoOccupiesBrazilAndVenezuela();
                PlayerOneOccupiesEveryTerritoryExceptBrazilVenezuelaAndNorthAfrica();
            };

            act = () =>
            {
                ClickOn(_locations.NorthAfrica);
                ClickOn(_locations.Brazil);
            };

            it["when player 1 attacks Brazil from North Africa"] = () =>
            {
                _worldMap.GetTerritory(_locations.NorthAfrica).Occupant.Should().Be(_player1, "player 1 should occupy North Africa");
                _worldMap.GetTerritory(_locations.NorthAfrica).Armies.Should().Be(1, "North Africa should have 1 army");
                _worldMap.GetTerritory(_locations.Brazil).Occupant.Should().Be(_player1, "player 1 should occupy Brazil");
                GetTerritoryViewModel(_locations.Brazil).IsSelected.Should().BeTrue("selected territory should be Brazil");
                _worldMap.GetTerritory(_locations.Brazil).Armies.Should().Be(4, "Brazil should have 4 armies");
            };

            context["when player 1 turn ends"] = () =>
            {
                act = () => { EndTurn(); };

                it["player 1 should have a card when turn ends"] = () => _player1.Cards.Count().Should().Be(1);
            };

            context["when player 1 attacks again"] = () =>
            {
                act = () => ClickOn(_locations.Venezuela);

                it["player 1 should occupy Venezuela with 3 armies"] = () =>
                {
                    _worldMap.GetTerritory(_locations.Venezuela).Occupant.Should().Be(_player1, "player 1 should occupy Venezuela");
                    _worldMap.GetTerritory(_locations.Venezuela).Armies.Should().Be(3, "Venezuela should have 3 armies");

                    _windowManager.Received().ShowDialog(_gameOverViewModel);
                };
            };
        }

        private void InjectGameOverViewModelFactory()
        {
            var gameOverViewModelFactory = Substitute.For<IGameOverViewModelFactory>();
            _gameOverViewModel = new GameOverViewModel(_player1);
            gameOverViewModelFactory.Create(_player1).Returns(_gameOverViewModel);

            ObjectFactory.Inject(gameOverViewModelFactory);
        }

        private void StubPlayerRepositoryWithTwoHumanPlayers()
        {
            _player1 = new HumanPlayer("Player 1");
            _player2 = new HumanPlayer("Player 2");
            _players.SetPlayers(new[] { _player1, _player2 });
        }

        private void InjectGame()
        {
            var locationSelector = Substitute.For<ILocationSelector>();
            var alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            alternateGameSetup.Initialize(locationSelector).Returns(_worldMap);
            var game = new Game(ObjectFactory.GetInstance<ITurnFactory>(), _players, alternateGameSetup, locationSelector);
            ObjectFactory.Inject<IGame>(game);
        }

        private void PlaceArmies()
        {
            const int numberOfArmiesToPlace = (40 - 21) * 2;

            for (int i = 0; i < numberOfArmiesToPlace; i++)
            {
                var gameSetupViewModel = (GameSetupViewModel)_mainGameViewModel.MainViewModel;
                var firstEnabledterritoryViewModel = gameSetupViewModel.WorldMapViewModel.WorldMapViewModels
                    .OfType<TerritoryLayoutViewModel>()
                    .First(x => x.IsEnabled);

                gameSetupViewModel.SelectLocation(firstEnabledterritoryViewModel.Location);
            }
        }

        private void EndTurn()
        {
            _gameboardViewModel.EndTurn();
        }

        private void SelectTwoHumanPlayersAndConfirm()
        {
            var gameSetupViewModel = (IGameSettingsViewModel)_mainGameViewModel.MainViewModel;

            gameSetupViewModel.Players.First().IsEnabled = true;
            gameSetupViewModel.Players.Second().IsEnabled = true;

            gameSetupViewModel.Confirm();
        }

        private void PlayerOneOccupiesNorthAfricaWithFiveArmies()
        {
            UpdateTerritory(_locations.NorthAfrica, _player1, 5);
        }

        private void PlayerTwoOccupiesBrazilAndVenezuela()
        {
            UpdateTerritory(_locations.Brazil, _player2, 1);
            UpdateTerritory(_locations.Venezuela, _player2, 1);
        }

        private void UpdateTerritory(ILocation location, IPlayer owner, int armies)
        {
            var territory = _worldMap.GetTerritory(location);
            territory.Occupant = owner;
            territory.Armies = armies;
        }

        private void PlayerOneOccupiesEveryTerritoryExceptBrazilVenezuelaAndNorthAfrica()
        {
            var excludedTerritories = new[]
            {
                _locations.Brazil,
                _locations.Venezuela,
                _locations.NorthAfrica
            };

            _locations.GetAll()
                .Where(x => !excludedTerritories.Contains(x))
                .Select(x => _worldMap.GetTerritory(x))
                .Apply(x =>
                {
                    x.Occupant = _player1;
                    x.Armies = 1;
                });
        }

        private void ClickOn(ILocation location)
        {
            GetTerritoryViewModel(location)
                .OnClick();
        }

        private ITerritoryLayoutViewModel GetTerritoryViewModel(ILocation location)
        {
            return _gameboardViewModel.WorldMapViewModel.WorldMapViewModels
                .OfType<ITerritoryLayoutViewModel>()
                .Single(x => x.Location == location);
        }

        private void InjectPlayerRepository()
        {
            _players = new Players();
            ObjectFactory.Inject<IPlayers>(_players);
        }

        private void InjectLocationProvider()
        {
            _locations = new Locations(new Continents());
            ObjectFactory.Inject(_locations);
        }

        private void InjectWorldMapFactory()
        {
            _worldMap = new WorldMap(_locations);

            var worldMapFactory = Substitute.For<IWorldMapFactory>();
            worldMapFactory.Create().Returns(_worldMap);

            ObjectFactory.Inject(worldMapFactory);
        }

        private void InjectDiceRollerWithReturningSixFiveFourAndThenFiveForTwoAttacks()
        {
            var diceRoller = Substitute.For<IDiceRoller>();
            diceRoller.Roll().Returns(
                DiceValue.Six, DiceValue.Five, DiceValue.Four, DiceValue.Five,
                DiceValue.Six, DiceValue.Five, DiceValue.Four, DiceValue.Five);
            ObjectFactory.Inject(diceRoller);
        }

        public void InjectWindowManager()
        {
            _windowManager = Substitute.For<IWindowManager>();
            ObjectFactory.Inject(_windowManager);
        }
    }
}