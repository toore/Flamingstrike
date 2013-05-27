using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.Repositories;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class MainGameViewModelTests
    {
        private IGameSettingsViewModelFactory _gameSettingsViewModelFactory;
        private IGameboardViewModelFactory _gameboardViewModelFactory;
        private IPlayerProvider _playerProvider;
        private IGameSetupViewModelFactory _gameSetupViewModelFactory;

        [SetUp]
        public void SetUp()
        {
            _gameSettingsViewModelFactory = Substitute.For<IGameSettingsViewModelFactory>();
            _gameboardViewModelFactory = Substitute.For<IGameboardViewModelFactory>();
            _playerProvider = Substitute.For<IPlayerProvider>();
            _gameSetupViewModelFactory = Substitute.For<IGameSetupViewModelFactory>();
        }

        [Test]
        public void Initialize_main_view_to_setup()
        {
            var gameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameSettingsViewModelFactory.Create().Returns(gameSettingsViewModel);

            Create().MainViewModel.Should().Be(gameSettingsViewModel);
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

            Create().Handle(gameSetupMessage);

            _playerProvider.All = new[] { player1, player2 };
        }

        [Test]
        public void Game_setup_message_starts_game()
        {
            var mainGameViewModel = Create();
            mainGameViewModel.MonitorEvents();
            var gameSetupviewModel = Substitute.For<IGameSetupViewModel>();
            _gameSetupViewModelFactory.Create(mainGameViewModel).Returns(gameSetupviewModel);

            mainGameViewModel.Handle(new GameSetupMessage { Players = new IPlayer[] { } });

            mainGameViewModel.MainViewModel.Should().Be(gameSetupviewModel);
            mainGameViewModel.ShouldRaisePropertyChangeFor(x => x.MainViewModel);
        }

        [Test]
        public void New_game_message_starts_new_game()
        {
            var startingGameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            var newGameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameSettingsViewModelFactory.Create().Returns(startingGameSettingsViewModel, newGameSettingsViewModel);
            var mainGameViewModel = Create();

            mainGameViewModel.Handle(new NewGameMessage());

            mainGameViewModel.MainViewModel.Should().Be(newGameSettingsViewModel);
        }

        private MainGameViewModel Create()
        {
            return new MainGameViewModel(_gameSettingsViewModelFactory, _gameboardViewModelFactory, _playerProvider, _gameSetupViewModelFactory);
        }
    }

}