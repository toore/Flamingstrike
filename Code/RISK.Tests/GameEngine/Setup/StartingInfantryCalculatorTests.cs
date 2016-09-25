using FluentAssertions;
using RISK.GameEngine.Setup;
using Xunit;

namespace RISK.Tests.GameEngine.Setup
{
    public class StartingInfantryCalculatorTests
    {
        private readonly StartingInfantryCalculator _sut;

        public StartingInfantryCalculatorTests()
        {
            _sut = new StartingInfantryCalculator();
        }

        [Theory]
        [InlineData(40, 2)]
        [InlineData(35, 3)]
        [InlineData(30, 4)]
        [InlineData(25, 5)]
        [InlineData(20, 6)]
        public void Number_of_players_gets_correct_number_of_armies(int expectedArmies, int numberOfPlayers)
        {
            _sut.Get(numberOfPlayers).Should().Be(expectedArmies);
        }
    }
}