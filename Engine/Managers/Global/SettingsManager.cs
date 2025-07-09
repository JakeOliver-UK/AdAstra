using AdAstra.Utils;
using Newtonsoft.Json;
using System;
using System.IO;

namespace AdAstra.Engine.Managers.Global
{
    internal static class SettingsManager
    {
        public static Settings Settings => _settings;
        public static string FilePath => Path.Combine(AppInfo.UserDataDirectory, "settings.json");

        private static Settings _settings;
        private static readonly JsonSerializerSettings _jsonSettings = new() { Formatting = Formatting.Indented };

        public static void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing Settings Manager...");

            Load();
            Apply();

            Log.WriteLine(LogLevel.Info, "Settings Manager initialized successfully.");
        }

        public static void Load()
        {
            Log.WriteLine(LogLevel.Info, $"Loading settings from file '{FilePath}'...");
            
            if (!File.Exists(FilePath))
            {
                Log.WriteLine(LogLevel.Warning, $"Settings file '{FilePath}' not found. Creating default settings.");
                Reset();
                Save();
                return;
            }

            try
            {
                string json = File.ReadAllText(FilePath);
                _settings = JsonConvert.DeserializeObject<Settings>(json, _jsonSettings) ?? new Settings();
            }
            catch (JsonException ex)
            {
                Log.WriteLine(LogLevel.Error, $"Failed to load settings from file '{FilePath}': {ex.Message}");
                Reset();
                Save();
                return;
            }
            catch (IOException ex)
            {
                Log.WriteLine(LogLevel.Error, $"I/O error while loading settings from file '{FilePath}': {ex.Message}");
                Reset();
                Save();
                return;
            }

            Log.WriteLine(LogLevel.Info, $"Settings loaded from file '{FilePath}' successfully.");
        }

        public static void Save()
        {
            Log.WriteLine(LogLevel.Info, $"Saving settings to file '{FilePath}'...");

            if (!Directory.Exists(AppInfo.UserDataDirectory)) Directory.CreateDirectory(AppInfo.UserDataDirectory);
            string json = JsonConvert.SerializeObject(Settings, _jsonSettings);
            File.WriteAllText(FilePath, json);

            Log.WriteLine(LogLevel.Info, $"Settings saved to file '{FilePath}' successfully.");
        }

        public static void Reset()
        {
            _settings = new Settings();
        }

        public static void Apply()
        {
            App.Instance.GraphicsDeviceManager.PreferredBackBufferWidth = Settings.WindowWidth;
            App.Instance.GraphicsDeviceManager.PreferredBackBufferHeight = Settings.WindowHeight;
            App.Instance.GraphicsDeviceManager.IsFullScreen = Settings.Fullscreen;
            App.Instance.GraphicsDeviceManager.HardwareModeSwitch = !Settings.Borderless;
            App.Instance.GraphicsDeviceManager.SynchronizeWithVerticalRetrace = Settings.VSync;
            App.Instance.GraphicsDeviceManager.ApplyChanges();
            App.Instance.IsFixedTimeStep = Settings.VSync;
            App.Instance.TargetElapsedTime = TimeSpan.FromSeconds(1.0 / Settings.TargetFrameRate);
        }

        public static void Dispose()
        {
            Log.WriteLine(LogLevel.Info, "Unloading Settings Manager...");

            _settings = null;

            Log.WriteLine(LogLevel.Info, "Settings Manager unloaded successfully.");
        }
    }
}
