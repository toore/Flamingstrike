using FlamingStrike.UI.WPF;
using FlamingStrike.UI.WPF.Services.GameEngineClient;
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
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameEngineClientProxy gameEngineClientProxy)
            : base(
                playerUiDataRepository,
                gamePreparationViewModelFactory,
                gameplayViewModelFactory,
                alternateGameSetupViewModelFactory,
                gameEngineClientProxy) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}