using AdAstra.Engine.Drawing;
using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace AdAstra.Engine.Extensions
{
    internal static class SpriteBatchExtensions
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness, int layer)
        {
            Texture2D pixel = AssetManager.Images.Pixel;
            Vector2 direction = end - start;
            float length = direction.Length();
            float rotation = MathF.Atan2(direction.Y, direction.X);
            Vector2 origin = new(0f, 0.5f);
            Vector2 scale = new(length, thickness);
            float layerDepth = layer / 10000.0f;

            spriteBatch.Draw(pixel, start, null, color, rotation, origin, scale, SpriteEffects.None, layerDepth);
        }

        public static void DrawPolygon(this SpriteBatch spriteBatch, Polygon polygon, Color color, float thickness, int layer)
        {
            Vector2[] vertices = polygon.Vertices;

            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 current = vertices[i];
                Vector2 next = vertices[(i + 1) % vertices.Length];
                spriteBatch.DrawLine(current, next, color, thickness, layer);
            }
        }
    }
}
