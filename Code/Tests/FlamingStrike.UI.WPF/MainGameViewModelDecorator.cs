using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF;
using FlamingStrike.UI.WPF.ViewModels;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace Tests.FlamingStrike.UI.WPF
{
    internal class MainGameViewModelDecorator : MainGameViewModel
    {
        public MainGameViewModelDecorator(CompositionRoot compositionRoot)
            : base(compositionRoot) {}

        public MainGameViewModelDecorator(
            IPlayerUiDataRepository playerUiDataRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameFactory gameFactory)
            : base(
                playerUiDataRepository,
                alternateGameSetupFactory,
                gamePreparationViewModelFactory,
                gameplayViewModelFactory,
                alternateGameSetupViewModelFactory,
                gameFactory) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}