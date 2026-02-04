using System.Net.Http.Json;

namespace WeatherHistoryDataRecorder.Services;

public class OpenMeteoClient
{
    private readonly HttpClient _http;

    public OpenMeteoClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<WeatherDataResponse> GetWeatherForDateAsync(string isoDate)
    {
        try
        {
            var url =
                $"https://archive-api.open-meteo.com/v1/archive" +
                $"?latitude=32.78&longitude=-96.8" +
                $"&start_date={isoDate}&end_date={isoDate}" +
                $"&daily=temperature_2m_min,temperature_2m_max,precipitation_sum" +
                $"&timezone=auto";

            var response = await _http.GetFromJsonAsync<DailyWeatherResponse>(url);

            if (response?.Daily == null ||
                response.Daily.Temperature_2m_min == null ||
                response.Daily.Temperature_2m_max == null ||
                response.Daily.Precipitation_sum == null ||
                response.Daily.Temperature_2m_min.Count == 0)
            {
                return new WeatherDataResponse
                {
                    Success = false,
                    Date = isoDate,
                    Error = "No weather data returned from API"
                };
            }

            return new WeatherDataResponse
            {
                Success = true,
                Date = isoDate,
                TemperatureMin = response.Daily.Temperature_2m_min.FirstOrDefault(),
                TemperatureMax = response.Daily.Temperature_2m_max.FirstOrDefault(),
                PrecipitationSum = response.Daily.Precipitation_sum.FirstOrDefault(),
                Error = null
            };
        }
        catch (Exception ex)
        {
            return new WeatherDataResponse
            {
                Success = false,
                Date = isoDate,
                Error = $"API error: {ex.Message}"
            };
        }
    }
}
