using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AdAstra.Engine.Entities.Components
{
    internal class ImageRenderer : Component
    {
        public string Image { get; set; }
        public bool IsVisible { get; set; } = true;
        public Color Color { get; set; } = Color.White;
        public float Opacity { get; set; } = 1.0f;
        public Vector2 Scale { get; set; } = Vector2.One;
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public int Layer { get; set; } = 0;

        public override void Draw()
        {
            float layerDepth = Layer / 10000.0f;
            Color color = Color * Opacity;

            Texture2D texture = AssetManager.Images[Image];

            if (texture == null || !IsVisible) return;

            App.Instance.SpriteBatch.Draw(texture, Entity.Transform.Position, null, color, Entity.Transform.Rotation, Origin, Scale, SpriteEffects.None, layerDepth);
        }
    }
}
