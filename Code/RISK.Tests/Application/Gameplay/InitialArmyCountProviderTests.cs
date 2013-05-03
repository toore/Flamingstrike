using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NSubstitute;
using RISK.Domain.GamePlaying.Setup;

namespace RISK.Tests.Application.Gameplay
{
    [TestFixture]
    public class InitialArmyCountProviderTests
    {
        private InitialArmyCountProvider _initialArmyCountProvider;

        [SetUp]
        public void SetUp()
        {
            _initialArmyCountProvider = new InitialArmyCountProvider();
        }

        [Test]
        [TestCase(40, 2, TestName = "2 players gets 40 armies each")]
        [TestCase(35, 3, TestName = "3 players gets 35 armies each")]
        [TestCase(30, 4, TestName = "4 players gets 30 armies each")]
        [TestCase(25, 5, TestName = "5 players gets 25 armies each")]
        [TestCase(20, 6, TestName = "6 players gets 20 armies each")]
        public void Number_of_players_gets_correct_number_of_armies(int expectedArmies, int numberOfPlayers)
        {
            _initialArmyCountProvider.Get(numberOfPlayers).Should().Be(expectedArmies);
        }
    }
}