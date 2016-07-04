using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Preparation;
using GuiWpf.ViewModels.Setup;
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
            IGameSetupViewModelFactory gameSetupViewModelFactory,
            IUserInteractorFactory userInteractorFactory)
            : base(
                  playerRepository,
                  alternateGameSetupFactory, 
                  gamePreparationViewModelFactory, 
                  gameboardViewModelFactory, 
                  gameSetupViewModelFactory,
                  userInteractorFactory) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}