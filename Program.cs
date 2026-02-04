using WeatherHistoryDataRecorder.Components;
using WeatherHistoryDataRecorder.Services;
using WeatherHistoryDataRecorder.Components.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<DateParsingService>(sp =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "dates.txt");
    return new DateParsingService(filePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapGet("/api/test-dates", (DateParsingService parser) =>
{
    var results = parser.ReadAndParseAllDates();
    return Results.Ok(results);
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
