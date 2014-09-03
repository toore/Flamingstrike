using System;
using System.Linq;
using FluentAssertions;
using GuiWpf.Infrastructure;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Domain.Entities;
using RISK.Domain.Extensions;
using RISK.Domain.GamePlaying.Setup;
using StructureMap;
using Xunit;

namespace RISK.Tests.Application.Specifications
{
    public class game_setup_spec : AcceptanceTestsBase<game_setup_spec>
    {
        private IMainGameViewModel _mainGameViewModel;
        private AutoRespondingUserInteractor _userInteractor;
        private IMainViewModel _setupViewModel;

        public game_setup_spec()
        {
            new PluginConfiguration().Configure();
        }

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

        private game_setup_spec a_new_game()
        {
            _userInteractor = new AutoRespondingUserInteractor();
            ObjectFactory.Inject<IUserInteractor>(_userInteractor);

            _mainGameViewModel = ObjectFactory.GetInstance<IMainGameViewModel>();

            return this;
        }

        private game_setup_spec two_human_players_are_confirmed()
        {
            var gameSettingsViewModel = (IGameSettingsViewModel)_mainGameViewModel.MainViewModel;

            gameSettingsViewModel.Players.First().IsEnabled = true;
            gameSettingsViewModel.Players.Second().IsEnabled = true;

            gameSettingsViewModel.Confirm();

            _setupViewModel = _mainGameViewModel.MainViewModel;

            return this;
        }

        private game_setup_spec all_armies_are_placed_on_the_map()
        {
            const int numberOfArmiesToPlace = (40 - 21) * 2;

            while (_userInteractor.LocationsHasBeenSelected < numberOfArmiesToPlace) {}

            return this;
        }

        private game_setup_spec the_main_view_is_changed()
        {
            while (_mainGameViewModel.MainViewModel == _setupViewModel) {}

            return this;
        }

        private void the_game_is_started()
        {
            _mainGameViewModel.MainViewModel.Should().BeOfType<GameboardViewModel>();
        }
    }

    internal class AutoRespondingUserInteractor : IUserInteractor
    {
        public int LocationsHasBeenSelected { get; set; }

        public ILocation GetLocation(ILocationSelectorParameter locationSelector)
        {
            LocationsHasBeenSelected++;
            return locationSelector.AvailableLocations.First();
        }

        public void SelectLocation(ILocation location)
        {
            throw new NotImplementedException();
        }
    }
}