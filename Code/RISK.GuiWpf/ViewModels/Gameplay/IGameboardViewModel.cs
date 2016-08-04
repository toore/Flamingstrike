namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
        string InformationText { get; }
        string PlayerName { get; }
        bool CanActivateFortify();
        void EnterFortifyMode();
        bool CanEndTurn();
        void EndTurn();
        void EndGame();
    }
}