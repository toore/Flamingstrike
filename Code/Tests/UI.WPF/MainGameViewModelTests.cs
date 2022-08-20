using System.Threading;
using System.Threading.Tasks;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished.Territory;

namespace Tests.UI.WPF
{
    public class MainGameViewModelTests
    {
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IGamePreparationViewModelFactory _gamePreparationViewModelFactory;
        private readonly IGameplayViewModelFactory _gameplayViewModelFactory;
        private readonly IAlternateGameSetupViewModelFactory _alternateGameSetupViewModelFactory;
        private readonly IGameEngineClient _gameEngineClient;

        public MainGameViewModelTests()
        {
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _gamePreparationViewModelFactory = Substitute.For<IGamePreparationViewModelFactory>();
            _gameplayViewModelFactory = Substitute.For<IGameplayViewModelFactory>();
            _alternateGameSetupViewModelFactory = Substitute.For<IAlternateGameSetupViewModelFactory>();
            _gameEngineClient = Substitute.For<IGameEngineClient>();
        }

        [Fact]
        public async Task OnInitialize_shows_game_settings_view()
        {
            var gameSettingsViewModel = Substitute.For<IGamePreparationViewModel>();
            _gamePreparationViewModelFactory.Create().Returns(gameSettingsViewModel);
            var sut = Initialize();

            await sut.OnInitializeAsync(CancellationToken.None);
            var actual = sut.ActiveItem;

            actual.Should().Be(gameSettingsViewModel);
        }

        [Fact]
        public async Task New_game_message_shows_game_settings_view()
        {
            var gameInitializationViewModel = Substitute.For<IGamePreparationViewModel>();
            _gamePreparationViewModelFactory.Create().Returns(gameInitializationViewModel);

            var sut = Initialize();
            await sut.HandleAsync(new NewGameMessage(), CancellationToken.None);

            sut.ActiveItem.Should().Be(gameInitializationViewModel);
        }

        [Fact]
        public async Task Setup_game_message_shows_game_setup_view()
        {
            var gameSetupViewModel = Substitute.For<IAlternateGameSetupViewModel>();
            _alternateGameSetupViewModelFactory.Create().Returns(gameSetupViewModel);

            var sut = Initialize();
            await sut.HandleAsync(new StartGameSetupMessage(), CancellationToken.None);

            sut.ActiveItem.Should().Be(gameSetupViewModel);
        }

        [Fact]
        public async Task Start_game_play_message_shows_gameplay_view()
        {
            var gameplayViewModel = Substitute.For<IGameplayViewModel>();
            _gameplayViewModelFactory.Create().Returns(gameplayViewModel);

            var sut = Initialize();
            await sut.HandleAsync(new StartGameplayMessage(new GamePlaySetup(new string[] {}, new Territory[] {})), CancellationToken.None);

            sut.ActiveItem.Should().Be(gameplayViewModel);
        }

        private MainGameViewModelDecorator Initialize()
        {
            return new MainGameViewModelDecorator(
                _playerUiDataRepository,
                _gamePreparationViewModelFactory,
                _gameplayViewModelFactory,
                _alternateGameSetupViewModelFactory,
                _gameEngineClient);
        }
    }
}