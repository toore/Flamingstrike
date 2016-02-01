using System;
using FluentAssertions;
using FluentAssertions.Execution;
using NSubstitute;
using RISK.Application;
using RISK.Application.Play;
using Xunit;

namespace RISK.Tests.Application.Extensions
{
    public class GameTestsExtensionsTests
    {
        private readonly IGame _sut;
        private readonly ITerritory _territory;
        private readonly ITerritory _anotherTerritory;
        private const int _numberOfArmies = 1;

        public GameTestsExtensionsTests()
        {
            _territory = Substitute.For<ITerritory>();
            _anotherTerritory = Substitute.For<ITerritory>();

            _sut = Substitute.For<IGame>();
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_CanAttack_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotAttack(_territory, _anotherTerritory),
                x => x.CanAttack(_territory, _anotherTerritory),
                x => x.Attack(_territory, _anotherTerritory));
        }

        [Fact]
        public void AssertCanNotAttack_asserts_that_Attack_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotAttack(_territory, _anotherTerritory));
        }

        [Fact]
        public void AssertCanNotMoveArmiesIntoCapturedTerritory_asserts_that_CanMoveArmiesIntoCapturedTerritory_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotMoveArmiesIntoCapturedTerritory(_numberOfArmies),
                x => x.MustSendInArmiesToOccupyTerritory(),
                x => x.SendInArmiesToOccupyTerritory(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotMoveArmiesIntoCapturedTerritory_asserts_that_MoveArmiesIntoCapturedTerritory_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotMoveArmiesIntoCapturedTerritory(_numberOfArmies));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_CanFortify_is_false()
        {
            AssertMethodThrowsAssertionFailedExceptionWhenIsEnabled(
                x => x.AssertCanNotFortify(_territory, _anotherTerritory),
                x => x.CanFortify(_territory, _anotherTerritory),
                x => x.Fortify(_territory, _anotherTerritory));
        }

        [Fact]
        public void AssertCanNotFortify_asserts_that_Fortify_throws()
        {
            AssertMethodThrowsAssertionFailedException(
                x => x.AssertCanNotFortify(_territory, _anotherTerritory));
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