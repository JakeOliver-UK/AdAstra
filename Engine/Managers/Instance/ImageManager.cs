using AdAstra.Engine.Managers.Global;
using AdAstra.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdAstra.Engine.Managers.Instance
{
    internal class ImageManager : IDisposable
    {
        public static string DirectoryPath => Path.Combine(AssetManager.DirectoryPath, "Images");
        public Texture2D Pixel => _pixel;

        private Texture2D _pixel;
        private Dictionary<string, Texture2D> _images;

        public void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing Image Manager...");
            
            _images = [];
            if (!Directory.Exists(DirectoryPath)) Directory.CreateDirectory(DirectoryPath);

            LoadDirectory(DirectoryPath);

            _pixel = new(App.Instance.GraphicsDevice, 1, 1);
            _pixel.SetData([Color.White]);

            Log.WriteLine(LogLevel.Info, "Image Manager initialized successfully.");
        }

        public void LoadDirectory(string path, bool canOverride = false)
        {
            Log.WriteLine(LogLevel.Info, $"Loading images from directory '{path}' to Image Manager...");

            if (string.IsNullOrWhiteSpace(path))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to load images from directory '{path}' to Image Manager as path cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }

            if (!Directory.Exists(path))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to load images from directory '{path}' to Image Manager as directory does not exist.");
                return;
            }

            string[] extensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".tif", ".tiff", ".dds"];
            string[] files = [.. Directory.GetFiles(path).Where(file => extensions.Contains(Path.GetExtension(file).ToLowerInvariant()))];

            if (files.Length == 0)
            {
                Log.WriteLine(LogLevel.Warning, $"Unable to load images from directory '{path}' to Image Manager as no valid image files found in directory.");
                return;
            }

            for (int i = 0; i < files.Length; i++)
            {
                string fileName = Path.GetFileNameWithoutExtension(files[i]);
                
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    Log.WriteLine(LogLevel.Warning, $"Unable to load image '{files[i]}' to Image Manager as it has no valid name.");
                    continue;
                }

                try
                {
                    Texture2D image = Texture2D.FromFile(App.Instance.GraphicsDevice, files[i]);

                    Color[] data = new Color[image.Width * image.Height];
                    
                    for (int j = 0; j < data.Length; j++)
                    {
                        data[j] = Color.FromNonPremultiplied(data[j].R, data[j].G, data[j].B, data[j].A);
                    }
                    
                    image.SetData(data);    

                    Add(fileName, image, canOverride);
                    Log.WriteLine(LogLevel.Info, $"Image '{fileName}' loaded to Image Manager successfully from file '{files[i]}'.");
                }
                catch (Exception ex)
                {
                    Log.WriteLine(LogLevel.Error, $"Failed to load image '{fileName}' to Image Manager from file '{files[i]}': {ex.Message}");
                    continue;
                }
            }
        }

        public void Add(string name, Texture2D image, bool canOverride = false)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add image '{name}' to Image Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }
            
            if (image == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add image '{name}' to Image Manager as specified image is null.");
                return;
            }
            
            if (_images.ContainsKey(name) && !canOverride)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to add image '{name}' to Image Manager as an image with that name already exists.");
                return;
            }
            
            if (_images.ContainsKey(name) && canOverride)
            {
                _images[name] = image;
                return;
            }

            _images.Add(name, image);
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove image '{name}' from Image Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return;
            }
            
            if (!_images.ContainsKey(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to remove image '{name}' from Image Manager as no image with that name exists.");
                return;
            }
            
            _images.Remove(name);
        }

        public Texture2D Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get image '{name}' from Image Manager as name cannot be null, empty or consist exclusively of white-space characters.");
                return null;
            }
            
            if (!_images.TryGetValue(name, out Texture2D image))
            {
                Log.WriteLine(LogLevel.Error, $"Unable to get image '{name}' from Image Manager as no image with that name exists.");
                return null;
            }

            return image;
        }

        public void Dispose()
        {
            Log.WriteLine(LogLevel.Info, "Unloading Image Manager...");

            Texture2D[] images = [.. _images.Values];

            for (int i = 0; i < images.Length; i++)
            {
                images[i]?.Dispose();
            }

            _images.Clear();
            _images = null;

            Log.WriteLine(LogLevel.Info, "Image Manager unloaded successfully.");
        }

        public Texture2D this[string name] => Get(name);
        public Texture2D[] GetAll() => [.. _images.Values];
        public bool Contains(string name) => _images.ContainsKey(name);
        public int Count => _images.Count;
    }
}
