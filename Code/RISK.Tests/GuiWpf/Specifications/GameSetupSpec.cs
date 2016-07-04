using System;
using System.Linq;
using FluentAssertions;
using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Core;
using RISK.GameEngine.Setup;
using Xunit;

namespace RISK.Tests.GuiWpf.Specifications
{
    public class GameSetupSpec : SpecBase<GameSetupSpec>
    {
        private MainGameViewModelDecorator _mainGameViewModel;
        private GameSetupViewModel _setupViewModel;
        private AutoRespondingUserInteraction _autoRespondingUserInteraction;
        private UserInteractionFactoryReturningSameInstance _userInteractionFactoryReturningSameInstance;

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

        private GameSetupSpec a_new_game()
        {
            _autoRespondingUserInteraction = new AutoRespondingUserInteraction();
            _userInteractionFactoryReturningSameInstance = new UserInteractionFactoryReturningSameInstance(_autoRespondingUserInteraction);
            var noGuiThreadDispatcher = new CurrentThreadDispatcher();

            var root = new Root(new SynchronousTaskEx());
            root.UserInteractorFactory = new UserInteractorFactory(
                _userInteractionFactoryReturningSameInstance, noGuiThreadDispatcher);

            _mainGameViewModel = new MainGameViewModelDecorator(root);
            ViewIsCreated(_mainGameViewModel);

            return this;
        }

        private static void ViewIsCreated(MainGameViewModelDecorator viewModel)
        {
            viewModel.OnInitialize();
        }

        private GameSetupSpec two_human_players_are_confirmed()
        {
            var gameSettingsViewModel = (IGamePreparationViewModel)_mainGameViewModel.ActiveItem;

            gameSettingsViewModel.Players.First().IsEnabled = true;
            gameSettingsViewModel.Players.ElementAt(1).IsEnabled = true;

            gameSettingsViewModel.Confirm();

            _setupViewModel = (GameSetupViewModel)_mainGameViewModel.ActiveItem;
            _setupViewModel.Activate();

            return this;
        }

        private GameSetupSpec all_armies_are_placed_on_the_map()
        {
            const int numberOfArmiesForEachPlayerToDeployInTwoPlayerMode = 40;
            const int nonOptionalArmyDeploymentPerPlayer = 21;
            const int numberOfPlayers = 2;
            const int numberOfArmiesToPlace = (numberOfArmiesForEachPlayerToDeployInTwoPlayerMode - nonOptionalArmyDeploymentPerPlayer) * numberOfPlayers;

            _autoRespondingUserInteraction.NumberOfCallsToWaitForTerritoryToBeSelected.Should().Be(numberOfArmiesToPlace);

            return this;
        }

        private void the_game_is_started()
        {
            _mainGameViewModel.ActiveItem.Should().BeOfType<GameboardViewModel>();
        }
    }

    internal class UserInteractionFactoryReturningSameInstance : IUserInteractionFactory
    {
        private readonly AutoRespondingUserInteraction _autoRespondingUserInteraction;

        public UserInteractionFactoryReturningSameInstance(AutoRespondingUserInteraction autoRespondingUserInteraction)
        {
            _autoRespondingUserInteraction = autoRespondingUserInteraction;
        }

        public IUserInteraction Create()
        {
            return _autoRespondingUserInteraction;
        }
    }

    internal class AutoRespondingUserInteraction : IUserInteraction
    {
        public int NumberOfCallsToWaitForTerritoryToBeSelected { get; private set; }

        public IRegion WaitForTerritoryToBeSelected(ITerritoryRequestParameter territoryRequestParameter)
        {
            NumberOfCallsToWaitForTerritoryToBeSelected++;
            return territoryRequestParameter.EnabledTerritories.First();
        }

        public void SelectTerritory(IRegion region)
        {
            throw new NotImplementedException();
        }
    }
}