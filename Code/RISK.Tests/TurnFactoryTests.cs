using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using RISK.Domain.GamePlaying;
using Rhino.Mocks;
using Is = Rhino.Mocks.Constraints.Is;

namespace RISK.Tests
{
    [TestFixture]
    public class TurnFactoryTests
    {
        private TurnFactory _factory;
        private IWorldMap _worldMap;
        private IBattleEvaluater _battleEvaluater;

        [SetUp]
        public void SetUp()
        {
            _worldMap = MockRepository.GenerateStub<IWorldMap>();
            _battleEvaluater = MockRepository.GenerateStub<IBattleEvaluater>();

            _factory = new TurnFactory(_worldMap, _battleEvaluater);
        }

        [Test]
        public void Create_initializes_turn()
        {
            var turn = _factory.Create(null);

            turn.Should().NotBeNull();
        }
    }
}