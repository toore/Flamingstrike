namespace GuiWpf.ViewModels.Gameplay
{
    public interface IGameboardViewModel : IMainViewModel
    {
        WorldMapViewModel WorldMapViewModel { get; }
        string InformationText { get; }
        string PlayerName { get; }
        bool CanEnterFortifyMode { get; }
        void EnterFortifyMode();
        bool CanEndTurn { get; }
        void EndTurn();
        void EndGame();
    }
}