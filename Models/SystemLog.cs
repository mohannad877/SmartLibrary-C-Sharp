using System;

namespace smartLibraryForC_.Models
{
    /// <summary>
    /// نموذج سجل النظام لتتبع العمليات
    /// </summary>
    public class SystemLog
    {
        public int LogId { get; set; }
        public LogActionType ActionType { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Details { get; set; }
        public int? BookId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// أنواع العمليات المسجلة
    /// </summary>
    public enum LogActionType
    {
        Search = 0,
        Download = 1,
        Open = 2,
        Delete = 3,
        Update = 4,
        Login = 5,
        WeatherUpdate = 6,
        System = 7
    }
}
