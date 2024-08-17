using FlamingStrike.Maui.ViewModels.Preparation;

namespace FlamingStrike.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();

        var gamePreparationViewModelFactory = new GamePreparationViewModelFactory(
            new PlayerTypes(),
            new PlayerUiDataRepository());

        MainPage.BindingContext = gamePreparationViewModelFactory.Create();
    }
}