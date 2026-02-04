namespace WeatherHistoryDataRecorder.Services;

public class WeatherOrchestrator
{
    private readonly OpenMeteoClient _client;
    private readonly WeatherStorageService _storage;

    public WeatherOrchestrator(OpenMeteoClient client, WeatherStorageService storage)
    {
        _client = client;
        _storage = storage;
    }

    public async Task<WeatherDataResponse> GetOrFetchAsync(string isoDate)
    {
        // 1. Validate the date BEFORE anything else
        if (!DateTime.TryParseExact(
                isoDate,
                "yyyy-MM-dd",
                null,
                System.Globalization.DateTimeStyles.None,
                out var parsedDate))
        {
            return new WeatherDataResponse
            {
                Success = false,
                Date = isoDate,
                Error = "Invalid date"
            };
        }

        // 2. Check local cache
        if (_storage.Exists(isoDate))
        {
            var cached = await _storage.LoadAsync(isoDate);
            if (cached != null)
                return cached;
        }

        // 3. Fetch from API
        var apiResult = await _client.GetWeatherForDateAsync(isoDate);

        var result = new WeatherDataResponse
        {
            Success = apiResult.Success,
            Date = isoDate,
            TemperatureMin = apiResult.TemperatureMin,
            TemperatureMax = apiResult.TemperatureMax,
            PrecipitationSum = apiResult.PrecipitationSum,
            Error = apiResult.Error
        };

        // 4. Save result
        await _storage.SaveAsync(isoDate, result);

        return result;
    }
}
