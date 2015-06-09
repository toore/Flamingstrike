using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Application.Entities;
using RISK.Application.GamePlaying;
using RISK.Application.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameSetupViewModelTests
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IUserInteractor _userInteractor;
        private readonly IGameFactory _gameFactory;
        private readonly GameSetupViewModelFactory _gameSetupViewModelFactory;

        public GameSetupViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _userInteractor = Substitute.For<IUserInteractor>();
            _gameFactory = Substitute.For<IGameFactory>();
            IGuiThreadDispatcher guiThreadDispatcher = new SameThreadDispatcher();
            var taskScheduler = new SynchronousTaskEx();

            _gameSetupViewModelFactory = new GameSetupViewModelFactory(_worldMapViewModelFactory, _dialogManager, _eventAggregator, _userInteractor, _gameFactory, guiThreadDispatcher, taskScheduler);
        }

        [Fact]
        public void Initialize_game_factory()
        {
            var sut = InitializeAndActivate();

            _gameFactory.Received().Create(sut);
        }

        [Fact]
        public void Select_location_gets_location_from_user_interactor()
        {
            var locationSelectorParameter = Substitute.For<ITerritorySelectorParameter>();
            locationSelectorParameter.GetPlayerThatTakesTurn().ReturnsForAnyArgs(Substitute.For<IPlayer>());
            var expected = Substitute.For<ITerritory>();
            _userInteractor.GetSelectedTerritory(locationSelectorParameter).Returns(expected);
            _worldMapViewModelFactory.Create(null, null, null).ReturnsForAnyArgs(new WorldMapViewModel());

            var gameSetupViewModel = InitializeAndActivate();
            var actual = gameSetupViewModel.SelectTerritory(locationSelectorParameter);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Select_location_updates_view()
        {
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null, null, null).ReturnsForAnyArgs(worldMapViewModel);
            var gameSetupViewModel = InitializeAndActivate();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.SelectTerritory(Substitute.For<ITerritorySelectorParameter>());

            gameSetupViewModel.WorldMapViewModel.Should().Be(worldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.Player);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void When_finished_game_conductor_is_notified()
        {
            var expectedGame = Substitute.For<IGameAdapter>();
            _gameFactory.Create(null).ReturnsForAnyArgs(expectedGame);
            IGameAdapter actualGameAdapter = null;
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThread(null)).Do(ci => actualGameAdapter = ci.Arg<StartGameplayMessage>().GameAdapter);

            InitializeAndActivate();

            _eventAggregator.ReceivedWithAnyArgs().PublishOnUIThread(new StartGameplayMessage(expectedGame));
            actualGameAdapter.Should().Be(expectedGame);
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
            var gameSetupViewModel = _gameSetupViewModelFactory.Create();
            gameSetupViewModel.Activate();

            return (GameSetupViewModel)gameSetupViewModel;
        }
    }
}