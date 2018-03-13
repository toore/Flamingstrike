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
        private readonly IWorldMapViewModelFactory _worldMapViewModelFactory;
        private readonly IPlayerUiDataRepository _playerUiDataRepository;
        private readonly IDialogManager _dialogManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly WorldMapViewModel _worldMapViewModel = new WorldMapViewModel();

        private readonly IPlayer _currentPlayer;
        private readonly Color _currentPlayerColor;
        private readonly object[] _expectedCurrentPlayerStatusViewModels;
        private IPlayerGameData[] _playerGameDatas;

        public GameplayViewModelTests()
        {
            var interactionStateFactory = new InteractionStateFactory();
            _worldMapViewModelFactory = Substitute.For<IWorldMapViewModelFactory>();
            _playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _dialogManager = Substitute.For<IDialogManager>();
            _eventAggregator = Substitute.For<IEventAggregator>();
            var playerStatusViewModelFactory = Substitute.For<IPlayerStatusViewModelFactory>();

            _worldMapViewModelFactory.Create(null).ReturnsForAnyArgs(_worldMapViewModel);

            _sut = new GameplayViewModel(
                interactionStateFactory,
                _worldMapViewModelFactory,
                _playerUiDataRepository,
                _dialogManager,
                _eventAggregator,
                playerStatusViewModelFactory);

            _currentPlayer = new PlayerBuilder().Name("current player").Build();
            _currentPlayerColor = Color.FromArgb(1, 2, 3, 4);
            _playerUiDataRepository.Get(_currentPlayer).Returns(new PlayerUiDataBuilder().Color(_currentPlayerColor).Build());
            var firstPlayerGameData = new PlayerGameData(new PlayerBuilder().Name("player 1").Build(), new List<ICard>());
            var secondPlayerGameData = new PlayerGameData(new PlayerBuilder().Name("player 2").Build(), new List<ICard>());
            var thirdPlayerGameData = new PlayerGameData(new PlayerBuilder().Name("player 3").Build(), new List<ICard>());
            var firstPlayerStatusViewModel = new PlayerStatusViewModelBuilder().Build();
            var secondPlayerStatusViewModel = new PlayerStatusViewModelBuilder().Build();
            var thirdPlayerStatusViewModel = new PlayerStatusViewModelBuilder().Build();
            playerStatusViewModelFactory.Create(firstPlayerGameData).Returns(firstPlayerStatusViewModel);
            playerStatusViewModelFactory.Create(secondPlayerGameData).Returns(secondPlayerStatusViewModel);
            playerStatusViewModelFactory.Create(thirdPlayerGameData).Returns(thirdPlayerStatusViewModel);
            _playerGameDatas = new IPlayerGameData[]
                {
                    firstPlayerGameData,
                    secondPlayerGameData,
                    thirdPlayerGameData
                };
            _expectedCurrentPlayerStatusViewModels = new object[] { firstPlayerStatusViewModel, secondPlayerStatusViewModel, thirdPlayerStatusViewModel };
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
            draftArmiesPhase.CurrentPlayer.Returns(_currentPlayer);
            draftArmiesPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(draftArmiesPhase);

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
                .ReturnsForAnyArgs(new PlayerUiDataBuilder().Color(Color.FromArgb(1, 2, 3, 4)).Build());

            var monitor = _sut.Monitor();
            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());

            monitor.Should().RaisePropertyChangeFor(x => x.PlayerName);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerColor);
            monitor.Should().RaisePropertyChangeFor(x => x.PlayerStatuses);
            monitor.Should().RaisePropertyChangeFor(x => x.InformationText);
        }

        [Fact]
        public void Attack_shows_correct_view()
        {
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayer.Returns(_currentPlayer);
            attackPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(attackPhase);

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
            var draftArmiesPhase = Substitute.For<IDraftArmiesPhase>();
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayer.Returns(_currentPlayer);

            _sut.DraftArmies(draftArmiesPhase);

            var monitor = _sut.Monitor();
            _sut.Attack(attackPhase);

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
            attackPhase.CurrentPlayer.Returns(_currentPlayer);
            attackPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(attackPhase);
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
            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IAttackPhase>());

            var monitor = _sut.Monitor();
            _sut.EnterFortifyMode();

            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterFortifyMode);
            monitor.Should().RaisePropertyChangeFor(x => x.CanEnterAttackMode);
        }

        [Fact]
        public void Entering_attack_mode_shows_correct_view()
        {
            var attackPhase = Substitute.For<IAttackPhase>();
            attackPhase.CurrentPlayer.Returns(_currentPlayer);
            attackPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(attackPhase);
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
            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IAttackPhase>());
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
            sendArmiesToOccupyPhase.CurrentPlayer.Returns(_currentPlayer);
            sendArmiesToOccupyPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IAttackPhase>());
            _sut.SendArmiesToOccupy(sendArmiesToOccupyPhase);

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
            sendArmiesToOccupyPhase.CurrentPlayer.Returns(_currentPlayer);
            sendArmiesToOccupyPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IAttackPhase>());
            var monitor = _sut.Monitor();
            _sut.SendArmiesToOccupy(sendArmiesToOccupyPhase);

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
            endTurnPhase.CurrentPlayer.Returns(_currentPlayer);
            endTurnPhase.PlayerGameDatas.Returns(_playerGameDatas);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.EndTurn(endTurnPhase);

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
            endTurnPhase.CurrentPlayer.Returns(_currentPlayer);
            
            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            var monitor = _sut.Monitor();
            _sut.EndTurn(endTurnPhase);

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
            endTurnPhase.CurrentPlayer.Returns(_currentPlayer);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IAttackPhase>());
            var monitor = _sut.Monitor();
            _sut.EndTurn(endTurnPhase);

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
            endTurnPhase.CurrentPlayer.Returns(_currentPlayer);

            _sut.DraftArmies(Substitute.For<IDraftArmiesPhase>());
            _sut.Attack(Substitute.For<IAttackPhase>());
            _sut.EnterFortifyMode();
            var monitor = _sut.Monitor();
            _sut.EndTurn(endTurnPhase);

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
            gameOverState.Winner.Returns(new PlayerBuilder().Name("winner").Build());

            _sut.GameOver(gameOverState);

            _dialogManager.Received().ShowGameOverDialog("winner");
            _eventAggregator.Received().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }

        [Fact]
        public void End_game_displays_confirm_dialog()
        {
            _dialogManager.ConfirmEndGame().Returns(false);

            _sut.EndGame();

            _eventAggregator.DidNotReceiveWithAnyArgs().PublishOnUIThread(null);
        }

        [Fact]
        public void Game_ends_after_confirmation()
        {
            _dialogManager.ConfirmEndGame().Returns(true);

            _sut.EndGame();

            _eventAggregator.Received().PublishOnUIThread(Arg.Any<NewGameMessage>());
        }
    }
}