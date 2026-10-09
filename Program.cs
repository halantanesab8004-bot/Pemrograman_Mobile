using BlazingPizzaApp.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Add Razor Pages and Server-side Blazor services so we can host a minimal Blazor UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IPizzaSpecials, PizzaSpecialsDAL>();
// Order state for storing the current order in memory
builder.Services.AddScoped<BlazingPizzaApp.API.Services.OrderState>();
// Register HttpClient for server-side components to call backend APIs
builder.Services.AddHttpClient();
// Provide a default HttpClient instance with BaseAddress pointing to the local API
// so components can use relative URIs like "/api/pizzaspecials".
builder.Services.AddScoped(sp => new System.Net.Http.HttpClient { BaseAddress = new Uri("http://localhost:5123/") });
builder.Services.AddSqlite<PizzaStoreContext>(
    builder.Configuration.GetConnectionString("PizzaStore") ?? "Data Source=pizza.db");

var app = builder.Build();

var webRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
if (Directory.Exists(webRootPath))
{
    app.Environment.WebRootPath = webRootPath;
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Serve static files from the project's wwwroot directory.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new CompositeFileProvider(
        new PhysicalFileProvider(app.Environment.WebRootPath),
        app.Environment.WebRootFileProvider)
});

app.UseAuthorization();

app.MapControllers();

// Map Razor Pages and Blazor hubs, and fallback to the Blazor host page
app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    db.Database.EnsureCreated();
    SeedData.InitializeAsync(db).GetAwaiter().GetResult();
}

app.Run();
