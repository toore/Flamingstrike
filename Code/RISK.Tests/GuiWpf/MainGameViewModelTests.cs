using System.Collections.Generic;
using FluentAssertions;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Preparation;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.GameEngine;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class MainGameViewModelTests
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameboardViewModelFactory _gameboardViewModelFactory;
        private readonly IGameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IUserInteractorFactory _userInteractorFactory;

        public MainGameViewModelTests()
        {
            _playerRepository = Substitute.For<IPlayerRepository>();
            _alternateGameSetupFactory = Substitute.For<IAlternateGameSetupFactory>();
            _gamePreparationViewModelFactory = Substitute.For<IGamePreparationViewModelFactory>();
            _gameboardViewModelFactory = Substitute.For<IGameboardViewModelFactory>();
            _gameSetupViewModelFactory = Substitute.For<IGameSetupViewModelFactory>();
            _userInteractorFactory = Substitute.For<IUserInteractorFactory>();
        }

        [Fact]
        public void OnInitialize_shows_game_settings_view()
        {
            var gameSettingsViewModel = Substitute.For<IGamePreparationViewModel>();
            _gamePreparationViewModelFactory.Create().Returns(gameSettingsViewModel);
            var sut = Initialize();

            sut.OnInitialize();
            var actual = sut.ActiveItem;

            actual.Should().Be(gameSettingsViewModel);
        }

        [Fact]
        public void New_game_message_shows_game_settings_view()
        {
            var gameInitializationViewModel = Substitute.For<IGamePreparationViewModel>();
            _gamePreparationViewModelFactory.Create().Returns(gameInitializationViewModel);

            var sut = Initialize();
            sut.Handle(new NewGameMessage());

            sut.ActiveItem.Should().Be(gameInitializationViewModel);
        }

        [Fact]
        public void Setup_game_message_shows_game_setup_view()
        {
            var gameSetupViewModel = Substitute.For<IGameSetupViewModel>();
            var alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            var players = new List<IPlayer>();
            _playerRepository.GetAll().Returns(players);
            _alternateGameSetupFactory.Create(players).Returns(alternateGameSetup);
            _gameSetupViewModelFactory.Create(alternateGameSetup).Returns(gameSetupViewModel);

            var sut = Initialize();
            sut.Handle(new StartGameSetupMessage());

            sut.ActiveItem.Should().Be(gameSetupViewModel);
        }

        [Fact]
        public void Start_game_play_shows_gameboard_view()
        {
            var game = Substitute.For<IGame>();
            var gameboardViewModel = Substitute.For<IGameboardViewModel>();
            _gameboardViewModelFactory.Create(game).Returns(gameboardViewModel);

            var sut = Initialize();
            sut.Handle(new StartGameplayMessage(game));

            sut.ActiveItem.Should().Be(gameboardViewModel);
        }

        private MainGameViewModelDecorator Initialize()
        {
            return new MainGameViewModelDecorator(
                _playerRepository,
                _alternateGameSetupFactory,
                _gamePreparationViewModelFactory,
                _gameboardViewModelFactory,
                _gameSetupViewModelFactory,
                _userInteractorFactory);
        }
    }
}