using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Application.GamePlaying;
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

        [Fact(Skip = "OnInitialize is protected?")]
        public void OnInitialize_starts_new_game()
        {
            var gameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameSettingsViewModelFactory.Create().Returns(gameSettingsViewModel);

            var actual = CreateSut().ActiveItem;

            actual.Should().Be(gameSettingsViewModel);
        }

        [Fact]
        public void Game_setup_message_starts_game()
        {
            var gameSetupviewModel = Substitute.For<IGameSetupViewModel>();
            _gameSetupViewModelFactory.Create().Returns(gameSetupviewModel);

            var sut = CreateSut();
            sut.Handle(new GameSetupMessage());

            sut.ActiveItem.Should().Be(gameSetupviewModel);
        }

        [Fact]
        public void New_game_message_starts_new_game()
        {
            var gameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameSettingsViewModelFactory.Create().Returns(gameSettingsViewModel);

            var sut = CreateSut();
            sut.Handle(new NewGameMessage());

            sut.ActiveItem.Should().Be(gameSettingsViewModel);
        }

        [Fact]
        public void Start_game_start_the_game()
        {
            var game = Substitute.For<IGame>();
            var gameboardViewModel = Substitute.For<IGameboardViewModel>();
            _gameboardViewModelFactory.Create(game).Returns(gameboardViewModel);

            var sut = CreateSut();
            sut.Handle(new StartGameMessage(game));

            sut.ActiveItem.Should().Be(gameboardViewModel);
        }

        private MainGameViewModel CreateSut()
        {
            return new MainGameViewModel(_gameSettingsViewModelFactory, _gameboardViewModelFactory, _gameSetupViewModelFactory);
        }
    }
}