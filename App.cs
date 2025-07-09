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
            Log.WriteLine("Initializing application...");

            base.Initialize();

            Window.Title = $"{AppInfo.Name} v{AppInfo.Version}";

            Log.WriteLine("Application initialized successfully.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Log.WriteLine("Shutting down application...");
            
            _spriteBatch?.Dispose();
            _spriteBatch = null;
            
            Log.WriteLine("Application shut down successfully.");
        }
    }
}
