using System;

namespace WebApplication3
{
    [Serializable]
    public class WeatherForecast
    {
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    [Serializable]
    public class PostRequest
    {
        public int TemperatureC { get; set; }
    }
}
