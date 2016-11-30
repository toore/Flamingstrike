using System;
using System.Collections.Generic;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.GameEngine;
using FlamingStrike.GameEngine.Play;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Messages;
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

        private readonly Color _currentPlayerColor;
        private readonly object[] _expectedCurrentPlayerStatusViewModels;
        private readonly IGameStatus _currentGameStatus;

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

            var currentPlayer = Make.Player.Name("current player").Build();
            _currentPlayerColor = Color.FromArgb(1, 2, 3, 4);
            _playerUiDataRepository.Get(currentPlayer).Returns(Make.PlayerUiData.Color(_currentPlayerColor).Build());
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
            _expectedCurrentPlayerStatusViewModels = new object[] { firstPlayerStatusViewModel, secondPlayerStatusViewModel, thirdPlayerStatusViewModel };

            _currentGameStatus = Substitute.For<IGameStatus>();
            _currentGameStatus.CurrentPlayer.Returns(currentPlayer);
            _currentGameStatus.PlayerGameDatas.Returns(playerGameDatas);
        }

        [Fact]
        public void World_map_is_created()
        {
            _worldMapViewModelFactory.Received(1).Create(Arg.Any<Action<IRegion>>());
            _sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Draft_armies_shows_correct_view()
        {
            var draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();
            draftArmiesPhase.NumberOfArmiesToDraft.Returns(1);

            _sut.DraftArmies(_currentGameStatus, draftArmiesPhase);

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(string.Format(Resources.DRAFT_ARMIES, 1));
            _sut.CanEnterFortifyMode.Should().BeFalse();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeFalse();
        }

        [Fact]
        public void Drafting_armies_first_time_raises_property_changed()
        {
            _playerUiDataRepository.Get(null)
                .ReturnsForAnyArgs(Make.PlayerUiData.Color(Color.FromArgb(1, 2, 3, 4)).Build());

            _sut.MonitorEvents();
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());

            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerColor);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerStatuses);
            _sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void Attack_shows_correct_view()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());

            _sut.Attack(_currentGameStatus, Substitute.For<IAttackPhase>());

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.ATTACK_SELECT_FROM_TERRITORY);
            _sut.CanEnterFortifyMode.Should().BeTrue();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Attack_raises_property_changed()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());

            _sut.MonitorEvents();
            _sut.Attack(_currentGameStatus, Substitute.For<IAttackPhase>());

            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerColor);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerStatuses);
            _sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEndTurn);
        }

        [Fact]
        public void Entering_fortify_mode_shows_correct_view()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(_currentGameStatus, Substitute.For<IAttackPhase>());

            _sut.EnterFortifyMode();

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.FORTIFY_SELECT_TERRITORY_TO_MOVE_FROM);
            _sut.CanEnterFortifyMode.Should().BeFalse();
            _sut.CanEnterAttackMode.Should().BeTrue();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Entering_fortify_mode_raises_property_changed()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(_currentGameStatus, Substitute.For<IAttackPhase>());

            _sut.MonitorEvents();
            _sut.EnterFortifyMode();

            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Entering_attack_mode_shows_correct_view()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(_currentGameStatus, Substitute.For<IAttackPhase>());
            _sut.EnterFortifyMode();

            _sut.EnterAttackMode();

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.ATTACK_SELECT_FROM_TERRITORY);
            _sut.CanEnterFortifyMode.Should().BeTrue();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Entering_attack_mode_raises_property_changed()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(_currentGameStatus, Substitute.For<IAttackPhase>());
            _sut.EnterFortifyMode();

            _sut.MonitorEvents();
            _sut.EnterAttackMode();

            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Send_armies_to_occupy_shows_correct_view()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IGameStatus>(), Substitute.For<IAttackPhase>());

            _sut.SendArmiesToOccupy(_currentGameStatus, Substitute.For<ISendArmiesToOccupyPhase>());

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.SEND_ARMIES_TO_OCCUPY);
            _sut.CanEnterFortifyMode.Should().BeFalse();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeFalse();
        }

        [Fact]
        public void Send_armies_to_occupy_raises_property_changed()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IGameStatus>(), Substitute.For<IAttackPhase>());

            _sut.MonitorEvents();
            _sut.SendArmiesToOccupy(_currentGameStatus, Substitute.For<ISendArmiesToOccupyPhase>());

            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerColor);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerStatuses);
            _sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEndTurn);
        }

        [Fact]
        public void End_turn_shows_correct_view()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());

            _sut.EndTurn(_currentGameStatus, Substitute.For<IEndTurnPhase>());

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.END_TURN);
            _sut.CanEnterFortifyMode.Should().BeFalse();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void End_turn_raises_property_changed_after_drafting_armies()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());

            _sut.MonitorEvents();
            _sut.EndTurn(_currentGameStatus, Substitute.For<IEndTurnPhase>());

            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerColor);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerStatuses);
            _sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEndTurn);
        }

        [Fact]
        public void End_turn_raises_property_changed_when_in_attack_mode()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IGameStatus>(), Substitute.For<IAttackPhase>());

            _sut.MonitorEvents();
            _sut.EndTurn(_currentGameStatus, Substitute.For<IEndTurnPhase>());

            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerColor);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerStatuses);
            _sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterFortifyMode);
        }

        [Fact]
        public void End_turn_raises_property_changed_when_in_fortifying_mode()
        {
            _sut.DraftArmies(Substitute.For<IGameStatus>(), Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IGameStatus>(), Substitute.For<IAttackPhase>());
            _sut.EnterFortifyMode();

            _sut.MonitorEvents();
            _sut.EndTurn(_currentGameStatus, Substitute.For<IEndTurnPhase>());

            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerName);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerColor);
            _sut.ShouldRaisePropertyChangeFor(x => x.PlayerStatuses);
            _sut.ShouldRaisePropertyChangeFor(x => x.InformationText);
            _sut.ShouldRaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Game_over_displays_dialog()
        {
            var gameOverState = Substitute.For<IGameOverState>();
            gameOverState.Winner.Returns(Make.Player.Name("winner").Build());

            _sut.GameOver(gameOverState);

            _dialogManager.Received().ShowGameOverDialog("winner");
            _eventAggregator.Received().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }
    }
}