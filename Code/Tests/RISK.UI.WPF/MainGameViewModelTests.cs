﻿using FluentAssertions;
using NSubstitute;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;
using RISK.UI.WPF.ViewModels.AlternateSetup;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Messages;
using RISK.UI.WPF.ViewModels.Preparation;
using Tests.RISK.GameEngine.Builders;
using Xunit;

namespace Tests.RISK.UI.WPF
{
    public class MainGameViewModelTests
    {
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IAlternateGameSetupFactory _alternateGameSetupFactory;
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameplayViewModelFactory _gameplayViewModelFactory;
        private readonly IAlternateGameSetupViewModelFactory _alternateGameSetupViewModelFactory;
        private readonly IGameFactory _gameFactory;

        public MainGameViewModelTests()
        {
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _alternateGameSetupFactory = Substitute.For<IAlternateGameSetupFactory>();
            _gamePreparationViewModelFactory = Substitute.For<IGamePreparationViewModelFactory>();
            _gameplayViewModelFactory = Substitute.For<IGameplayViewModelFactory>();
            _alternateGameSetupViewModelFactory = Substitute.For<IAlternateGameSetupViewModelFactory>();
            _gameFactory = Substitute.For<IGameFactory>();
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
            var gameSetupViewModel = Substitute.For<IAlternateGameSetupViewModel>();
            _alternateGameSetupViewModelFactory.Create().Returns(gameSetupViewModel);

            var sut = Initialize();
            sut.Handle(new StartGameSetupMessage());

            sut.ActiveItem.Should().Be(gameSetupViewModel);
        }

        [Fact]
        public void Start_game_play_message_shows_gameplay_view()
        {
            var gameplayViewModel = Substitute.For<IGameplayViewModel>();
            _gameplayViewModelFactory.Create().Returns(gameplayViewModel);

            var sut = Initialize();
            sut.Handle(new StartGameplayMessage(Make.GamePlaySetup.Build()));

            sut.ActiveItem.Should().Be(gameplayViewModel);
        }

        private MainGameViewModelDecorator Initialize()
        {
            return new MainGameViewModelDecorator(
                _playerUiDataRepository,
                _alternateGameSetupFactory,
                _gamePreparationViewModelFactory,
                _gameplayViewModelFactory,
                _alternateGameSetupViewModelFactory,
                _gameFactory);
        }
    }
}