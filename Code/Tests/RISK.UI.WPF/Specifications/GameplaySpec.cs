using System.Linq;
using FluentAssertions;
using RISK.UI.WPF.ViewModels;
using RISK.UI.WPF.ViewModels.AlternateSetup;
using RISK.UI.WPF.ViewModels.Gameplay;
using RISK.UI.WPF.ViewModels.Preparation;
using Xunit;

namespace Tests.RISK.UI.WPF.Specifications
{
    public class GameplaySpec : SpecBase<GameplaySpec>
    {
        private MainGameViewModelDecorator _mainGameViewModel;
        private AlternateGameSetupViewModel _gameSetupViewModel;

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

        private GameplaySpec a_new_game()
        {
            _mainGameViewModel = new MainGameViewModelDecorator(new Root());

            OnInitializeIsCalledByCaliburnMicroFramework(_mainGameViewModel);

            return this;
        }

        private static void OnInitializeIsCalledByCaliburnMicroFramework(MainGameViewModelDecorator viewModel)
        {
            viewModel.OnInitialize();
        }

        private GameplaySpec two_human_players_are_confirmed()
        {
            var gameSettingsViewModel = (IGamePreparationViewModel)_mainGameViewModel.ActiveItem;

            gameSettingsViewModel.PotentialPlayers.First().IsEnabled = true;
            gameSettingsViewModel.PotentialPlayers.ElementAt(1).IsEnabled = true;

            gameSettingsViewModel.Confirm();

            _gameSetupViewModel = (AlternateGameSetupViewModel)_mainGameViewModel.ActiveItem;

            return this;
        }

        private GameplaySpec all_armies_are_placed_on_the_map()
        {
            const int numberOfArmiesForEachPlayer = 40;
            const int armiesAssignedToTerritoriesAutomatically = 21;
            const int numberOfPlayers = 2;
            const int numberOfArmiesToPlace = (numberOfArmiesForEachPlayer - armiesAssignedToTerritoriesAutomatically) * numberOfPlayers;

            for (var i = 0; i < numberOfArmiesToPlace; i++)
            {
                var regionViewModel = _gameSetupViewModel.WorldMapViewModel.WorldMapViewModels
                    .OfType<RegionViewModel>()
                    .First(x => x.IsEnabled);
                regionViewModel.OnClick();
            }

            return this;
        }

        private void the_game_is_started()
        {
            _mainGameViewModel.ActiveItem.Should().BeOfType<GameplayViewModel>();
        }
    }
}