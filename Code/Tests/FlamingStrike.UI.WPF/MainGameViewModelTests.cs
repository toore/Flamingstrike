using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Tests.FlamingStrike.GameEngine.Setup;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF
{
    public class MainGameViewModelTests
    {
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IAlternateGameSetupBootstrapper _alternateGameSetupBootstrapper;
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameplayViewModelFactory _gameplayViewModelFactory;
        private readonly IAlternateGameSetupViewModelFactory _alternateGameSetupViewModelFactory;
        private readonly IGameFactory _gameFactory;

        public MainGameViewModelTests()
        {
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _alternateGameSetupBootstrapper = Substitute.For<IAlternateGameSetupBootstrapper>();
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
            sut.Handle(new StartGameplayMessage(new GamePlaySetupBuilder().Build()));

            sut.ActiveItem.Should().Be(gameplayViewModel);
        }

        private MainGameViewModelDecorator Initialize()
        {
            return new MainGameViewModelDecorator(
                _playerUiDataRepository,
                _alternateGameSetupBootstrapper,
                _gamePreparationViewModelFactory,
                _gameplayViewModelFactory,
                _alternateGameSetupViewModelFactory,
                _gameFactory);
        }
    }
}