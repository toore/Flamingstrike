using System.Linq;
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
        public void Game_is_setup_and_started()
        {
            Given
                .a_new_game();

            When
                .two_human_players_are_confirmed()
                .all_armies_are_placed_on_the_map();

            Then
                .the_game_is_started();
        }

        private void a_new_game()
        {
            _mainGameViewModel = new MainGameViewModelDecorator(new CompositionRoot());

            CaliburnMicroFrameworkActivation(_mainGameViewModel);
        }

        private static void CaliburnMicroFrameworkActivation(MainGameViewModelDecorator viewModel)
        {
            viewModel.OnInitialize();
            ((IActivate)viewModel).Activate();
        }

        private GameplaySpec two_human_players_are_confirmed()
        {
            var gamePreparationViewModel = (IGamePreparationViewModel)_mainGameViewModel.ActiveItem;

            gamePreparationViewModel.PotentialPlayers.First().IsEnabled = true;
            gamePreparationViewModel.PotentialPlayers.ElementAt(1).IsEnabled = true;

            gamePreparationViewModel.Confirm();

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