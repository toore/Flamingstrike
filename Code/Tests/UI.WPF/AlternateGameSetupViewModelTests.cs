using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.Core;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Setup.Finished;
using FlamingStrike.GameEngine.Setup.TerritorySelection;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Player = FlamingStrike.GameEngine.Setup.TerritorySelection.Player;
using Territory = FlamingStrike.GameEngine.Setup.TerritorySelection.Territory;

namespace Tests.UI.WPF
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
            var territories = new List<Territory>();
            //var enabledRegions = new List<IRegion> { new RegionBuilder().Build() };
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.GetTerritories().Returns(territories);
            //placeArmyRegionSelector.SelectableRegions.Returns(enabledRegions);

            var sut = Create();
            sut.SelectRegion(placeArmyRegionSelector);

            _worldMapViewModelFactory.Received().Update(expectedWorldMapViewModel, /*territories*/null, Maybe<IRegion>.Nothing);
        }

        [Fact]
        public void SelectRegion_updates_information_text()
        {
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.GetPlayer().Returns(new Player("", 1));
            placeArmyRegionSelector.GetArmiesLeftToPlace().Returns(1);

            var sut = Create();
            var monitor = sut.Monitor();
            sut.SelectRegion(placeArmyRegionSelector);

            sut.InformationText.Should().Be(string.Format(Resources.PLACE_ARMY, 1));
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void SelectRegion_updates_player_name()
        {
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.GetPlayer().Returns(new Player("player name", 0));
            _playerUiDataRepository.Get(null).ReturnsForAnyArgs(new PlayerUiData("", Colors.Black));

            var sut = Create();
            var monitor = sut.Monitor();
            sut.SelectRegion(placeArmyRegionSelector);

            sut.PlayerName.Should().Be("player name");
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
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