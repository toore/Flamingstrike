using FluentAssertions;
using RISK.Application;
using RISK.Application.GameSetup;
using Xunit;

namespace RISK.Tests.Application.Gameplay
{
    public class InitialArmyForceTests
    {
        private readonly StartingInfantryCalculator _startingInfantryCalculator;

        public InitialArmyForceTests()
        {
            _startingInfantryCalculator = new StartingInfantryCalculator();
        }

        [Theory]
        [InlineData(40, 2)]
        [InlineData(35, 3)]
        [InlineData(30, 4)]
        [InlineData(25, 5)]
        [InlineData(20, 6)]
        public void Number_of_players_gets_correct_number_of_armies(int expectedArmies, int numberOfPlayers)
        {
            _startingInfantryCalculator.Get(numberOfPlayers).Should().Be(expectedArmies);
        }
    }
}