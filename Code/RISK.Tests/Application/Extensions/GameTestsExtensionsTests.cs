using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using RISK.Application.Play;
using RISK.Application.World;
using Xunit;

namespace RISK.Tests.Application.Extensions
{
    public class GameTestsExtensionsTests
    {
        private readonly IGame _sut;
        private readonly IRegion _region;
        private readonly IRegion _anotherRegion;
        private const int _numberOfArmies = 1;

        public GameTestsExtensionsTests()
        {
            _region = Substitute.For<IRegion>();
            _anotherRegion = Substitute.For<IRegion>();

            _sut = Substitute.For<IGame>();
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_CanAttack_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotAttack(_region, _anotherRegion),
                x => x.CanAttack(_region, _anotherRegion),
                x => x.Attack(_region, _anotherRegion));
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_Attack_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotAttack(_region, _anotherRegion));
        }

        [Fact]
        public void AssertCanNotSendInArmiesToOccupy_asserts_that_CanSendInArmiesToOccupy_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotSendInArmiesToOccupy(_numberOfArmies),
                x => x.CanSendArmiesToOccupy(),
                x => x.SendArmiesToOccupy(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotSendInArmiesToOccupy_asserts_that_SendInArmiesToOccupy_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotSendInArmiesToOccupy(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_CanFortify_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotFortify(_region, _anotherRegion),
                x => x.CanFortify(_region, _anotherRegion),
                x => x.Fortify(_region, _anotherRegion));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_Fortify_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotFortify(_region, _anotherRegion));
        }

        private void AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
            Action<IGame> assertAction,
            Func<IGame, bool> isEnabled,
            Action<IGame> action)
        {
            isEnabled.Invoke(_sut).Returns(true);
            _sut.When(action).Throw<InvalidOperationException>();

            AssertMethodThrowsAssertionFailedException(assertAction);
        }

        private void AssertMethodThrowsAssertionFailedException(Action<IGame> assertAction)
        {
            Action act = () => assertAction.Invoke(_sut);
            act.ShouldThrow<AssertionFailedException>();
        }
    }
}