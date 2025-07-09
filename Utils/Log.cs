using System;
using System.IO;

namespace AdAstra.Utils
{
    internal static class Log
    {
        public static string DirectoryPath => Path.Combine(AppInfo.UserDataDirectory, "Logs");
        public static string CurrentFilePath => _currentFilePath;
        public static string PreviousFilePath => Path.Combine(DirectoryPath, "previous.log");

        private static string _currentFilePath;

        public static void Write(LogLevel level, string message, bool includeTimestamp)
        {
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            if (string.IsNullOrEmpty(_currentFilePath))
            {
                _currentFilePath = Path.Combine(DirectoryPath, "current.log");
                if (File.Exists(_currentFilePath)) File.Move(_currentFilePath, PreviousFilePath, true);
                string header = $"{AppInfo.Name} - v{AppInfo.Version} - {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}{Environment.NewLine}{Environment.NewLine}";
                File.WriteAllText(_currentFilePath, header);
            }

            string timestamp = includeTimestamp ? $"[{DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}] " : string.Empty;
            string logLevel = $"{level.ToString().ToUpper()}: ";
            if (level == LogLevel.None) logLevel = string.Empty;
            string logMessage = $"{timestamp}{logLevel}{message}";
            File.AppendAllText(_currentFilePath, logMessage);
        }

        public static void Write(LogLevel level, string message) => Write(level, message, true);
        public static void Write(string message) => Write(LogLevel.None, message, false);
        public static void WriteLine(LogLevel level, string message, bool includeTimestamp) => Write(level, message + Environment.NewLine, includeTimestamp);
        public static void WriteLine(LogLevel level, string message) => Write(level, message + Environment.NewLine, true);
        public static void WriteLine(string message) => Write(LogLevel.None, message + Environment.NewLine, false);
        public static void WriteLine() => Write(LogLevel.None, Environment.NewLine, false);
    }

    internal enum LogLevel
    {
        None,
        Debug,
        Info,
        Warning,
        Error
    }
}
