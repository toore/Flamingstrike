using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using GuiWpf.ViewModels.Messages;
using GuiWpf.ViewModels.Settings;
using NSubstitute;
using RISK.Application;
using RISK.Application.Extensions;
using Xunit;

namespace RISK.Tests.GuiWpf
{
    public class GameSettingsViewModelTests
    {
        private readonly GameSettingsViewModel _gameSettingsViewModel;
        private readonly IEventAggregator _gameEventAggregator;

        public GameSettingsViewModelTests()
        {
            IPlayerFactory playerFactory = Substitute.For<IPlayerFactory>();
            IPlayerTypes playerTypes = Substitute.For<IPlayerTypes>();
            IPlayerRepository playerRepository = Substitute.For<IPlayerRepository>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            var playerType = Substitute.For<PlayerTypeBase>();
            playerTypes.Values.Returns(playerType.AsList());

            _gameSettingsViewModel = new GameSettingsViewModel(playerFactory, playerTypes, playerRepository, _gameEventAggregator);
        }

        [Fact]
        public void Has_6_players()
        {
            _gameSettingsViewModel.Players.Count.Should().Be(6);
        }

        [Fact]
        public void Cant_confirm_when_no_players_are_selected()
        {
            _gameSettingsViewModel.CanConfirm.Should().BeFalse();
        }

        [Fact]
        public void Cant_confirm_when_less_than_two_players_are_selected()
        {
            _gameSettingsViewModel.Players.First().IsEnabled = true;

            _gameSettingsViewModel.CanConfirm.Should().BeFalse();
        }

        [Fact]
        public void Can_confirm_when_at_least_two_players_are_selected()
        {
            _gameSettingsViewModel.Players.First().IsEnabled = true;
            _gameSettingsViewModel.Players.Second().IsEnabled = true;

            _gameSettingsViewModel.CanConfirm.Should().BeTrue();
        }

        [Fact]
        public void Confirm_publishes_message()
        {
            _gameSettingsViewModel.Confirm();

            _gameEventAggregator.Received().PublishOnCurrentThread(Arg.Any<GameSetupMessage>());
        }
    }
}