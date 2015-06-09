using FluentAssertions;
using NSubstitute;
using RISK.Application.GamePlaying.DiceAndCalculation;
using Toore.Shuffling;
using Xunit;

namespace RISK.Tests.Application.Battle
{
    public class DiceTests
    {
        private readonly Dice _sut;
        private readonly IRandomWrapper _randomWrapper;

        public DiceTests()
        {
            _randomWrapper = Substitute.For<IRandomWrapper>();

            _sut = new Dice(_randomWrapper);
        }

        [Fact]
        public void Six_face_dice_is_rolled()
        {
            _sut.Roll();

            _randomWrapper.Received().Next(1, 7);
        }
    }
}