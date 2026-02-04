using WeatherHistoryDataRecorder.Components;
using WeatherHistoryDataRecorder.Services;
using WeatherHistoryDataRecorder.Components.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<DateParsingService>(sp =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "dates.txt");
    return new DateParsingService(filePath);
});

builder.Services.AddHttpClient<OpenMeteoClient>();
builder.Services.AddSingleton<WeatherOrchestrator>();
builder.Services.AddSingleton<WeatherStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.MapGet("/api/test-dates", (DateParsingService parser) =>
{
    var results = parser.ReadAndParseAllDates();
    return Results.Ok(results);
});
app.MapGet("/api/test-weather/{date}", async (string date, OpenMeteoClient client) =>
{
    var result = await client.GetWeatherForDateAsync(date);
    return Results.Ok(result);
});
app.MapGet("/api/weather/{date}", async (string date, WeatherOrchestrator orchestrator) =>
{
    var result = await orchestrator.GetOrFetchAsync(date);
    return Results.Ok(result);
});
app.MapGet("/api/weather", async (WeatherStorageService storage) =>
{
    var all = await storage.LoadAllAsync();
    return Results.Ok(all);
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
