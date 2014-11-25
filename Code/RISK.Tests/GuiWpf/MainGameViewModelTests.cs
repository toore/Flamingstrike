using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class MainGameViewModelTests
    {
        private readonly IGameSettingsViewModelFactory _gameSettingsViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;

        public MainGameViewModelTests()
        {
            _gameSettingsViewModelFactory = Substitute.For<IGameSettingsViewModelFactory>();
            _gameboardViewModelFactory = Substitute.For<IGameboardViewModelFactory>();
            _gameSetupViewModelFactory = Substitute.For<IGameSetupViewModelFactory>();
        }

        [Fact]
        public void Initialize_main_view_to_setup()
        {
            var gameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameSettingsViewModelFactory.Create().Returns(gameSettingsViewModel);

            var actual = CreateSut().ActiveItem;

            actual.Should().Be(gameSettingsViewModel);
        }

        [Fact]
        public void Game_setup_message_starts_game()
        {
            var mainGameViewModel = CreateSut();
            mainGameViewModel.MonitorEvents();
            var gameSetupviewModel = Substitute.For<IGameSetupViewModel>();
            _gameSetupViewModelFactory.Create(mainGameViewModel).Returns(gameSetupviewModel);

            mainGameViewModel.Handle(new GameSetupMessage());

            mainGameViewModel.ActiveItem.Should().Be(gameSetupviewModel);
            mainGameViewModel.ShouldRaisePropertyChangeFor(x => x.ActiveItem);
        }

        [Fact]
        public void New_game_message_starts_new_game()
        {
            var startingGameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            var newGameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameSettingsViewModelFactory.Create().Returns(startingGameSettingsViewModel, newGameSettingsViewModel);
            var mainGameViewModel = CreateSut();

            mainGameViewModel.Handle(new NewGameMessage());

            mainGameViewModel.ActiveItem.Should().Be(newGameSettingsViewModel);
        }

        private MainGameViewModel CreateSut()
        {
            return new MainGameViewModel(_gameSettingsViewModelFactory, _gameboardViewModelFactory, _gameSetupViewModelFactory);
        }
    }
}