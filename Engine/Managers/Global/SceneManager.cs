using AdAstra.Scenes;
using AdAstra.Utils;
using System.Collections.Generic;

namespace AdAstra.Engine.Managers.Global
{
    internal static class SceneManager
    {
        public static Scene Current => _current;

        private static Scene _current;
        private static Dictionary<string, Scene> _scenes;

        public static void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing Scene Manager...");

            _scenes = [];
            _current = null;

            Log.WriteLine(LogLevel.Info, "Scene Manager initialized successfully.");
        }

        public static void Add(string name, Scene scene, bool canOverride = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }

            if (scene == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager as specified scene is null.");
                return;
            }

            if (_scenes.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add scene '{name}' to Scene Manager as a scene with that name already exists.");
                return;
            }

            if (_scenes.ContainsKey(name) && canOverride)
            {
                _scenes[name] = scene;
                return;
            }

            _scenes.Add(name, scene);
        }

        public static void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove scene '{name}' from Scene Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }

            if (!_scenes.ContainsKey(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove scene '{name}' from Scene Manager as no scene with that name exists.");
                return;
            }

            _scenes.Remove(name);
        }

        public static void Switch(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to switch to scene '{name}' as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }

            if (!_scenes.TryGetValue(name, out Scene value))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to switch to scene '{name}' as no scene with that name exists.");
                return;
            }

            _current?.Dispose();
            _current = value;
            _current.Initialize();
        }

        public static Scene Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get scene '{name}' from Scene Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return null;
            }

            if (!_scenes.TryGetValue(name, out Scene value))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get scene '{name}' from Scene Manager as no scene with that name exists.");
                return null;
            }

            return value;
        }

        public static void Dispose()
        {
            Log.WriteLine(LogLevel.Info, "Unloading Scene Manager...");

            _current?.Dispose();
            _current = null;
            _scenes.Clear();
            _scenes = null;

            Log.WriteLine(LogLevel.Info, "Scene Manager unloaded successfully.");
        }

        public static bool Contains(string name) => _scenes.ContainsKey(name);
        public static int Count => _scenes.Count;
    }
}
