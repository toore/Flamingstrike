using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using FlamingStrike.UI.WPF.ViewModels.Messages;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Tests.UI.WPF
{
    public class GamePreparationViewModelTests
    {
        private readonly GamePreparationViewModel _gameInitializationViewModel;
        private readonly IEventAggregator _gameEventAggregator;

        public GamePreparationViewModelTests()
        {
            var playerTypes = Substitute.For<IPlayerTypes>();
            var playerUiDataRepository = Substitute.For<IPlayerUiDataRepository>();
            _gameEventAggregator = Substitute.For<IEventAggregator>();

            var playerType = Substitute.For<PlayerTypeBase>();
            playerTypes.Values.Returns(new[] { playerType }.ToList());

            _gameInitializationViewModel = new GamePreparationViewModel(playerTypes, playerUiDataRepository, _gameEventAggregator);
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
        public async Task Confirm_publishes_message()
        {
            await _gameInitializationViewModel.ConfirmAsync();

            await _gameEventAggregator.Received().PublishOnUIThreadAsync(Arg.Any<StartGameSetupMessage>());
        }
    }
}