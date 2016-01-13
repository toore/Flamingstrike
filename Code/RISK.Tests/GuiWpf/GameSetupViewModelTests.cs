using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using RISK.Application.Setup;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameSetupViewModelTests
    {
        private readonly IGameFactory _gameFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly NoGuiThreadDispatcher _guiThreadDispatcher;
        private readonly SynchronousTaskEx _taskScheduler;
        private readonly GameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameSetupViewModelTests()
        {
            _gameFactory = Substitute.For<IGameFactory>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _userInteractor = Substitute.For<IUserInteractor>();
            _guiThreadDispatcher = new NoGuiThreadDispatcher();
            _taskScheduler = new SynchronousTaskEx();

            _gameSetupViewModelFactory = new GameSetupViewModelFactory(
                _gameFactory,
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator,
                _guiThreadDispatcher,
                _taskScheduler);

            _alternateGameSetup = Substitute.For<IAlternateGameSetup>();
        }

        [Fact]
        public void A_territory_request_gets_territory_from_user()
        {
            var territoryRequestParameter = Substitute.For<ITerritoryRequestParameter>();
            territoryRequestParameter.PlayerId.ReturnsForAnyArgs(Substitute.For<IPlayerId>());
            var expected = Substitute.For<ITerritoryId>();
            _userInteractor.WaitForTerritoryToBeSelected(territoryRequestParameter).Returns(expected);
            _worldMapViewModelFactory.Create(null, null, null).ReturnsForAnyArgs(new WorldMapViewModel());
            var sut = Initialize();

            var actual = sut.ProcessRequest(territoryRequestParameter);

            actual.Should().Be(expected);
        }

        [Fact]
        public void After_user_has_responded_with_a_territory_the_WorldMapViewModel_is_updated()
        {
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null, null, null).ReturnsForAnyArgs(worldMapViewModel);
            var gameSetupViewModel = Initialize();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.ProcessRequest(Substitute.For<ITerritoryRequestParameter>());

            gameSetupViewModel.WorldMapViewModel.Should().Be(worldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void When_finished_game_conductor_is_notified()
        {
            var sut = Initialize(activate: false);
            var expectedGame = Substitute.For<IGame>();
            IGame actualGame = null;
            var gamePlaySetup = Substitute.For<IGamePlaySetup>();
            _alternateGameSetup.Initialize(sut).Returns(gamePlaySetup);
            _gameFactory.Create(gamePlaySetup).Returns(expectedGame);
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThread(null)).Do(ci => actualGame = ci.Arg<StartGameplayMessage>().Game);

            sut.Activate();

            _eventAggregator.ReceivedWithAnyArgs().PublishOnUIThread(null);
            actualGame.Should().Be(expectedGame);
        }

        [Fact]
        public void Can_not_fortify()
        {
            var gameSetupViewModel = Initialize();

            gameSetupViewModel.CanFortify().Should().BeFalse();
        }

        [Fact]
        public void Can_not_end_turn()
        {
            var gameSetupViewModel = Initialize();

            gameSetupViewModel.CanEndTurn().Should().BeFalse();
        }

        [Fact]
        public void Confirm_sends_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(true);

            var gameSetupViewModel = Initialize();

            gameSetupViewModel.EndGame();

            _eventAggregator.Received().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public void Cancel_does_not_send_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(false);

            var gameSetupViewModel = Initialize();

            gameSetupViewModel.EndGame();

            _eventAggregator.DidNotReceive().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        private GameSetupViewModel Initialize(bool activate = true)
        {
            var gameSetupViewModel = (GameSetupViewModel)_gameSetupViewModelFactory.Create(_alternateGameSetup);
            if (activate)
            {
                gameSetupViewModel.Activate();
            }

            return gameSetupViewModel;
        }
    }
}