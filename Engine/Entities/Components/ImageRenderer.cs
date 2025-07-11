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
        public ImageAlignment Alignment { get; set; } = ImageAlignment.Center;
        public float RotationAdjustment { get; set; } = 0.0f;
        public int Layer { get; set; } = 0;

        public Vector2 Origin
        {
            get
            {
                if (Image == null) return Vector2.Zero;
                Texture2D texture = AssetManager.Images[Image];
                if (texture == null) return Vector2.Zero;
                return Alignment switch
                {
                    ImageAlignment.TopLeft => Vector2.Zero,
                    ImageAlignment.TopCenter => new Vector2(texture.Width / 2.0f, 0.0f),
                    ImageAlignment.TopRight => new Vector2(texture.Width, 0.0f),
                    ImageAlignment.CenterLeft => new Vector2(0, texture.Height / 2f),
                    ImageAlignment.Center => new Vector2(texture.Width / 2.0f, texture.Height / 2.0f),
                    ImageAlignment.CenterRight => new Vector2(texture.Width, texture.Height / 2.0f),
                    ImageAlignment.BottomLeft => new Vector2(0, texture.Height),
                    ImageAlignment.BottomCenter => new Vector2(texture.Width / 2.0f, texture.Height),
                    ImageAlignment.BottomRight => new Vector2(texture.Width, texture.Height),
                    _ => Vector2.Zero,
                };
            }
        }

        public override void Draw()
        {
            float layerDepth = Layer / 10000.0f;
            Color color = Color * Opacity;
            float adjustedRotation = Entity.Transform.Rotation + RotationAdjustment;

            Texture2D texture = AssetManager.Images[Image];

            if (texture == null || !IsVisible) return;

            App.Instance.SpriteBatch.Draw(texture, Entity.Transform.Position, null, color, adjustedRotation, Origin, Scale, SpriteEffects.None, layerDepth);
        }
    }

    internal enum ImageAlignment
    {
        TopLeft,
        TopCenter,
        TopRight,
        CenterLeft,
        Center,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }
}
