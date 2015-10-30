using System;
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
        private readonly GameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameSetupViewModelTests()
        {
            _gameFactory = Substitute.For<IGameFactory>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _userInteractor = Substitute.For<IUserInteractor>();
            var guiThreadDispatcher = new NoGuiThreadDispatcher();
            var taskScheduler = new SynchronousTaskEx();

            _gameSetupViewModelFactory = new GameSetupViewModelFactory(
                _gameFactory,
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator,
                _userInteractor,
                guiThreadDispatcher,
                taskScheduler);

            _alternateGameSetup = Substitute.For<IAlternateGameSetup>();
        }

        [Fact]
        public void Initialize_game_factory()
        {
            var sut = InitializeAndActivate();

            //_gameFactory.Received().Create(sut);
            throw new NotImplementedException();
        }

        [Fact]
        public void Select_location_gets_location_from_user_interactor()
        {
            var territoryRequestParameter = Substitute.For<ITerritoryRequestParameter>();
            territoryRequestParameter.PlayerId.ReturnsForAnyArgs(Substitute.For<IPlayerId>());
            var expected = Substitute.For<ITerritoryId>();
            _userInteractor.GetSelectedTerritory(territoryRequestParameter).Returns(expected);
            _worldMapViewModelFactory.Create(null, null, null).ReturnsForAnyArgs(new WorldMapViewModel());

            var gameSetupViewModel = InitializeAndActivate();
            var actual = gameSetupViewModel.ProcessRequest(territoryRequestParameter);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Select_location_updates_view()
        {
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null, null, null).ReturnsForAnyArgs(worldMapViewModel);
            var gameSetupViewModel = InitializeAndActivate();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.ProcessRequest(Substitute.For<ITerritoryRequestParameter>());

            gameSetupViewModel.WorldMapViewModel.Should().Be(worldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.PlayerId);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void When_finished_game_conductor_is_notified()
        {
            var expectedGame = Substitute.For<IGame>();
            IGame actualGame = null;
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThread(null)).Do(ci => actualGame = ci.Arg<StartGameplayMessage>().Game);

            InitializeAndActivate();

            _eventAggregator.ReceivedWithAnyArgs().PublishOnUIThread(new StartGameplayMessage(expectedGame));
            actualGame.Should().Be(expectedGame);
        }

        [Fact]
        public void Can_not_fortify()
        {
            var gameSetupViewModel = InitializeAndActivate();

            gameSetupViewModel.CanFortify().Should().BeFalse();
        }

        [Fact]
        public void Can_not_end_turn()
        {
            var gameSetupViewModel = InitializeAndActivate();

            gameSetupViewModel.CanEndTurn().Should().BeFalse();
        }

        [Fact]
        public void Confirm_sends_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(true);

            var gameSetupViewModel = InitializeAndActivate();

            gameSetupViewModel.EndGame();

            _eventAggregator.Received().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public void Cancel_does_not_send_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(false);

            var gameSetupViewModel = InitializeAndActivate();

            gameSetupViewModel.EndGame();

            _eventAggregator.DidNotReceive().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        private GameSetupViewModel InitializeAndActivate()
        {
            var gameSetupViewModel = _gameSetupViewModelFactory.Create(_alternateGameSetup);
            gameSetupViewModel.Activate();

            return (GameSetupViewModel)gameSetupViewModel;
        }
    }
}