using System;
using System.Linq;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Setup;
using NUnit.Framework;
using RISK.Domain.Extensions;
using Rhino.Mocks;

namespace RISK.Tests.Setup
{
    [TestFixture]
    public class GameSetupViewModelTests
    {
        private GameSetupViewModel _gameSetupViewModel;
        private IPlayerFactory _playerFactory;
        private IPlayerTypes _playerTypes;
        private Action<GameSetup> _confirm;

        [SetUp]
        public void SetUp()
        {
            _playerFactory = MockRepository.GenerateStub<IPlayerFactory>();
            _playerTypes = MockRepository.GenerateStub<IPlayerTypes>();
            _confirm = MockRepository.GenerateStub<Action<GameSetup>>();

            var playerType = MockRepository.GenerateStub<PlayerTypeBase>();
            _playerTypes.Stub(x => x.Values).Return(playerType.AsList().ToList());

            _gameSetupViewModel = new GameSetupViewModel(_playerFactory, _playerTypes, _confirm);
        }

        [Test]
        public void Has_6_players()
        {
            _gameSetupViewModel.Players.Count.Should().Be(6);
        }

        [Test]
        public void Cant_confirm_when_no_players_are_selected()
        {
            _gameSetupViewModel.CanConfirm.Should().BeFalse();
        }

        [Test]
        public void Cant_confirm_when_less_than_two_players_are_selected()
        {
            _gameSetupViewModel.Players.First().IsEnabled = true;

            _gameSetupViewModel.CanConfirm.Should().BeFalse();
        }

        [Test]
        public void Can_confirm_when_at_least_two_players_are_selected()
        {
            _gameSetupViewModel.Players.First().IsEnabled = true;
            _gameSetupViewModel.Players.Second().IsEnabled = true;

            _gameSetupViewModel.CanConfirm.Should().BeTrue();
        }
    }
}