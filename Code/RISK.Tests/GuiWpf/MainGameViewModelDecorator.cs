using GuiWpf.ViewModels;
using GuiWpf.ViewModels.AlternateSetup;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Preparation;
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
            IGameboardViewModelFactory gameboardViewModelFactory,
            IAlternateGameSetupViewModelFactory alternateGameSetupViewModelFactory,
            IUserInteractorFactory userInteractorFactory)
            : base(
                  playerRepository,
                  alternateGameSetupFactory, 
                  gamePreparationViewModelFactory, 
                  gameboardViewModelFactory, 
                  alternateGameSetupViewModelFactory,
                  userInteractorFactory) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}