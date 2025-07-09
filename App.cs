using AdAstra.Engine;
using AdAstra.Engine.Managers.Global;
using AdAstra.Scenes;
using AdAstra.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdAstra
{
    internal class App : Game
    {
        public static App Instance => _instance;
        public GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public SpriteBatch SpriteBatch => _spriteBatch;

        private static App _instance;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        public App()
        {
            _instance = this;
            _graphicsDeviceManager = new(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing application...");

            base.Initialize();

            AssetManager.Initialize();

            SettingsManager.Initialize();
            
            SceneManager.Initialize();
            SceneManager.Add("MainScene", new MainScene());
            SceneManager.Switch("MainScene");

            Log.WriteLine(LogLevel.Info, "Application initialized successfully.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            FPS.Update();
            Window.Title = SettingsManager.Settings.ShowFPS ? $"{AppInfo.Name} v{AppInfo.Version} - FPS: {FPS.Current:n0}" : $"{AppInfo.Name} v{AppInfo.Version}";

            SceneManager.Current?.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Current?.Draw();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Log.WriteLine(LogLevel.Info, "Shutting down application...");
            
            _spriteBatch?.Dispose();
            _spriteBatch = null;

            SettingsManager.Dispose();
            SceneManager.Dispose();
            AssetManager.Dispose();

            Log.WriteLine(LogLevel.Info, "Application shut down successfully.");
        }
    }
}
