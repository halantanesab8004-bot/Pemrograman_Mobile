using BlazingPizzaApp.API.Data;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSqlite<PizzaStoreContext>(
    builder.Configuration.GetConnectionString("PizzaStore") ?? "Data Source=pizza.db");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Serve static files (for any CSS/images used by the Blazor UI)
app.UseStaticFiles();

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
