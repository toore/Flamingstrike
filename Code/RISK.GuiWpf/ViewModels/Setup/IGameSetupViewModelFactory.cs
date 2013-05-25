namespace GuiWpf.ViewModels.Setup
{
    public interface IGameSetupViewModelFactory
    {
        IGameSetupViewModel Create(IGameSettingStateConductor gameSettingStateConductor);
    }
}