using System;
using System.Collections.Generic;
using Caliburn.Micro;
using FluentAssertions;
using NSubstitute;
using RISK.Core;
using RISK.GameEngine.Setup;
using RISK.UI.WPF.Properties;
using RISK.UI.WPF.Services;
using RISK.UI.WPF.ViewModels.AlternateSetup;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Messages;
using Tests.Builders;
using Xunit;

namespace Tests.RISK.UI.WPF
{
    public class GameSetupViewModelTests
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly AlternateGameSetupViewModelFactory factory;

        public GameSetupViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();

            factory = new AlternateGameSetupViewModelFactory(
                _worldMapViewModelFactory,
                _dialogManager,
                _eventAggregator);
        }

        [Fact]
        public void UpdateView_updates_world_map_view_model()
        {
            var expectedWorldMapViewModel = new WorldMapViewModel();
            var territories = new List<ITerritory>();
            Action<IRegion> onClickAction = x => { };
            var enabledRegions = new List<IRegion> { Make.Region.Build() };
            _worldMapViewModelFactory.Create(onClickAction)
                .Returns(expectedWorldMapViewModel);
            _worldMapViewModelFactory.Update(expectedWorldMapViewModel, territories, enabledRegions, selectedRegion: null);
            var sut = Create();
            sut.MonitorEvents();

            //sut.UpdateView(
            //    territories: territories,
            //    selectAction: onClickAction,
            //    enabledRegions: enabledRegions,
            //    playerName: null,
            //    armiesLeftToPlace: 0);
            sut.SelectRegion(null);

            sut.WorldMapViewModel.Should().Be(expectedWorldMapViewModel);
            sut.ShouldRaisePropertyChangeFor(x => x.WorldMapViewModel);
        }

        [Fact]
        public void UpdateView_updates_information_text()
        {
            var sut = Create();
            sut.MonitorEvents();

            //sut.UpdateView(
            //    territories: null,
            //    selectAction: null,
            //    enabledRegions: null,
            //    playerName: null,
            //    armiesLeftToPlace: 1);
            sut.SelectRegion(null);

            sut.InformationText.Should().Be(string.Format(Resources.PLACE_ARMY, 1));
            sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void UpdateView_updates_player_name()
        {
            const string expectedPlayerName = "any player name";
            var sut = Create();
            sut.MonitorEvents();

            //sut.UpdateView(
            //    territories: null,
            //    selectAction: null,
            //    enabledRegions: null,
            //    playerName: expectedPlayerName,
            //    armiesLeftToPlace: 0);
            sut.SelectRegion(null);

            sut.PlayerName.Should().Be(expectedPlayerName);
            sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
        }

        [Fact]
        public void When_finished_setup_game_conductor_is_notified()
        {
            var sut = Create();
            IGamePlaySetup expectedGamePlaySetup = null;
            var gamePlaySetup = Substitute.For<IGamePlaySetup>();
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThread(null)).Do(ci => expectedGamePlaySetup = ci.Arg<StartGameplayMessage>().GamePlaySetup);

            _eventAggregator.ReceivedWithAnyArgs().PublishOnUIThread(null);
            expectedGamePlaySetup.Should().Be(gamePlaySetup);
        }

        [Fact]
        public void Can_not_activate_fortify()
        {
            var sut = Create();

            sut.CanEnterFortifyMode.Should().BeFalse();
        }

        [Fact]
        public void Can_not_end_turn()
        {
            var sut = Create();

            sut.CanEndTurn.Should().BeFalse();
        }

        [Fact]
        public void Confirm_sends_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(true);

            var sut = Create();
            sut.EndGame();

            _eventAggregator.Received().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public void Cancel_does_not_send_end_game_message()
        {
            _dialogManager.ConfirmEndGame().Returns(false);

            var sut = Create();
            sut.EndGame();

            _eventAggregator.DidNotReceive().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        private AlternateGameSetupViewModel Create()
        {
            return (AlternateGameSetupViewModel)factory.Create();
        }
    }
}