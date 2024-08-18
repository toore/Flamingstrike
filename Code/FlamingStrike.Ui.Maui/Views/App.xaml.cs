using Caliburn.Micro;
using FlamingStrike.Maui.ViewModels.Preparation;

namespace FlamingStrike.Maui.Views;

public partial class App : Application
{
    public App(
        IPlayerTypes playerTypes, 
        IPlayerUiDataRepository playerUiDataRepository,
        IEventAggregator eventAggregator)
    {
        InitializeComponent();

        var appShell = new AppShell();
        MainPage = appShell;

        var gamePreparationViewModelFactory = new GamePreparationViewModelFactory(
            playerTypes,
            playerUiDataRepository,
            eventAggregator);

        MainPage.BindingContext = gamePreparationViewModelFactory.Create();
    }
}