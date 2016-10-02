using System.Linq;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.AlternateSetup;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Preparation;
using Xunit;

namespace RISK.Tests.GuiWpf.Specifications
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

            gameSettingsViewModel.Players.First().IsEnabled = true;
            gameSettingsViewModel.Players.ElementAt(1).IsEnabled = true;

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

            for (int i = 0; i < numberOfArmiesToPlace; i++)
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