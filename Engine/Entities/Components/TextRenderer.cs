using AdAstra.Engine.Managers.Global;
using FontStashSharp;
using FontStashSharp.RichText;
using Microsoft.Xna.Framework;

namespace AdAstra.Engine.Entities.Components
{
    internal class TextRenderer : Component
    {
        public string Text { get; set; } = string.Empty;
        public string Font { get; set; } = "DefaultFont";
        public float Size { get; set; } = 16.0f;
        public Color Color { get; set; } = Color.White;
        public float Opacity { get; set; } = 1.0f;
        public TextAlignment Alignment { get; set; } = TextAlignment.TopLeft;
        public TextHorizontalAlignment HorizontalAlignment { get; set; } = TextHorizontalAlignment.Left;
        public Vector2 Offset { get; set; } = Vector2.Zero;
        public int Layer { get; set; } = 0;
        public bool IsVisible { get; set; } = true;

        public Vector2 Origin
        {
            get
            {
                if (string.IsNullOrEmpty(Text)) return Vector2.Zero;
                SpriteFontBase font = AssetManager.Fonts.Get(Font, Size);
                if (font == null) return Vector2.Zero;
                Vector2 size = font.MeasureString(Text);
                return Alignment switch
                {
                    TextAlignment.TopLeft => Vector2.Zero,
                    TextAlignment.TopCenter => new Vector2(size.X / 2.0f, 0.0f),
                    TextAlignment.TopRight => new Vector2(size.X, 0.0f),
                    TextAlignment.CenterLeft => new Vector2(0, size.Y / 2f),
                    TextAlignment.Center => new Vector2(size.X / 2.0f, size.Y / 2.0f),
                    TextAlignment.CenterRight => new Vector2(size.X, size.Y / 2.0f),
                    TextAlignment.BottomLeft => new Vector2(0, size.Y),
                    TextAlignment.BottomCenter => new Vector2(size.X / 2.0f, size.Y),
                    TextAlignment.BottomRight => new Vector2(size.X, size.Y),
                    _ => Vector2.Zero,
                };
            }
        }

        public override void Draw()
        {
            if (string.IsNullOrEmpty(Text) || !IsVisible) return;
            SpriteFontBase font = AssetManager.Fonts.Get(Font, Size);
            if (font == null) return;
            float layerDepth = Layer / 10000.0f;
            Color color = Color * Opacity;
            RichTextLayout rtl = new()
            {
                Font = font,
                Text = Text,
            };

            rtl.Draw(App.Instance.SpriteBatch, Entity.Transform.Position + Offset, color, Entity.Transform.Rotation, Origin, null, layerDepth, HorizontalAlignment);
        }
    }

    internal enum TextAlignment
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
