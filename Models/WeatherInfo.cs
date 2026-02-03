using System;

namespace smartLibraryForC_.Models
{
    /// <summary>
    /// نموذج بيانات معلومات الطقس
    /// </summary>
    public class WeatherInfo
    {
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public string IconCode { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double WindSpeed { get; set; }
        public int Humidity { get; set; }
        public DateTime Time { get; set; } = DateTime.Now;
    }
}
