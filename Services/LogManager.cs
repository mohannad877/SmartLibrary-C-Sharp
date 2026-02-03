using System;
using System.IO;
using System.Text;

namespace smartLibraryForC_.Services
{
    /// <summary>
    /// نظام تسجيل احترافي بملفات مع دوران تلقائي
    /// </summary>
    public class LogManager
    {
        private static LogManager _instance;
        private readonly string _logDirectory;
        private readonly string _logFileName;
        private readonly object _lockObject = new object();
        private const long MaxLogSizeBytes = 5 * 1024 * 1024; // 5MB

        public static LogManager Instance => _instance ?? (_instance = new LogManager());

        private LogManager()
        {
            _logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            _logFileName = "SmartLibrary.log";

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }

            RotateLogIfNeeded();
        }

        public enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error
        }

        public void Log(LogLevel level, string message, Exception exception = null)
        {
            try
            {
                lock (_lockObject)
                {
                    var logPath = Path.Combine(_logDirectory, _logFileName);
                    var logMessage = FormatLogMessage(level, message, exception);

                    File.AppendAllText(logPath, logMessage, Encoding.UTF8);

                    // Also write to Debug for development
                    System.Diagnostics.Debug.WriteLine($"[{level}] {message}");

                    RotateLogIfNeeded();
                }
            }
            catch
            {
                // Silent fail - logging should not crash the app
            }
        }

        private string FormatLogMessage(LogLevel level, string message, Exception exception)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
   
            if (exception != null)
            {
                sb.AppendLine($"  Exception: {exception.GetType().Name}");
                sb.AppendLine($"  Message: {exception.Message}");
                sb.AppendLine($"  StackTrace: {exception.StackTrace}");
            }

            return sb.ToString();
        }

        private void RotateLogIfNeeded()
        {
            try
            {
                var logPath = Path.Combine(_logDirectory, _logFileName);
                
                if (!File.Exists(logPath))
                    return;

                var fileInfo = new FileInfo(logPath);
                
                if (fileInfo.Length > MaxLogSizeBytes)
                {
                    var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    var archiveName = $"SmartLibrary_{timestamp}.log";
                    var archivePath = Path.Combine(_logDirectory, archiveName);
                    
                    File.Move(logPath, archivePath);
                    
                    // Keep only last 5 archived logs
                    CleanOldLogs();
                }
            }
            catch
            {
                // Silent fail
            }
        }

        private void CleanOldLogs()
        {
            try
            {
                var logFiles = Directory.GetFiles(_logDirectory, "SmartLibrary_*.log");
                
                if (logFiles.Length > 5)
                {
                    Array.Sort(logFiles);
                    
                    for (int i = 0; i < logFiles.Length - 5; i++)
                    {
                        File.Delete(logFiles[i]);
                    }
                }
            }
            catch
            {
                // Silent fail
            }
        }

        // Convenience methods
        public void Debug(string message) => Log(LogLevel.Debug, message);
        public void Info(string message) => Log(LogLevel.Info, message);
        public void Warning(string message) => Log(LogLevel.Warning, message);
        public void Error(string message, Exception ex = null) => Log(LogLevel.Error, message, ex);
    }
}
