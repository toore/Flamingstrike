using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Tests.FlamingStrike.GameEngine.Builders;
using Xunit;

namespace Tests.FlamingStrike.UI.WPF
{
    public class GameplayViewModelTests
    {
        private readonly GameplayViewModel _sut;
        private readonly IInteractionStateFactory _interactionStateFactory;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly IPlayerStatusViewModelFactory _playerStatusViewModelFactory;
        private readonly WorldMapViewModel _worldMapViewModel = new WorldMapViewModel();

        public GameplayViewModelTests()
        {
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            _playerStatusViewModelFactory = Substitute.For<IPlayerStatusViewModelFactory>();

            _worldMapViewModelFactory.Create(null).ReturnsForAnyArgs(_worldMapViewModel);

            _sut = new GameplayViewModel(
                _interactionStateFactory,
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator,
                _playerStatusViewModelFactory);
        }

        [Fact]
        public void World_map_is_created()
        {
            _sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Draft_armies_shows_correct_view()
        {
            var draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();
            var currentPlayer = Make.Player.Name("current player").Build();
            var currentPlayerColor = Color.FromArgb(1, 2, 3, 4);
            _playerUiDataRepository.Get(currentPlayer).Returns(Make.PlayerUiData.Color(currentPlayerColor).Build());
            draftArmiesPhase.Player.Returns(currentPlayer);
            var firstPlayerGameData = new PlayerGameData(Make.Player.Name("player 1").Build(), new List<ICard>());
            var secondPlayerGameData = new PlayerGameData(Make.Player.Name("player 2").Build(), new List<ICard>());
            var thirdPlayerGameData = new PlayerGameData(Make.Player.Name("player 3").Build(), new List<ICard>());
            var firstPlayerStatusViewModel = Make.PlayerStatusViewModel.Build();
            var secondPlayerStatusViewModel = Make.PlayerStatusViewModel.Build();
            var thirdPlayerStatusViewModel = Make.PlayerStatusViewModel.Build();
            _playerStatusViewModelFactory.Create(firstPlayerGameData).Returns(firstPlayerStatusViewModel);
            _playerStatusViewModelFactory.Create(secondPlayerGameData).Returns(secondPlayerStatusViewModel);
            _playerStatusViewModelFactory.Create(thirdPlayerGameData).Returns(thirdPlayerStatusViewModel);
            var playerGameDatas = new IPlayerGameData[]
                {
                    firstPlayerGameData,
                    secondPlayerGameData,
                    thirdPlayerGameData
                };
            var playerStatusViewModels = new object[] { firstPlayerStatusViewModel, secondPlayerStatusViewModel, thirdPlayerStatusViewModel };
            draftArmiesPhase.PlayerGameDatas.Returns(playerGameDatas);

            _sut.DraftArmies(draftArmiesPhase);

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(currentPlayerColor);
            _sut.Players.Should().BeEquivalentTo(playerStatusViewModels);
        }
    }
}