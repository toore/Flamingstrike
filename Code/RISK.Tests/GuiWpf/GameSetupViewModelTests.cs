using System;
using System.Collections.Generic;
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
using RISK.Tests.Builders;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameSetupViewModelTests
    {
        private readonly IGameFactory _gameFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly SynchronousTaskEx _taskScheduler;
        private readonly GameSetupViewModelFactory _gameSetupViewModelFactory;
        private readonly IAlternateGameSetup _alternateGameSetup;

        public GameSetupViewModelTests()
        {
            _gameFactory = Substitute.For<IGameFactory>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _taskScheduler = new SynchronousTaskEx();

            _gameSetupViewModelFactory = new GameSetupViewModelFactory(
                _gameFactory,
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator,
                _taskScheduler);

            _alternateGameSetup = Substitute.For<IAlternateGameSetup>();
        }

        [Fact]
        public void UpdateView_updates_world_map_view_model()
        {
            var expectedWorldMapViewModel = new WorldMapViewModel();
            var territories = new List<ITerritory>();
            Action<IRegion> onClickAction = x => { };
            var enabledTerritories = new List<IRegion> { Make.TerritoryGeography.Build() };
            _worldMapViewModelFactory.Create(territories, onClickAction, enabledTerritories)
                .Returns(expectedWorldMapViewModel);
            var gameSetupViewModel = Initialize();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.UpdateView(
                territories: territories, 
                selectTerritoryAction: onClickAction, 
                enabledTerritories: enabledTerritories, 
                playerName: null, 
                armiesLeftToPlace: 0);

            gameSetupViewModel.WorldMapViewModel.Should().Be(expectedWorldMapViewModel);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
        }

        [Fact]
        public void UpdateView_updates_information_text()
        {
            var resourceManager = Substitute.For<IResourceManager>();
            ResourceManager.Instance = resourceManager;
            const string expectedInformationText = "information text shows armies left: 1";
            resourceManager.GetString("PLACE_ARMY").Returns("information text shows armies left: {0}");

            var gameSetupViewModel = Initialize();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.UpdateView(
                territories: null,
                selectTerritoryAction: null,
                enabledTerritories: null,
                playerName: null,
                armiesLeftToPlace: 1);

            gameSetupViewModel.InformationText.Should().Be(expectedInformationText);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void UpdateView_updates_player_name()
        {
            const string expectedPlayerName = "any player name";
            var gameSetupViewModel = Initialize();
            gameSetupViewModel.MonitorEvents();

            gameSetupViewModel.UpdateView(
                territories: null, 
                selectTerritoryAction: null, 
                enabledTerritories: null, 
                playerName: expectedPlayerName, 
                armiesLeftToPlace: 0);

            gameSetupViewModel.PlayerName.Should().Be(expectedPlayerName);
            gameSetupViewModel.ShouldRaisePropertyChangeFor(x => x.PlayerName);
        }

        [Fact]
        public void When_finished_game_conductor_is_notified()
        {
            var sut = Initialize(activate: false);
            var expectedGame = Substitute.For<IGame>();
            IGame actualGame = null;
            var gamePlaySetup = Substitute.For<IGamePlaySetup>();
            _alternateGameSetup.Initialize().Returns(gamePlaySetup);
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