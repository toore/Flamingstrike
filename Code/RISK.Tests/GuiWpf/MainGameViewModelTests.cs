using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.Repositories;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class MainGameViewModelTests
    {
        private MainGameViewModel _mainGameViewModel;
        private IGameFactory _gameFactory;
        private IGameSettingsViewModel _gameSettingsViewModel;
        private IGameboardViewModelFactory _gameboardViewModelFactory;
        private IPlayerRepository _playerRepository;
        private IGameSetupViewModelFactory _gameSetupViewModelFactory;
        private GameSetupMessage _gameSetupMessage;
        private IGameSetupViewModel _gameSetupViewModel;

        [SetUp]
        public void SetUp()
        {
            _gameFactory = Substitute.For<IGameFactory>();
            _gameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameboardViewModelFactory = Substitute.For<IGameboardViewModelFactory>();
            _playerRepository = Substitute.For<IPlayerRepository>();
            _gameSetupViewModelFactory = Substitute.For<IGameSetupViewModelFactory>();

            _gameSetupViewModel = Substitute.For<IGameSetupViewModel>();

            _mainGameViewModel = new MainGameViewModel(_gameSettingsViewModel, _gameboardViewModelFactory, _playerRepository, _gameSetupViewModelFactory);

            _gameSetupViewModelFactory.Create(_mainGameViewModel).Returns(_gameSetupViewModel);


            _gameSetupMessage = new GameSetupMessage { Players = new IPlayer[] { } };
        }

        [Test]
        public void Initialize_main_view_to_setup()
        {
            _mainGameViewModel.MainViewModel.Should().Be(_gameSettingsViewModel);
        }

        [Test]
        public void Game_setup_message_adds_players()
        {
            var player1 = Substitute.For<IPlayer>();
            var player2 = Substitute.For<IPlayer>();
            var gameSetupMessage = new GameSetupMessage
                {
                    Players = new[]
                        {
                            player1,
                            player2
                        }
                };

            _mainGameViewModel.Handle(gameSetupMessage);

            _playerRepository.Received().Add(player1);
            _playerRepository.Received().Add(player2);
        }

        [Test]
        [Ignore]
        public void Game_setup_message_starts_new_game_view()
        {
            var game = Substitute.For<IGame>();
            _gameFactory.Create(_gameSetupViewModel).Returns(game);
            var gameboardViewModel = Substitute.For<IGameboardViewModel>();
            _gameboardViewModelFactory.Create(game).Returns(gameboardViewModel);
            _mainGameViewModel.MonitorEvents();

            _mainGameViewModel.Handle(_gameSetupMessage);

            _mainGameViewModel.MainViewModel.Should().Be(gameboardViewModel);
            _mainGameViewModel.ShouldRaisePropertyChangeFor(x => x.MainViewModel);
        }

        [Test]
        [Ignore]
        public void Game_setup_message_shows_game_setup_board_during_gameplay_setup()
        {
            IMainGameViewViewModel mainViewModelDuringGameCreation = null;
            _gameFactory
                .When(x => x.Create(_gameSetupViewModel))
                .Do(x => mainViewModelDuringGameCreation = _mainGameViewModel.MainViewModel);

            _mainGameViewModel.Handle(_gameSetupMessage);

            mainViewModelDuringGameCreation.Should().Be(_gameSetupViewModelFactory);
        }
    }
}