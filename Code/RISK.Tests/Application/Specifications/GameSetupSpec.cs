using System;
using System.Linq;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.Entities;
using RISK.Application.Extensions;
using RISK.Application.GamePlaying.Setup;
using Xunit;

namespace RISK.Tests.Application.Specifications
{
    public class GameSetupSpec : AcceptanceTestsBase<GameSetupSpec>
    {
        private MainGameViewModel _mainGameViewModel;
        private AutoRespondingUserInteractor _userInteractor;
        private IMainViewModel _setupViewModel;

        [Fact]
        public void Game_is_started()
        {
            Given
                .a_new_game();

            When
                .two_human_players_are_confirmed()
                .all_armies_are_placed_on_the_map()
                .the_main_view_is_changed();

            Then
                .the_game_is_started();
        }

        private GameSetupSpec a_new_game()
        {
            //_userInteractor = new AutoRespondingUserInteractor();
            //ObjectFactory.Inject<IUserInteractor>(_userInteractor);

            //_mainGameViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

            var root = new Root();
            _userInteractor = new AutoRespondingUserInteractor();
            root.UserInteractor = _userInteractor;
            
            _mainGameViewModel = new MainGameViewModelAdapter(root);
            _mainGameViewModel.OnInitializeFromChild();

            return this;
        }

        private GameSetupSpec two_human_players_are_confirmed()
        {
            var gameSettingsViewModel = (IGameInitializationViewModel)_mainGameViewModel.ActiveItem;

            gameSettingsViewModel.Players.First().IsEnabled = true;
            gameSettingsViewModel.Players.Second().IsEnabled = true;

            gameSettingsViewModel.Confirm();

            _setupViewModel = _mainGameViewModel.ActiveItem;

            return this;
        }

        private GameSetupSpec all_armies_are_placed_on_the_map()
        {
            const int numberOfArmiesToPlace = (40 - 21) * 2;

            while (_userInteractor.LocationsHasBeenSelected < numberOfArmiesToPlace) { }

            return this;
        }

        private GameSetupSpec the_main_view_is_changed()
        {
            while (_mainGameViewModel.ActiveItem == _setupViewModel) { }

            return this;
        }

        private void the_game_is_started()
        {
            _mainGameViewModel.ActiveItem.Should().BeOfType<GameboardViewModel>();
        }
    }

    internal class MainGameViewModelAdapter : MainGameViewModel
    {
        public MainGameViewModelAdapter(Root root)
            : base(root)
        {
        }
    }

    internal class AutoRespondingUserInteractor : IUserInteractor
    {
        public int LocationsHasBeenSelected { get; set; }

        public ITerritory GetLocation(ITerritorySelectorParameter territorySelector)
        {
            LocationsHasBeenSelected++;
            return territorySelector.EnabledTerritories.First();
        }

        public void SelectLocation(ITerritory location)
        {
            throw new NotImplementedException();
        }
    }
}