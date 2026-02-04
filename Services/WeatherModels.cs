namespace WeatherHistoryDataRecorder.Services;

public class DailyWeatherResponse
{
    public DailyUnits? DailyUnits { get; set; }
    public DailyData? Daily { get; set; }
}

public class DailyUnits
{
    public string? Temperature_2m_min { get; set; }
    public string? Temperature_2m_max { get; set; }
    public string? Precipitation_sum { get; set; }
}

public class DailyData
{
    public List<string>? Time { get; set; }
    public List<double>? Temperature_2m_min { get; set; }
    public List<double>? Temperature_2m_max { get; set; }
    public List<double>? Precipitation_sum { get; set; }
}
