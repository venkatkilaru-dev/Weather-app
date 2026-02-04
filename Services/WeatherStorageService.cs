using System.Text.Json;

namespace WeatherHistoryDataRecorder.Services;

public class WeatherStorageService
{
    private readonly string _folderPath = Path.Combine("weather-data");

    public WeatherStorageService()
    {
        if (!Directory.Exists(_folderPath))
            Directory.CreateDirectory(_folderPath);
    }

    public string GetFilePath(string isoDate)
    {
        return Path.Combine(_folderPath, $"{isoDate}.json");
    }

    public bool Exists(string isoDate)
    {
        return File.Exists(GetFilePath(isoDate));
    }

    public async Task SaveAsync(string isoDate, WeatherDataResponse data)
    {
        var filePath = GetFilePath(isoDate);

        var json = JsonSerializer.Serialize(
            data,
            new JsonSerializerOptions { WriteIndented = true }
        );

        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<WeatherDataResponse?> LoadAsync(string isoDate)
    {
        var filePath = GetFilePath(isoDate);

        if (!File.Exists(filePath))
            return null;

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<WeatherDataResponse>(json);
    }
}
