using AdAstra.Engine.Managers.Instance;
using AdAstra.Utils;
using System.IO;

namespace AdAstra.Engine.Managers.Global
{
    internal static class AssetManager
    {
        public static string DirectoryPath => Path.Combine(AppInfo.ApplicationDirectory, "Assets");
        public static ImageManager Images => _images;
        public static FontManager Fonts => _fonts;

        private static ImageManager _images;
        private static FontManager _fonts;

        public static void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing Asset Manager...");

            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            _images = new();
            _images.Initialize();
            _fonts = new();
            _fonts.Initialize();

            Log.WriteLine(LogLevel.Info, "Asset Manager initialized successfully.");
        }

        public static void Dispose()
        {
            Log.WriteLine(LogLevel.Info, "Unloading Asset Manager...");

            _fonts?.Dispose();
            _fonts = null;
            _images?.Dispose();
            _images = null;

            Log.WriteLine(LogLevel.Info, "Asset Manager unloaded successfully.");
        }
    }
}
