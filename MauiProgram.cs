using GamanaDashBoardApp;
using GamanaDashBoardApp.Services;
using GamanaDashBoardApp.Viewmodels;
using GamanaDashBoardApp.Views;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace GamanaDashBoardApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            // For Windows local development
            builder.Services.AddSingleton<HttpClient>(sp =>
            {
                var handler = new HttpClientHandler();
                // Bypass SSL certificate validation in development
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

                return new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:5001/"),
                    Timeout = TimeSpan.FromSeconds(30)
                };
            });
#endif

            builder.Services.AddSingleton<DashboardService>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<Dashboard>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

