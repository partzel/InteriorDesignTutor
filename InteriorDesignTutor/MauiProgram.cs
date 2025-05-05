using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;

namespace InteriorDesignTutor
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit() // Initialize CommunityToolkit.Maui
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register IAudioManager
            // builder.Services.AddSingleton(AudioManager.Current);

            return builder.Build();
        }
    }
}
