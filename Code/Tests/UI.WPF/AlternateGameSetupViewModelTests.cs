using System;
using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.Core;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Xunit;
using Territory = FlamingStrike.UI.WPF.Services.GameEngineClient.SetupTerritorySelection.Territory;

namespace Tests.UI.WPF
{
    public class AlternateGameSetupViewModelTests
    {
        private readonly AlternateGameSetupViewModelFactory _factory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly FakeGameEngineClient _gameEngineClientProxy;

        public AlternateGameSetupViewModelTests()
        {
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _gameEngineClientProxy = new FakeGameEngineClient();

            _factory = new AlternateGameSetupViewModelFactory(
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator,
                _gameEngineClientProxy);
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
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.GetTerritories().Returns(new List<Territory>());
            placeArmyRegionSelector.Player.Returns("");

            Create();
            _gameEngineClientProxy.SelectRegion(placeArmyRegionSelector);

            _worldMapViewModelFactory.Received().Update(expectedWorldMapViewModel, Arg.Any<List<FlamingStrike.UI.WPF.ViewModels.Gameplay.Territory>>(), Maybe<Region>.Nothing);
        }

        [Fact]
        public void SelectRegion_updates_information_text()
        {
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.Player.Returns("");
            placeArmyRegionSelector.ArmiesLeftToPlace.Returns(1);

            var sut = Create();
            var monitor = sut.Monitor();
            _gameEngineClientProxy.SelectRegion(placeArmyRegionSelector);

            sut.InformationText.Should().Be(string.Format(Resources.PLACE_ARMY, 1));
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void SelectRegion_updates_player_name()
        {
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.Player.Returns("player name");
            _playerUiDataRepository.Get(null).ReturnsForAnyArgs(new PlayerUiData("", Colors.Black));

            var sut = Create();
            var monitor = sut.Monitor();
            _gameEngineClientProxy.SelectRegion(placeArmyRegionSelector);

            sut.PlayerName.Should().Be("player name");
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
        }

        [Fact]
        public void When_finished_setup_game_conductor_is_notified()
        {
            IGamePlaySetup expectedGamePlaySetup = null;
            var gamePlaySetup = Substitute.For<IGamePlaySetup>();
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThread(null)).Do(ci => expectedGamePlaySetup = ci.Arg<StartGameplayMessage>().GamePlaySetup);

            Create();
            _gameEngineClientProxy.NewGamePlaySetup(gamePlaySetup);

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
            var alternateGameSetupViewModel = (AlternateGameSetupViewModel)_factory.Create();
            alternateGameSetupViewModel.Activate();
            return alternateGameSetupViewModel;
        }

        private class FakeGameEngineClient : GameEngineClientProxyBase
        {
            public override void Setup(IEnumerable<string> players)
            {
                throw new InvalidOperationException();
            }

            public override void StartGame(IGamePlaySetup gamePlaySetup)
            {
                throw new InvalidOperationException();
            }

            public void SelectRegion(ITerritorySelector territorySelector)
            {
                _territorySelectorSubject.OnNext(territorySelector);
            }

            public void NewGamePlaySetup(IGamePlaySetup gamePlaySetup)
            {
                _gamePlaySetupSubject.OnNext(gamePlaySetup);
            }
        }
    }
}