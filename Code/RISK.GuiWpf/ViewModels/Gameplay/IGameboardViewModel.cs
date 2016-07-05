namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
        string InformationText { get; }
        string PlayerName { get; }
        bool CanActivateFreeMove();
        void ActivateFreeMove();
        bool CanEndTurn();
        void EndTurn();
        void EndGame();
    }
}