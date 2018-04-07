using FlamingStrike.GameEngine.Play;
using FlamingStrike.GameEngine.Setup;
using FlamingStrike.UI.WPF;
using FlamingStrike.UI.WPF.ViewModels;
using FlamingStrike.UI.WPF.ViewModels.AlternateSetup;
using FlamingStrike.UI.WPF.ViewModels.Gameplay;
using FlamingStrike.UI.WPF.ViewModels.Preparation;

namespace Tests.UI.WPF
{
    internal class MainGameViewModelDecorator : MainGameViewModel
    {
        public MainGameViewModelDecorator(CompositionRoot compositionRoot)
            : base(compositionRoot) {}

        public MainGameViewModelDecorator(
            IPlayerUiDataRepository playerUiDataRepository,
            IAlternateGameSetupBootstrapper alternateGameSetupBootstrapper,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameBootstrapper gameBootstrapper)
            : base(
                playerUiDataRepository,
                alternateGameSetupBootstrapper,
                gamePreparationViewModelFactory,
                gameplayViewModelFactory,
                alternateGameSetupViewModelFactory,
                gameBootstrapper) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}