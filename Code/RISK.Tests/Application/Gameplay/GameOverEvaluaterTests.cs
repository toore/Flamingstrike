using FluentAssertions;
using GuiWpf.ViewModels.Gameplay;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Entities;
using RISK.Domain.GamePlaying;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class GameOverEvaluaterTests
    {
        private GameOverEvaluater _gameOverEvaluater;
        private IWorldMap _worldMap;

        [SetUp]
        public void SetUp()
        {
            _worldMap = Substitute.For<IWorldMap>();
            _gameOverEvaluater = new GameOverEvaluater();
        }

        [Test]
        public void Is_game_over()
        {
            _worldMap.GetAllPlayersOccupyingTerritories()
                .Returns(new[] { Substitute.For<IPlayer>() });

            IsGameOver().Should().BeTrue("1 player occupies territories");
        }

        [Test]
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