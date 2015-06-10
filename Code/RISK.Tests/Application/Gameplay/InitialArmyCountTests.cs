using FluentAssertions;
using RISK.Application.GamePlaying.Setup;
using Xunit;
using Xunit.Extensions;

namespace RISK.Tests.Application.Gameplay
{
    public class InitialArmyCountTests
    {
        private readonly InitialArmyForce _initialArmyForce;

        public InitialArmyCountTests()
        {
            _initialArmyForce = new InitialArmyForce();
        }

        [Theory]
        [InlineData(40, 2)]
        [InlineData(35, 3)]
        [InlineData(30, 4)]
        [InlineData(25, 5)]
        [InlineData(20, 6)]
        public void Number_of_players_gets_correct_number_of_armies(int expectedArmies, int numberOfPlayers)
        {
            _initialArmyForce.Get(numberOfPlayers).Should().Be(expectedArmies);
        }
    }
}