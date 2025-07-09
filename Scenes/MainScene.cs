using AdAstra.Engine;
using AdAstra.Engine.Drawing;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using AdAstra.Engine.Extensions;
using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AdAstra.Scenes
{
    internal class MainScene : Scene
    {
        private Entity _player;
        private Polygon _polygon;
        private Color _polygonColor = Color.Lime;
        private Polygon _polygon2;
        private Color _polygon2Color = Color.Cyan;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;
            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100.0f);
            _player.AddComponent<ImageRenderer>().Image = "playerShip1_blue";
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _polygon = new(new Vector2(200.0f), 5, 32.0f, 0.0f);
            _polygon2 = new(InputManager.MousePosition, 3, 16.0f, MathHelper.ToRadians(45.0f));
        }

        public override void Update()
        {
            base.Update();

            if (InputManager.IsKeyDown(Keys.W)) _player.Transform.Position += _player.Transform.Forward * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.A)) _player.Transform.Rotation -= 1.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.D)) _player.Transform.Rotation += 1.0f * Time.Delta;

            _polygon2.Position = InputManager.MousePosition;

            if (_polygon.Intersects(_polygon2))
            {
                _polygonColor = Color.Red;
                _polygon2Color = Color.Yellow;
            }
            else
            {
                _polygonColor = Color.Lime;
                _polygon2Color = Color.Cyan;
            }
        }

        public override void DrawWorld()
        {
            base.DrawWorld();

            App.Instance.SpriteBatch.DrawPolygon(_polygon, _polygonColor, 1.0f, 1);
            App.Instance.SpriteBatch.DrawPolygon(_polygon2, _polygon2Color, 1.0f, 1);
        }
    }
}
