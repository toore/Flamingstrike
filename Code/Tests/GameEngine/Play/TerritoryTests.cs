using FluentAssertions;
using Xunit;

namespace Tests.GameEngine.Play
{
    public class TerritoryTests
    {
        [Theory]
        [InlineData(1, 0)]
        [InlineData(2, 1)]
        [InlineData(10, 9)]
        public void GetNumberOfArmiesThatCanFortifyAnotherTerritory(int armies, int armiesThatCanFortify)
        {
            new TerritoryBuilder().Armies(armies).Build()
                .GetNumberOfArmiesThatCanFortifyAnotherTerritory().Should().Be(armiesThatCanFortify);
        }
    }
}