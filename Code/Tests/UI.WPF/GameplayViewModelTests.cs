﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.Properties;
using FlamingStrike.UI.WPF.Services;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
using FlamingStrike.UI.WPF.Services.GameEngineClient.Play;
using FlamingStrike.UI.WPF.Services.GameEngineClient.SetupFinished;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Gameplay.Interaction;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF
{
    public class GameplayViewModelTests
    {
        private readonly GameplayViewModel _sut;
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly WorldMapViewModel _worldMapViewModel = new();

        private readonly string _currentPlayerName;
        private readonly Color _currentPlayerColor;
        private readonly object[] _expectedCurrentPlayerStatusViewModels;
        private readonly Player[] _player;
        private readonly IDraftArmiesPhase _draftArmiesPhase;
        private readonly IAttackPhase _attackPhase;
        private readonly FakeGameEngineClient _gameEngineClientProxy;

        public GameplayViewModelTests()
        {
            var interactionStateFactory = new InteractionStateFactory();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            var playerStatusViewModelFactory = Substitute.For<IPlayerStatusViewModelFactory>();
            _gameEngineClientProxy = new FakeGameEngineClient();

            _worldMapViewModelFactory.Create(null).ReturnsForAnyArgs(_worldMapViewModel);

            _sut = new GameplayViewModel(
                interactionStateFactory,
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator,
                playerStatusViewModelFactory,
                _gameEngineClientProxy);

            ((IActivate)_sut).ActivateAsync();

            _currentPlayerName = "current player";
            _currentPlayerColor = Color.FromArgb(1, 2, 3, 4);
            _playerUiDataRepository.Get(_currentPlayerName).Returns(new PlayerUiData(_currentPlayerName, _currentPlayerColor));
            var firstPlayerGameData = new Player("player 1", 0);
            var secondPlayerGameData = new Player("player 2", 0);
            var thirdPlayerGameData = new Player("player 3", 0);
            var firstPlayerStatusViewModel = new PlayerStatusViewModelBuilder().Build();
            var secondPlayerStatusViewModel = new PlayerStatusViewModelBuilder().Build();
            var thirdPlayerStatusViewModel = new PlayerStatusViewModelBuilder().Build();
            playerStatusViewModelFactory.Create(firstPlayerGameData).Returns(firstPlayerStatusViewModel);
            playerStatusViewModelFactory.Create(secondPlayerGameData).Returns(secondPlayerStatusViewModel);
            playerStatusViewModelFactory.Create(thirdPlayerGameData).Returns(thirdPlayerStatusViewModel);
            _player = new[]
                {
                    firstPlayerGameData,
                    secondPlayerGameData,
                    thirdPlayerGameData
                };
            _expectedCurrentPlayerStatusViewModels = new object[] { firstPlayerStatusViewModel, secondPlayerStatusViewModel, thirdPlayerStatusViewModel };

            _draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();
            _draftArmiesPhase.CurrentPlayerName.Returns("");

            _attackPhase = Substitute.For<IAttackPhase>();
            _attackPhase.CurrentPlayerName.Returns("");
        }

        [Fact]
        public void World_map_is_created()
        {
            _worldMapViewModelFactory.Received(1).Create(Arg.Any<Action<Region>>());
            _sut.WorldMapViewModel.Should().Be(_worldMapViewModel);
        }

        [Fact]
        public void Draft_armies_shows_correct_view()
        {
            var draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();
            draftArmiesPhase.NumberOfArmiesToDraft.Returns(1);
            draftArmiesPhase.CurrentPlayerName.Returns(_currentPlayerName);
            draftArmiesPhase.Players.Returns(_player);

            _gameEngineClientProxy.DraftArmies(draftArmiesPhase);

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
                .ReturnsForAnyArgs(new PlayerUiData("", Colors.Black));

            var monitor = _sut.Monitor();
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void Attack_shows_correct_view()
        {
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayerName.Returns(_currentPlayerName);
            attackPhase.Players.Returns(_player);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);

            _gameEngineClientProxy.Attack(attackPhase);

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.SELECT_TERRITORY_TO_START_THE_ATTACK_FROM);
            _sut.CanEnterFortifyMode.Should().BeTrue();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Attack_raises_property_changed()
        {
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayerName.Returns(_currentPlayerName);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);

            var monitor = _sut.Monitor();
            _gameEngineClientProxy.Attack(attackPhase);

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEndTurn);
        }

        [Fact]
        public void Entering_fortify_mode_shows_correct_view()
        {
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayerName.Returns(_currentPlayerName);
            attackPhase.Players.Returns(_player);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(attackPhase);

            _sut.EnterFortifyMode();

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.SELECT_TERRITORY_TO_MOVE_FROM);
            _sut.CanEnterFortifyMode.Should().BeFalse();
            _sut.CanEnterAttackMode.Should().BeTrue();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Entering_fortify_mode_raises_property_changed()
        {
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(_attackPhase);

            var monitor = _sut.Monitor();
            _sut.EnterFortifyMode();

            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Entering_attack_mode_shows_correct_view()
        {
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayerName.Returns(_currentPlayerName);
            attackPhase.Players.Returns(_player);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(attackPhase);
            _sut.EnterFortifyMode();

            _sut.EnterAttackMode();

            _sut.PlayerName.Should().Be("current player");
            _sut.PlayerColor.Should().Be(_currentPlayerColor);
            _sut.PlayerStatuses.Should().BeEquivalentTo(_expectedCurrentPlayerStatusViewModels);
            _sut.InformationText.Should().Be(Resources.SELECT_TERRITORY_TO_START_THE_ATTACK_FROM);
            _sut.CanEnterFortifyMode.Should().BeTrue();
            _sut.CanEnterAttackMode.Should().BeFalse();
            _sut.CanEndTurn.Should().BeTrue();
        }

        [Fact]
        public void Entering_attack_mode_raises_property_changed()
        {
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(_attackPhase);
            _sut.EnterFortifyMode();

            var monitor = _sut.Monitor();
            _sut.EnterAttackMode();

            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Send_armies_to_occupy_shows_correct_view()
        {
            var sendArmiesToOccupyPhase = Substitute.For<ISendArmiesToOccupyPhase>();
            sendArmiesToOccupyPhase.CurrentPlayerName.Returns(_currentPlayerName);
            sendArmiesToOccupyPhase.Players.Returns(_player);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(_attackPhase);

            _gameEngineClientProxy.SendArmiesToOccupy(sendArmiesToOccupyPhase);

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
            var sendArmiesToOccupyPhase = Substitute.For<ISendArmiesToOccupyPhase>();
            sendArmiesToOccupyPhase.CurrentPlayerName.Returns(_currentPlayerName);
            sendArmiesToOccupyPhase.Players.Returns(_player);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(_attackPhase);

            var monitor = _sut.Monitor();
            _gameEngineClientProxy.SendArmiesToOccupy(sendArmiesToOccupyPhase);

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEndTurn);
        }

        [Fact]
        public void End_turn_shows_correct_view()
        {
            var endTurnPhase = Substitute.For<IEndTurnPhase>();
            endTurnPhase.CurrentPlayerName.Returns(_currentPlayerName);
            endTurnPhase.Players.Returns(_player);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);

            _gameEngineClientProxy.EndTurn(endTurnPhase);

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
            var endTurnPhase = Substitute.For<IEndTurnPhase>();
            endTurnPhase.CurrentPlayerName.Returns(_currentPlayerName);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);

            var monitor = _sut.Monitor();
            _gameEngineClientProxy.EndTurn(endTurnPhase);

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEndTurn);
        }

        [Fact]
        public void End_turn_raises_property_changed_when_in_attack_mode()
        {
            var endTurnPhase = Substitute.For<IEndTurnPhase>();
            endTurnPhase.CurrentPlayerName.Returns(_currentPlayerName);
            var draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();
            draftArmiesPhase.CurrentPlayerName.Returns("");
            _gameEngineClientProxy.DraftArmies(draftArmiesPhase);
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayerName.Returns("");
            _gameEngineClientProxy.Attack(attackPhase);

            var monitor = _sut.Monitor();
            _gameEngineClientProxy.EndTurn(endTurnPhase);

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterFortifyMode);
        }

        [Fact]
        public void End_turn_raises_property_changed_when_in_fortifying_mode()
        {
            var endTurnPhase = Substitute.For<IEndTurnPhase>();
            endTurnPhase.CurrentPlayerName.Returns(_currentPlayerName);
            _gameEngineClientProxy.DraftArmies(_draftArmiesPhase);
            _gameEngineClientProxy.Attack(_attackPhase);
            _sut.EnterFortifyMode();

            var monitor = _sut.Monitor();
            _gameEngineClientProxy.EndTurn(endTurnPhase);

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Game_over_displays_dialog()
        {
            var gameOverState = Substitute.For<IGameOverState>();
            gameOverState.Winner.Returns("winner");

            _gameEngineClientProxy.GameOver(gameOverState);

            _dialogManager.Received().ShowGameOverDialogAsync("winner");
            _eventAggregator.Received().PublishOnUIThreadAsync(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public async Task End_game_displays_confirm_dialog()
        {
            _dialogManager.ConfirmEndGameAsync().Returns(Task.FromResult<bool?>(false));

            await _sut.EndGameAsync();

            await _eventAggregator.DidNotReceiveWithAnyArgs().PublishOnUIThreadAsync(null);
        }

        [Fact]
        public async Task Game_ends_after_confirmation()
        {
            _dialogManager.ConfirmEndGameAsync().Returns(Task.FromResult<bool?>(true));

            await _sut.EndGameAsync();

            await _eventAggregator.Received().PublishOnUIThreadAsync(Arg.Any<NewGameMessage>());
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

            public void DraftArmies(IDraftArmiesPhase draftArmiesPhase)
            {
                _draftArmiesPhaseSubject.OnNext(draftArmiesPhase);
            }

            public void Attack(IAttackPhase attackPhase)
            {
                _attackPhaseSubject.OnNext(attackPhase);
            }

            public void SendArmiesToOccupy(ISendArmiesToOccupyPhase sendArmiesToOccupyPhase)
            {
                _sendArmiesToOccupyPhaseSubject.OnNext(sendArmiesToOccupyPhase);
            }

            public void EndTurn(IEndTurnPhase endTurnPhase)
            {
                _endTurnPhaseSubject.OnNext(endTurnPhase);
            }

            public void GameOver(IGameOverState gameOverState)
            {
                _gameOverStateSubject.OnNext(gameOverState);
            }
        }
    }
}