using System;
using System.IO;
using System.Reflection;

namespace AdAstra.Utils
{
    internal static class AppInfo
    {
        public static string Name => "Ad Astra";
        public static int VersionMajor => 0;
        public static int VersionMinor => 1;
        public static int VersionPatch => 0;
        public static string Version => $"{VersionMajor}.{VersionMinor}.{VersionPatch}";
        public static string Author => "Guinea Game Studios";
        public static string UserDataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Author, Name);
        public static string ApplicationDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
