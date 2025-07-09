using AdAstra.Engine;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AdAstra.Scenes
{
    internal class MainScene : Scene
    {
        private Entity _player;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;
            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100, 100);
            _player.AddComponent<ImageRenderer>().Image = "playerShip1_blue";
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
        }

        public override void Update()
        {
            base.Update();

            if (InputManager.IsKeyDown(Keys.W)) _player.Transform.Position += new Vector2(0.0f, -1.0f) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.S)) _player.Transform.Position += new Vector2(0.0f, 1.0f) * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.A)) _player.Transform.Rotation -= 1.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.D)) _player.Transform.Rotation += 1.0f * Time.Delta;
        }
    }
}
