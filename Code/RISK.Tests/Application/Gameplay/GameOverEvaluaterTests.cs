using FluentAssertions;
using GuiWpf.ViewModels.Gameplay;
using NSubstitute;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class GameOverEvaluaterTests
    {
        private readonly GameOverEvaluater _gameOverEvaluater;
        private readonly IWorldMap _worldMap;

        public GameOverEvaluaterTests()
        {
            _worldMap = Substitute.For<IWorldMap>();
            _gameOverEvaluater = new GameOverEvaluater();
        }

        [Fact]
        public void Is_game_over()
        {
            _worldMap.GetAllPlayersOccupyingTerritories()
                .Returns(new[] { Substitute.For<IPlayer>() });

            IsGameOver().Should().BeTrue("1 player occupies territories");
        }

        [Fact]
        public void Is_not_game_over()
        {
            _worldMap.GetAllPlayersOccupyingTerritories()
                .Returns(new[]
                {
                    Substitute.For<IPlayer>(),
                    Substitute.For<IPlayer>()
                });

            IsGameOver().Should().BeFalse("2 players occupies territories");
        }

        private bool IsGameOver()
        {
            return _gameOverEvaluater.IsGameOver(_worldMap);
        }
    }
}