using AdAstra.Engine.Managers.Global;
using AdAstra.Utils;
using FontStashSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace AdAstra.Engine.Managers.Instance
{
    internal class FontManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AssetManager.DirectoryPath, "Fonts");

        private Dictionary<string, FontSystem> _fonts;

        public void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing Font Manager...");
            
            _fonts = [];
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);
            LoadDirectory(DirectoryPath);
            
            Log.WriteLine(LogLevel.Info, "Font Manager initialized successfully.");
        }

        public void LoadDirectory(string path, bool canOverride = false)
        {
            Log.WriteLine(LogLevel.Info, $"Loading fonts from directory '{path}' to Font Manager...");
            
            if (string.IsNullOrWhiteSpace(path))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to load fonts from directory '{path}' to Font Manager as path cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }
            
            if (!Directory.Exists(path))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to load fonts from directory '{path}' to Font Manager as directory does not exist.");
                return;
            }
            
            string[] files = Directory.GetFiles(path, "*.ttf");
            
            if (files.Length == 0)
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to load fonts from directory '{path}' to Font Manager as no valid font files found in directory.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Log.WriteLine(LogLevel.Warning, $"Unable to load font from file '{files[i]}' to Font Manager as file name is null, empty or consists exclusively of white-space characters.");
                    continue;
                }

                try
                {
                    FontSystem font = new();
                    font.AddFont(File.ReadAllBytes(files[i]));
                    Add(fileName, font, canOverride);
                    Log.WriteLine(LogLevel.Info, $"Font '{fileName}' loaded to Font Manager successfully from file '{files[i]}'.");
                }
                catch (Exception ex)
                {
                    Log.WriteLine(LogLevel.Error, $"Failed to load image '{fileName}' to Image Manager from file '{files[i]}': {ex.Message}");
                    continue;
                }
            }
        }

        public void Add(string name, FontSystem font, bool canOverride = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add font '{name}' to Font Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }
            
            if (font == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add font '{name}' to Font Manager as specified font is null.");
                return;
            }
            
            if (_fonts.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add font '{name}' to Font Manager as a font with that name already exists.");
                return;
            }
            
            if (_fonts.ContainsKey(name) && canOverride)
            {
                _fonts[name] = font;
                return;
            }
            
            _fonts.Add(name, font);
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove font '{name}' from Font Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }
            
            if (!_fonts.ContainsKey(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove font '{name}' from Font Manager as no font with that name exists.");
                return;
            }
            
            _fonts.Remove(name);
        }

        public SpriteFontBase Get(string name, float size)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get font '{name}' from Font Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return null;
            }
            
            if (!_fonts.TryGetValue(name, out FontSystem value))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get font '{name}' from Font Manager as no font with that name exists.");
                return null;
            }
            
            SpriteFontBase font = value.GetFont(size);
            
            if (font == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get font '{name}' from Font Manager as font with specified size does not exist.");
                return null;
            }

            return font;
        }

        public void Dispose()
        {
            Log.WriteLine(LogLevel.Info, "Unloading Font Manager...");

            FontSystem[] fonts = [.. _fonts.Values];

            for (int i = 0; i < fonts.Length; i++)
            {
                fonts[i]?.Dispose();
            }

            _fonts.Clear();
            _fonts = null;

            Log.WriteLine(LogLevel.Info, "Font Manager unloaded successfully.");
        }

        public SpriteFontBase this[string name, float size] => Get(name, size);
        public FontSystem[] GetAll() => [.. _fonts.Values];
        public bool Contains(string name) => _fonts.ContainsKey(name);
        public int Count => _fonts.Count;
    }
}
