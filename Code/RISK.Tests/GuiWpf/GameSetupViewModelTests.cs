﻿using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.Services;
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
        private readonly IGameFactoryWorker _gameFactoryWorker;
        private readonly GameSetupViewModelFactory _gameSetupViewModelFactory;

        public GameSetupViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
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
            var locationSelectorParameter = Substitute.For<ITerritorySelectorParameter>();
            locationSelectorParameter.GetPlayerThatTakesTurn().ReturnsForAnyArgs(Substitute.For<IPlayer>());
            var expected = Substitute.For<ITerritory>();
            _userInteractor.GetLocation(locationSelectorParameter).Returns(expected);
            _worldMapViewModelFactory.Create(null, null).ReturnsForAnyArgs(new WorldMapViewModel());

            var gameSetupViewModel = InitializeAndStartSetup();
            var actual = gameSetupViewModel.SelectTerritory(locationSelectorParameter);

            actual.Should().Be(expected);
        }

        [Fact]
        public void Select_location_updates_view()
        {
            var worldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null, null).ReturnsForAnyArgs(worldMapViewModel);
            var gameSetupViewModel = InitializeAndStartSetup();
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
            var expectedGame = Substitute.For<IGame>();
            IGame actualGame = null;
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnCurrentThread(null)).Do(ci => actualGame = ci.Arg<StartGameplayMessage>().Game);

            var gameSetupViewModel = InitializeAndStartSetup();
            gameSetupViewModel.InitializationFinished(expectedGame);

            _eventAggregator.ReceivedWithAnyArgs().PublishOnCurrentThread(new StartGameplayMessage(expectedGame));
            actualGame.Should().Be(expectedGame);
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

            _eventAggregator.Received().PublishOnCurrentThread(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public void Cancel_does_not_send_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(false);

            var gameSetupViewModel = InitializeAndStartSetup();

            gameSetupViewModel.EndGame();

            _eventAggregator.DidNotReceive().PublishOnCurrentThread(Arg.Any<NewGameMessage>());
        }

        private GameSetupViewModel InitializeAndStartSetup()
        {
            var gameSetupViewModel = _gameSetupViewModelFactory.Create();
            gameSetupViewModel.Activate();


            return (GameSetupViewModel)gameSetupViewModel;
        }
    }
}