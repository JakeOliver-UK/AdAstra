using AdAstra.Engine.Managers.Instance;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdAstra.Engine
{
    internal abstract class Scene
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public EntityManager WorldEntityManager => _worldEntityManager;
        public EntityManager OverlayEntityManager => _overlayEntityManager;

        private EntityManager _worldEntityManager;
        private EntityManager _overlayEntityManager;

        public virtual void Initialize()
        {
            _worldEntityManager = new(false);
            _overlayEntityManager = new(true);
        }
        
        public virtual void Update()
        {
            _worldEntityManager.Update();
            _overlayEntityManager.Update();
        }
        
        public virtual void Draw()
        {
            App.Instance.GraphicsDevice.Clear(BackgroundColor);

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, null);
            DrawWorld();
            App.Instance.SpriteBatch.End();

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null, null);
            DrawOverlay();
            App.Instance.SpriteBatch.End();
        }

        public virtual void DrawWorld()
        {
            _worldEntityManager.Draw();
        }

        public virtual void DrawOverlay()
        {
            _overlayEntityManager.Draw();
        }

        public virtual void Dispose()
        {
            _worldEntityManager.Dispose();
            _overlayEntityManager.Dispose();
        }
    }
}
