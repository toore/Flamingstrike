using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using NSubstitute;
using NUnit.Framework;
using RISK.Domain.Extensions;

namespace RISK.Tests.GuiWpf
{
    [TestFixture]
    public class GameSettingsViewModelTests
    {
        private GameSettingsViewModel _gameSettingsViewModel;
        private IPlayerFactory _playerFactory;
        private IPlayerTypes _playerTypes;
        private IEventAggregator _gameEventAggregator;

        [SetUp]
        public void SetUp()
        {
            _playerFactory = Substitute.For<IPlayerFactory>();
            _playerTypes = Substitute.For<IPlayerTypes>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            var playerType = Substitute.For<PlayerTypeBase>();
            _playerTypes.Values.Returns(playerType.AsList());

            _gameSettingsViewModel = new GameSettingsViewModel(_playerFactory, _playerTypes, _gameEventAggregator);
        }

        [Test]
        public void Has_6_players()
        {
            _gameSettingsViewModel.Players.Count.Should().Be(6);
        }

        [Test]
        public void Cant_confirm_when_no_players_are_selected()
        {
            _gameSettingsViewModel.CanConfirm.Should().BeFalse();
        }

        [Test]
        public void Cant_confirm_when_less_than_two_players_are_selected()
        {
            _gameSettingsViewModel.Players.First().IsEnabled = true;

            _gameSettingsViewModel.CanConfirm.Should().BeFalse();
        }

        [Test]
        public void Can_confirm_when_at_least_two_players_are_selected()
        {
            _gameSettingsViewModel.Players.First().IsEnabled = true;
            _gameSettingsViewModel.Players.Second().IsEnabled = true;

            _gameSettingsViewModel.CanConfirm.Should().BeTrue();
        }

        [Test]
        public void Confirm_publishes_message()
        {
            _gameSettingsViewModel.Confirm();

            _gameEventAggregator.Received().Publish(Arg.Any<GameSetupMessage>());
        }
    }
}