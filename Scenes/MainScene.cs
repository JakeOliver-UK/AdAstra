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
        private Entity _other;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;
            
            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100.0f);
            _player.AddComponent<ImageRenderer>().Image = "playerShip1_blue";
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _player.AddComponent<Collider>().DrawBounds = true;
            _player.GetComponent<Collider>().Sides = 100;
            _player.GetComponent<Collider>().Width = 64.0f;
            _player.GetComponent<Collider>().Height = 48.0f;
            _player.GetComponent<Collider>().OnCollisionEnter += (other) =>
            {
                _player.GetComponent<Collider>().BoundsColor = Color.Red;
                other.BoundsColor = Color.Red;
            };
            _player.GetComponent<Collider>().OnCollisionExit += (other) =>
            {
                _player.GetComponent<Collider>().BoundsColor = Color.Lime;
                other.BoundsColor = Color.Lime;
            };

            _other = WorldEntityManager.Create("Other");
            _other.Transform.Position = new(200.0f);
            _other.AddComponent<ImageRenderer>().Image = "playerShip2_red";
            _other.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _other.AddComponent<Collider>().DrawBounds = true;
            _other.GetComponent<Collider>().Sides = 100;
            _other.GetComponent<Collider>().Width = 64.0f;
            _other.GetComponent<Collider>().Height = 48.0f;
        }

        public override void Update()
        {
            base.Update();

            if (InputManager.IsKeyDown(Keys.W)) _player.Transform.Position += _player.Transform.Forward * 100.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.A)) _player.Transform.Rotation -= 1.0f * Time.Delta;
            if (InputManager.IsKeyDown(Keys.D)) _player.Transform.Rotation += 1.0f * Time.Delta;
        }
    }
}
