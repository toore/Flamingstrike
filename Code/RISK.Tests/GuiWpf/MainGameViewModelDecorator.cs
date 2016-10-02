using GuiWpf.ViewModels;
using GuiWpf.ViewModels.AlternateSetup;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Preparation;
using RISK.GameEngine.Play;
using RISK.GameEngine.Setup;

namespace RISK.Tests.GuiWpf
{
    internal class MainGameViewModelDecorator : MainGameViewModel
    {
        public MainGameViewModelDecorator(Root root)
            : base(root) {}

        public MainGameViewModelDecorator(
            IPlayerRepository playerRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGamePreparationViewModelFactory gamePreparationViewModelFactory,
            IGameplayViewModelFactory gameplayViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IGameFactory gameFactory)
            : base(
                playerRepository,
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