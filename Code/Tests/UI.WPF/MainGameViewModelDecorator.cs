using System.Threading;
using System.Threading.Tasks;
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
            IGameEngineClient gameEngineClient)
            : base(
                playerUiDataRepository,
                gamePreparationViewModelFactory,
                gameplayViewModelFactory,
                alternateGameSetupViewModelFactory,
                gameEngineClient) {}

        public new async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await base.OnInitializeAsync(cancellationToken);
        }
    }
}