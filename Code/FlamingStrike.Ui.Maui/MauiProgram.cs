using Caliburn.Micro;
using FlamingStrike.Maui.ViewModels.Preparation;
using FlamingStrike.Maui.Views;
using Microsoft.Extensions.Logging;

namespace FlamingStrike.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IPlayerTypes, PlayerTypes>();
            builder.Services.AddSingleton<IPlayerUiDataRepository, PlayerUiDataRepository>();
            builder.Services.AddSingleton<IEventAggregator, EventAggregator>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
