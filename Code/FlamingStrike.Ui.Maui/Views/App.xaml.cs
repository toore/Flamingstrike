using FlamingStrike.Maui.ViewModels.Preparation;

namespace FlamingStrike.Maui.Views;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var appShell = new AppShell();
        MainPage = appShell;

        var gamePreparationViewModelFactory = new GamePreparationViewModelFactory(
            new PlayerTypes(),
            new PlayerUiDataRepository());

        MainPage.BindingContext = gamePreparationViewModelFactory.Create();
    }
}