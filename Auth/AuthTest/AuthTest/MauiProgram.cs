using AuthTest.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace AuthTest
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
                });

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddSingleton<AuthenticationStateProvider,CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

            //builder.Services.AddSingleton(serviceProvider =>
            //{
            //    var dbPath = Path.Combine(FileSystem.AppDataDirectory, "demo_local_db.db3");
            //    return new SQLLiteAsyncConnection(dbPath);
            //});




#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
