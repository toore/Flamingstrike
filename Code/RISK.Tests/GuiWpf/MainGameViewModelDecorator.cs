using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;
using RISK.Application.Setup;

namespace RISK.Tests.GuiWpf
{
    internal class MainGameViewModelDecorator : MainGameViewModel
    {
        public MainGameViewModelDecorator(Root root)
            : base(root) {}

        public MainGameViewModelDecorator(
            IPlayerRepository playerRepository,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IGameInitializationViewModelFactory gameInitializationViewModelFactory,
            IGameboardViewModelFactory gameboardViewModelFactory,
            IGameSetupViewModelFactory gameSetupViewModelFactory,
            IUserInteractorFactory userInteractorFactory)
            : base(
                  playerRepository,
                  alternateGameSetupFactory, 
                  gameInitializationViewModelFactory, 
                  gameboardViewModelFactory, 
                  gameSetupViewModelFactory,
                  userInteractorFactory) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}