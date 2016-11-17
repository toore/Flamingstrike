using System.Linq;
using Caliburn.Micro;
using FluentAssertions;
using NSubstitute;
using RISK.UI.WPF.ViewModels.Messages;
using RISK.UI.WPF.ViewModels.Preparation;
using Xunit;

namespace Tests.RISK.UI.WPF
{
    public class GamePreparationViewModelTests
    {
        private readonly GamePreparationViewModel _gameInitializationViewModel;
        private readonly IEventAggregator _gameEventAggregator;

        public GamePreparationViewModelTests()
        {
            var playerFactory = Substitute.For<IPlayerFactory>();
            var playerTypes = Substitute.For<IPlayerTypes>();
            var playerRepository = Substitute.For<IPlayerUiDataRepository>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            var playerType = Substitute.For<PlayerTypeBase>();
            playerTypes.Values.Returns(new[] { playerType }.ToList());

            _gameInitializationViewModel = new GamePreparationViewModel(playerFactory, playerTypes, playerRepository, _gameEventAggregator);
        }

        [Fact]
        public void Has_6_potential_players()
        {
            _gameInitializationViewModel.PotentialPlayers.Count.Should().Be(6);
        }

        [Fact]
        public void Cant_confirm_when_no_players_are_selected()
        {
            _gameInitializationViewModel.CanConfirm.Should().BeFalse();
        }

        [Fact]
        public void Cant_confirm_when_less_than_two_players_are_selected()
        {
            _gameInitializationViewModel.PotentialPlayers.First().IsEnabled = true;

            _gameInitializationViewModel.CanConfirm.Should().BeFalse();
        }

        [Fact]
        public void Can_confirm_when_at_least_two_players_are_selected()
        {
            _gameInitializationViewModel.PotentialPlayers.First().IsEnabled = true;
            _gameInitializationViewModel.PotentialPlayers.ElementAt(1).IsEnabled = true;

            _gameInitializationViewModel.CanConfirm.Should().BeTrue();
        }

        [Fact]
        public void Confirm_publishes_message()
        {
            _gameInitializationViewModel.Confirm();

            _gameEventAggregator.Received().PublishOnUIThread(Arg.Any<StartGameSetupMessage>());
        }
    }
}