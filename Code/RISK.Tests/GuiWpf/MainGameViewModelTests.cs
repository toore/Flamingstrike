using System.Collections.Generic;
using System.Windows.Documents;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Setup;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class MainGameViewModelTests
    {
        private readonly IGameInitializationViewModelFactory _gameInitializationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IPlayerRepository _playerRepository;

        public MainGameViewModelTests()
        {
            _gameInitializationViewModelFactory = Substitute.For<IGameInitializationViewModelFactory>();
            _gameboardViewModelFactory = Substitute.For<IGameboardViewModelFactory>();
            _gameSetupViewModelFactory = Substitute.For<IGameSetupViewModelFactory>();
            _alternateGameSetupFactory = Substitute.For<IAlternateGameSetupFactory>();
            _playerRepository = Substitute.For<IPlayerRepository>();
        }

        [Fact(Skip = "OnInitialize is protected?")]
        public void OnInitialize_starts_new_game()
        {
            var gameSettingsViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameInitializationViewModelFactory.Create().Returns(gameSettingsViewModel);

            var actual = Initialize().ActiveItem;

            actual.Should().Be(gameSettingsViewModel);
        }

        [Fact]
        public void New_game_message_shows_game_initialization_view()
        {
            var gameInitializationViewModel = Substitute.For<IGameSettingsViewModel>();
            _gameInitializationViewModelFactory.Create().Returns(gameInitializationViewModel);

            var sut = Initialize();
            sut.Handle(new NewGameMessage());

            sut.ActiveItem.Should().Be(gameInitializationViewModel);
        }

        [Fact]
        public void Setup_game_message_shows_setup_game_phase_view()
        {
            var gameSetupviewModel = Substitute.For<IGameSetupViewModel>();
            var alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            var players = new List<IPlayerId>();
            _playerRepository.GetAll().Returns(players);
            _alternateGameSetupFactory.Create(players).Returns(alternateGameSetup);
            _gameSetupViewModelFactory.Create(alternateGameSetup).Returns(gameSetupviewModel);

            var sut = Initialize();
            sut.Handle(new SetupGameMessage());

            sut.ActiveItem.Should().Be(gameSetupviewModel);
        }

        [Fact]
        public void Start_game_play_shows_the_game_playing_view()
        {
            var game = Substitute.For<IGame>();
            var gameboardViewModel = Substitute.For<IGameboardViewModel>();
            _gameboardViewModelFactory.Create(game).Returns(gameboardViewModel);

            var sut = Initialize();
            sut.Handle(new StartGameplayMessage(game));

            sut.ActiveItem.Should().Be(gameboardViewModel);
        }

        private MainGameViewModel Initialize()
        {
            return new MainGameViewModel(
                _gameInitializationViewModelFactory,
                _gameboardViewModelFactory,
                _gameSetupViewModelFactory,
                _alternateGameSetupFactory,
                _playerRepository);
        }
    }
}