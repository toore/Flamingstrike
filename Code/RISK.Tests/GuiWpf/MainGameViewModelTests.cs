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
        private IGameSetupViewModel _gameSetupViewModel;
        private IGameboardViewModelFactory _gameboardViewModelFactory;
        private IPlayerRepository _playerRepository;

        [SetUp]
        public void SetUp()
        {
            _gameFactory = Substitute.For<IGameFactory>();
            _gameSetupViewModel = Substitute.For<IGameSetupViewModel>();
            _gameboardViewModelFactory = Substitute.For<IGameboardViewModelFactory>();
            _playerRepository = Substitute.For<IPlayerRepository>();

            _mainGameViewModel = new MainGameViewModel(_gameFactory, _gameSetupViewModel, _gameboardViewModelFactory, _playerRepository);
        }

        [Test]
        public void Initialize_main_view_to_setup()
        {

            _mainGameViewModel.MainViewModel.Should().Be(_gameSetupViewModel);
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
        public void Game_setup_message_starts_new_game()
        {
            var game = Substitute.For<IGame>();
            _gameFactory.Create(_mainGameViewModel).Returns(game);
            var gameboardViewModel = Substitute.For<IGameboardViewModel>();
            _gameboardViewModelFactory.Create(game).Returns(gameboardViewModel);
            var gameSetupMessage = new GameSetupMessage { Players = new IPlayer[] { } };
            _mainGameViewModel.MonitorEvents();

            _mainGameViewModel.Handle(gameSetupMessage);

            _mainGameViewModel.MainViewModel.Should().Be(gameboardViewModel);
            _mainGameViewModel.ShouldRaisePropertyChangeFor(x => x.MainViewModel);
        }
    }
}