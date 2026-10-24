using AcademicHybridApp.Services;
using Microsoft.Extensions.Logging;

namespace AcademicHybridApp;

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
		builder.Services.AddScoped(sp => new HttpClient
		{
			BaseAddress = new Uri("http://localhost:5009/")
		});
		builder.Services.AddScoped<DosenService>();
		builder.Services.AddScoped<MahasiswaService>();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
