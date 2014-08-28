using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay.Map;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Setup;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameSetupViewModelTests
    {
        private IWorldMapViewModelFactory _worldMapViewModelFactory;
        private IGameSettingStateConductor _gameSettingStateConductor;
        private IDialogManager _dialogManager;
        private IEventAggregator _eventAggregator;
        private IUserInteractor _userInteractor;
        private IGameFactoryWorker _gameFactoryWorker;
        private GameSetupViewModelFactory _gameSetupViewModelFactory;

        public GameSetupViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _gameSettingStateConductor = Substitute.For<IGameSettingStateConductor>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _userInteractor = Substitute.For<IUserInteractor>();
            _gameFactoryWorker = Substitute.For<IGameFactoryWorker>();

            _gameSetupViewModelFactory = new GameSetupViewModelFactory(_worldMapViewModelFactory, _dialogManager, _eventAggregator, _userInteractor, _gameFactoryWorker);
        }

        [Fact]
        public void Initialize_game_factory_worker()
        {
            var gameSetupViewModel = InitializeAndStartSetup();

            _gameFactoryWorker.Received().Run(gameSetupViewModel, gameSetupViewModel);
        }

        [Fact]
        public void Select_location_gets_location_from_user_interactor()
        {
            var locationSelectorParameter = Substitute.For<ILocationSelectorParameter>();
            locationSelectorParameter.GetPlayerThatTakesTurn().ReturnsForAnyArgs(Substitute.For<IPlayer>());
            var expected = Substitute.For<ILocation>();
            _userInteractor.GetLocation(locationSelectorParameter).Returns(expected);
            _worldMapViewModelFactory.Create(null, null).ReturnsForAnyArgs(new WorldMapViewModel());

            var gameSetupViewModel = InitializeAndStartSetup();
            var actual = gameSetupViewModel.SelectLocation(locationSelectorParameter);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Select_location_updates_view()
        {
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null, null).ReturnsForAnyArgs(worldMapViewModel);
            var gameSetupViewModel = InitializeAndStartSetup();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.SelectLocation(Substitute.For<ILocationSelectorParameter>());

            gameSetupViewModel.WorldMapViewModel.Should().Be(worldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.Player);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void When_finished_game_conductor_is_notified()
        {
            var game = Substitute.For<IGame>();

            var gameSetupViewModel = InitializeAndStartSetup();
            gameSetupViewModel.InitializationFinished(game);

            _gameSettingStateConductor.Received().StartGamePlay(game);
        }

        [Fact]
        public void Can_not_fortify()
        {
            var gameSetupViewModel = InitializeAndStartSetup();

            gameSetupViewModel.CanFortify().Should().BeFalse();
        }

        [Fact]
        public void Can_not_end_turn()
        {
            var gameSetupViewModel = InitializeAndStartSetup();

            gameSetupViewModel.CanEndTurn().Should().BeFalse();
        }

        [Fact]
        public void Confirm_sends_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(true);

            var gameSetupViewModel = InitializeAndStartSetup();

            gameSetupViewModel.EndGame();

            _eventAggregator.Received().Publish(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public void Cancel_does_not_send_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(false);

            var gameSetupViewModel = InitializeAndStartSetup();

            gameSetupViewModel.EndGame();

            _eventAggregator.DidNotReceive().Publish(Arg.Any<NewGameMessage>());
        }

        private GameSetupViewModel InitializeAndStartSetup()
        {
            var gameSetupViewModel = _gameSetupViewModelFactory.Create(_gameSettingStateConductor);
            gameSetupViewModel.StartSetup();

            return (GameSetupViewModel)gameSetupViewModel;
        }
    }
}