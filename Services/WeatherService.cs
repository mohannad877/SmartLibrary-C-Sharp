using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using smartLibraryForC_.Models;

namespace smartLibraryForC_.Services
{
    /// <summary>
    /// خدمة الاتصال بواجهة برمجة الطقس (Open-Meteo API)
    /// </summary>
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.open-meteo.com/v1/forecast";

        // إحداثيات الرياض، السعودية (يمكن تغييرها)
        private const double DefaultLatitude = 24.7136;
        private const double DefaultLongitude = 46.6753;

        public WeatherService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        /// <summary>
        /// جلب حالة الطقس الحالية
        /// </summary>
        public async Task<WeatherInfo> GetCurrentWeatherAsync(double latitude = DefaultLatitude, double longitude = DefaultLongitude)
        {
            try
            {
                var url = $"{BaseUrl}?latitude={latitude}&longitude={longitude}&current_weather=true&timezone=auto";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                
                var result = JsonSerializer.Deserialize<OpenMeteoResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result == null || result.CurrentWeather == null)
                {
                     return null;
                }

                return new WeatherInfo
                {
                    Temperature = result.CurrentWeather.Temperature,
                    Condition = GetWeatherCondition(result.CurrentWeather.WeatherCode),
                    IconCode = result.CurrentWeather.WeatherCode.ToString(),
                    WindSpeed = result.CurrentWeather.WindSpeed,
                    Time = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"خطأ في جلب الطقس: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// تحويل كود الطقس إلى نص وصفي
        /// </summary>
        private string GetWeatherCondition(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "سماء صافية",
                1 => "غائم جزئياً",
                2 => "غائم",
                3 => "غائم كلياً",
                45 => "ضباب",
                48 => "ضباب",
                51 => "رذاذ",
                53 => "رذاذ",
                55 => "رذاذ",
                61 => "مطر",
                63 => "مطر",
                65 => "مطر",
                71 => "ثلج",
                73 => "ثلج",
                75 => "ثلج",
                80 => "زخات مطر",
                81 => "زخات مطر",
                82 => "زخات مطر",
                95 => "عاصفة رعدية",
                96 => "عاصفة رعدية",
                99 => "عاصفة رعدية",
                _ => "غير معروف"
            };
        }

        /// <summary>
        /// الحصول على الوقت الحالي
        /// </summary>
        public string GetCurrentTime()
        {
            return DateTime.Now.ToString("hh:mm:ss tt");
        }

        /// <summary>
        /// الحصول على التاريخ
        /// </summary>
        public string GetCurrentDate()
        {
            return DateTime.Now.ToString("yyyy/MM/dd");
        }

        /// <summary>
        /// الحصول على اسم اليوم
        /// </summary>
        public string GetDayName()
        {
            return DateTime.Now.ToString("dddd", new System.Globalization.CultureInfo("ar-SA"));
        }
    }

    /// <summary>
    /// نموذج استجابة Open-Meteo
    /// </summary>
    public class OpenMeteoResponse
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        
        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = string.Empty;

        [JsonPropertyName("current_weather")]
        public CurrentWeather CurrentWeather { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("temperature")]
        public double Temperature { get; set; }
        
        [JsonPropertyName("windspeed")]
        public double Windspeed { get; set; }

        [JsonPropertyName("weathercode")]
        public int WeatherCode { get; set; }
        
        [JsonPropertyName("time")]
        public string Time { get; set; }

        public double WindSpeed => Windspeed;
    }
}
