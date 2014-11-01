﻿using FluentAssertions;
using NSubstitute;
using RISK.Domain;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using RISK.Domain.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameTests
    {
        private Game _game;
        private IWorldMap _worldMap;
        private IInteractionStateFactory _interactionStateFactory;
        private IInteractionState _nextInteractionState;
        private IInteractionState _interactionStateAfterNextInteractionState;
        private IPlayers _players;
        private IPlayer _player1;
        private IPlayer _player2;
        private IAlternateGameSetup _alternateGameSetup;
        private IGameInitializerLocationSelector _gameInitializerLocationSelector;

        public GameTests()
        {
            _worldMap = Substitute.For<IWorldMap>();
            _interactionStateFactory = Substitute.For<IInteractionStateFactory>();
            _players = Substitute.For<IPlayers>();
            _alternateGameSetup = Substitute.For<IAlternateGameSetup>();
            _gameInitializerLocationSelector = Substitute.For<IGameInitializerLocationSelector>();

            _nextInteractionState = Substitute.For<IInteractionState>();
            _interactionStateAfterNextInteractionState = Substitute.For<IInteractionState>();
            _player1 = Substitute.For<IPlayer>();
            _player2 = Substitute.For<IPlayer>();
            _interactionStateFactory.CreateSelectState(_player1, _worldMap).Returns(_nextInteractionState);
            _interactionStateFactory.CreateSelectState(_player2, _worldMap).Returns(_interactionStateAfterNextInteractionState);

            _players.GetAll().Returns(new[] { _player1, _player2 });

            _alternateGameSetup.Initialize(_gameInitializerLocationSelector).Returns(_worldMap);

            _game = new Game(_interactionStateFactory, _players, _alternateGameSetup, _gameInitializerLocationSelector);
        }

        [Fact]
        public void Gets_world_map()
        {
            _game.GetWorldMap().Should().Be(_worldMap);
        }

        [Fact]
        public void Gets_next_turn()
        {
            var actual = _game.GetNextTurn();

            actual.Should().Be(_nextInteractionState);
        }

        [Fact]
        public void Gets_turn_after_next_turn()
        {
            _game.GetNextTurn();
            var actual = _game.GetNextTurn();

            actual.Should().Be(_interactionStateAfterNextInteractionState);
        }
    }
}