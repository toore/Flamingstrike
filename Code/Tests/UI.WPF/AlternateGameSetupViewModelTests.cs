using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task WorldMapViewModel_is_initialized()
        {
            var expectedWorldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null)
                .ReturnsForAnyArgs(expectedWorldMapViewModel);

            var sut = await CreateAsync();

            sut.WorldMapViewModel.Should().Be(expectedWorldMapViewModel);
        }

        [Fact]
        public async Task SelectRegion_updates_world_map_view_model()
        {
            var expectedWorldMapViewModel = new WorldMapViewModel();
            _worldMapViewModelFactory.Create(null)
                .ReturnsForAnyArgs(expectedWorldMapViewModel);
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.Territories.Returns(new List<Territory>());
            placeArmyRegionSelector.Player.Returns("");

            await CreateAsync();
            _gameEngineClientProxy.SelectRegion(placeArmyRegionSelector);

            _worldMapViewModelFactory.Received().Update(expectedWorldMapViewModel, Arg.Any<List<FlamingStrike.UI.WPF.ViewModels.Gameplay.Territory>>(), Maybe<Region>.Nothing);
        }

        [Fact]
        public async Task SelectRegion_updates_information_text()
        {
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.Player.Returns("");
            placeArmyRegionSelector.ArmiesLeftToPlace.Returns(1);

            var sut = await CreateAsync();
            var monitor = sut.Monitor();
            _gameEngineClientProxy.SelectRegion(placeArmyRegionSelector);

            sut.InformationText.Should().Be(string.Format(Resources.PLACE_ARMY, 1));
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public async Task SelectRegion_updates_player_name()
        {
            var placeArmyRegionSelector = Substitute.For<ITerritorySelector>();
            placeArmyRegionSelector.Player.Returns("player name");
            _playerUiDataRepository.Get(null).ReturnsForAnyArgs(new PlayerUiData("", Colors.Black));

            var sut = await CreateAsync();
            var monitor = sut.Monitor();
            _gameEngineClientProxy.SelectRegion(placeArmyRegionSelector);

            sut.PlayerName.Should().Be("player name");
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
        }

        [Fact]
        public async Task When_finished_setup_game_conductor_is_notified()
        {
            IGamePlaySetup expectedGamePlaySetup = null;
            var gamePlaySetup = Substitute.For<IGamePlaySetup>();
            _eventAggregator.WhenForAnyArgs(x => x.PublishOnUIThreadAsync(null)).Do(ci => expectedGamePlaySetup = ci.Arg<StartGameplayMessage>().GamePlaySetup);

            await CreateAsync();
            _gameEngineClientProxy.NewGamePlaySetup(gamePlaySetup);

            await _eventAggregator.ReceivedWithAnyArgs().PublishOnUIThreadAsync(null);
            expectedGamePlaySetup.Should().Be(gamePlaySetup);
        }

        [Fact]
        public async Task Can_not_enter_fortify_mode()
        {
            var sut = await CreateAsync();

            sut.CanEnterFortifyMode.Should().BeFalse();
        }

        [Fact]
        public async Task Can_not_enter_attack_mode()
        {
            var sut = await CreateAsync();

            sut.CanEnterAttackMode.Should().BeFalse();
        }

        [Fact]
        public async Task Can_not_end_turn()
        {
            var sut = await CreateAsync();

            sut.CanEndTurn.Should().BeFalse();
        }

        [Fact]
        public async Task Confirm_sends_end_game_message()
        {
            _dialogManager.ConfirmEndGameAsync().Returns(true);

            var sut = await CreateAsync();
            await sut.EndGameAsync();

            await _eventAggregator.Received().PublishOnUIThreadAsync(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public async Task Cancel_does_not_send_end_game_message()
        {
            _dialogManager.ConfirmEndGameAsync().Returns(false);

            var sut = await CreateAsync();
            await sut.EndGameAsync();

            await _eventAggregator.DidNotReceive().PublishOnUIThreadAsync(Arg.Any<NewGameMessage>());
        }

        private async Task<AlternateGameSetupViewModel> CreateAsync()
        {
            var alternateGameSetupViewModel = (AlternateGameSetupViewModel)_factory.Create();
            await ((IActivate)alternateGameSetupViewModel).ActivateAsync();
            return alternateGameSetupViewModel;
        }

        private class FakeGameEngineClient : GameEngineClientBase
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