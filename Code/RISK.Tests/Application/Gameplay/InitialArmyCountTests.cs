using FluentAssertions;
using RISK.Application.GamePlaying.Setup;
using Xunit;
using Xunit.Extensions;

namespace RISK.Tests.Application.Gameplay
{
    public class InitialArmyCountTests
    {
        private readonly InitialArmyCount _initialArmyCount;

        public InitialArmyCountTests()
        {
            _initialArmyCount = new InitialArmyCount();
        }

        [Theory]
        [InlineData(40, 2)]
        [InlineData(35, 3)]
        [InlineData(30, 4)]
        [InlineData(25, 5)]
        [InlineData(20, 6)]
        public void Number_of_players_gets_correct_number_of_armies(int expectedArmies, int numberOfPlayers)
        {
            _initialArmyCount.Get(numberOfPlayers).Should().Be(expectedArmies);
        }
    }
}