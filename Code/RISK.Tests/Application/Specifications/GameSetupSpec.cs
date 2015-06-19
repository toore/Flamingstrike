﻿using System;
using System.Linq;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.GameSetup;
using RISK.Application.World;
using RISK.Tests.GuiWpf;
using Xunit;

namespace RISK.Tests.Application.Specifications
{
    public class GameSetupSpec : SpecBase<GameSetupSpec>
    {
        private MainGameViewModelAdapter _mainGameViewModel;
        private IGameSetupViewModel _setupViewModel;
        private AutoRespondingUserInteractor _userInteractor;

        [Fact]
        public void Game_is_started()
        {
            Given
                .a_new_game();

            When
                .two_human_players_are_confirmed()
                .all_armies_are_placed_on_the_map();

            Then
                .the_game_is_started();
        }

        private GameSetupSpec a_new_game()
        {
            _userInteractor = new AutoRespondingUserInteractor();

            var root = new Root();
            root.UserInteractor = _userInteractor;
            root.GuiThreadDispatcher = new BypassGuiThreadDispatcher();
            root.TaskEx = new SynchronousTaskEx();
            _mainGameViewModel = new MainGameViewModelAdapter(root);
            _mainGameViewModel.OnInitialize();

            return this;
        }

        private GameSetupSpec two_human_players_are_confirmed()
        {
            var gameSettingsViewModel = (IGameSettingsViewModel)_mainGameViewModel.ActiveItem;

            gameSettingsViewModel.Players.First().IsEnabled = true;
            gameSettingsViewModel.Players.ElementAt(1).IsEnabled = true;

            gameSettingsViewModel.Confirm();

            _setupViewModel = (IGameSetupViewModel)_mainGameViewModel.ActiveItem;
            _setupViewModel.Activate();

            return this;
        }

        private GameSetupSpec all_armies_are_placed_on_the_map()
        {
            const int numberOfArmiesToPlace = (40 - 21) * 2;

            _userInteractor.NumberOfSelectTerritoryRequests.Should().Be(numberOfArmiesToPlace);

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

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }

    internal class AutoRespondingUserInteractor : IUserInteractor
    {
        public int NumberOfSelectTerritoryRequests { get; set; }

        public ITerritory GetSelectedTerritory(ITerritoryRequestParameter territoryRequestParameter)
        {
            NumberOfSelectTerritoryRequests++;
            return territoryRequestParameter.EnabledTerritories.First();
        }

        public void SelectTerritory(ITerritory location)
        {
            throw new NotImplementedException();
        }
    }
}