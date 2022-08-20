using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using FlamingStrike.UI.WPF;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Preparation;
using FluentAssertions;
using Xunit;

namespace Tests.UI.WPF
{
    public class GameplaySpec : SpecBase<GameplaySpec>
    {
        private MainGameViewModelDecorator _mainGameViewModel;
        private AlternateGameSetupViewModel _alternateGameSetupViewModel;

        [Fact]
        public async Task Game_is_setup_and_started()
        {
            await Given
                .a_new_game();

            (await When
                .two_human_players_are_confirmed())
                .all_armies_are_placed_on_the_map();

            Then
                .the_game_is_started();
        }

        private async Task a_new_game()
        {
            _mainGameViewModel = new MainGameViewModelDecorator(new CompositionRoot());

            await CaliburnMicroFrameworkActivation(_mainGameViewModel);
        }

        private static async Task CaliburnMicroFrameworkActivation(MainGameViewModelDecorator viewModel)
        {
            await viewModel.OnInitializeAsync(CancellationToken.None);
            await ((IActivate)viewModel).ActivateAsync();
        }

        private async Task<GameplaySpec> two_human_players_are_confirmed()
        {
            var gamePreparationViewModel = (IGamePreparationViewModel)_mainGameViewModel.ActiveItem;

            gamePreparationViewModel.PotentialPlayers.First().IsEnabled = true;
            gamePreparationViewModel.PotentialPlayers.ElementAt(1).IsEnabled = true;

            await gamePreparationViewModel.ConfirmAsync();

            _alternateGameSetupViewModel = (AlternateGameSetupViewModel)_mainGameViewModel.ActiveItem;

            return this;
        }

        private void all_armies_are_placed_on_the_map()
        {
            const int numberOfPlayers = 2;
            const int armiesToPlace = 19;

            for (var i = 0; i < numberOfPlayers * armiesToPlace; i++)
            {
                var regionViewModel = _alternateGameSetupViewModel.WorldMapViewModel.WorldMapViewModels
                    .OfType<RegionViewModel>()
                    .First(x => x.IsEnabled);
                regionViewModel.OnClick();
            }
        }

        private void the_game_is_started()
        {
            _mainGameViewModel.ActiveItem.Should().BeOfType<GameplayViewModel>();
        }
    }
}