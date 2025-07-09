using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdAstra.Engine
{
    internal abstract class Scene
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

        public virtual void Initialize() { }
        public virtual void Update() { }
        
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

        public virtual void DrawWorld() { }
        public virtual void DrawOverlay() { }
        public virtual void Dispose() { }
    }
}
