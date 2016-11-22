﻿using System.Collections.Generic;
using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Tests.FlamingStrike.GameEngine.Builders;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF
{
    public class AlternateGameSetupViewModelTests
    {
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly AlternateGameSetupViewModelFactory _factory;

        public AlternateGameSetupViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();

            _factory = new AlternateGameSetupViewModelFactory(
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator);
        }

        [Fact]
        public void WorldMapViewModel_is_initialized()
        {
            var expectedWorldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null)
                .ReturnsForAnyArgs(expectedWorldMapViewModel);

            var sut = Create();

            sut.WorldMapViewModel.Should().Be(expectedWorldMapViewModel);
        }

        [Fact]
        public void SelectRegion_updates_world_map_view_model()
        {
            var expectedWorldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null)
                .ReturnsForAnyArgs(expectedWorldMapViewModel);
            var territories = new List<ITerritory>();
            var enabledRegions = new List<IRegion> { Make.Region.Build() };
            var placeArmyRegionSelector = Substitute.For<IPlaceArmyRegionSelector>();
            placeArmyRegionSelector.Territories.Returns(territories);
            placeArmyRegionSelector.SelectableRegions.Returns(enabledRegions);

            var sut = Create();
            sut.SelectRegion(placeArmyRegionSelector);

            _worldMapViewModelFactory.Received().Update(expectedWorldMapViewModel, territories, enabledRegions, Maybe<IRegion>.Nothing);
        }

        [Fact]
        public void SelectRegion_updates_information_text()
        {
            var placeArmyRegionSelector = Substitute.For<IPlaceArmyRegionSelector>();
            placeArmyRegionSelector.GetArmiesLeftToPlace().Returns(1);

            var sut = Create();
            sut.MonitorEvents();
            sut.SelectRegion(placeArmyRegionSelector);

            sut.InformationText.Should().Be(string.Format(Resources.PLACE_ARMY, 1));
            sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void SelectRegion_updates_player_name()
        {
            var placeArmyRegionSelector = Substitute.For<IPlaceArmyRegionSelector>();
            var expectedPlayer = Substitute.For<IPlayer>();
            expectedPlayer.Name.Returns("player name");
            placeArmyRegionSelector.Player.Returns(expectedPlayer);
            _playerUiDataRepository.Get(null).ReturnsForAnyArgs(Make.PlayerUiData.Build());

            var sut = Create();
            sut.MonitorEvents();
            sut.SelectRegion(placeArmyRegionSelector);

            sut.PlayerName.Should().Be("player name");
            sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
        }

        [Fact]
        public void When_finished_setup_game_conductor_is_notified()
        {
            IGamePlaySetup expectedGamePlaySetup = null;
            var gamePlaySetup = Substitute.For<IGamePlaySetup>();
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThread(null)).Do(ci => expectedGamePlaySetup = ci.Arg<StartGameplayMessage>().GamePlaySetup);

            var sut = Create();
            sut.NewGamePlaySetup(gamePlaySetup);

            _eventAggregator.ReceivedWithAnyArgs().PublishOnUIThread(null);
            expectedGamePlaySetup.Should().Be(gamePlaySetup);
        }

        [Fact]
        public void Can_not_enter_fortify_mode()
        {
            var sut = Create();

            sut.CanEnterFortifyMode.Should().BeFalse();
        }

        [Fact]
        public void Can_not_enter_attack_mode()
        {
            var sut = Create();

            sut.CanEnterAttackMode.Should().BeFalse();
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
            return (AlternateGameSetupViewModel)_factory.Create();
        }
    }
}