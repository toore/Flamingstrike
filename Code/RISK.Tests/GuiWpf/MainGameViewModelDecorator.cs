using GuiWpf.ViewModels;
using GuiWpf.ViewModels.Gameplay;
using GuiWpf.ViewModels.Settings;
using GuiWpf.ViewModels.Setup;

namespace RISK.Tests.GuiWpf
{
    internal class MainGameViewModelDecorator : MainGameViewModel
    {
        public MainGameViewModelDecorator(Root root)
            : base(root) {}

        public MainGameViewModelDecorator(
            IGameInitializationViewModelFactory gameInitializationViewModelFactory,
            IGameboardViewModelFactory gameboardViewModelFactory,
            IGameSetupViewModelFactory gameSetupViewModelFactory,
            IAlternateGameSetupFactory alternateGameSetupFactory,
            IPlayerRepository playerRepository)
            : base(gameInitializationViewModelFactory, 
                  gameboardViewModelFactory, 
                  gameSetupViewModelFactory, 
                  alternateGameSetupFactory, 
                  playerRepository) {}

        public new void OnInitialize()
        {
            base.OnInitialize();
        }
    }
}