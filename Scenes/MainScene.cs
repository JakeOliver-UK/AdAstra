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
        private Entity _other;
        private Entity _uiText;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;
            
            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100.0f);
            _player.AddComponent<ImageRenderer>().Image = "playerShip1_blue";
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _player.GetComponent<ImageRenderer>().Layer = 1;
            _player.AddComponent<Collider>().Sides = 100;
            _player.GetComponent<Collider>().Width = 64.0f;
            _player.GetComponent<Collider>().Height = 48.0f;
            _player.AddComponent<Rigidbody>();

            _other = WorldEntityManager.Create("Other");
            _other.Transform.Position = new(200.0f);
            _other.AddComponent<ImageRenderer>().Image = "playerShip2_red";
            _other.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _other.AddComponent<Collider>().Sides = 100;
            _other.GetComponent<Collider>().Width = 64.0f;
            _other.GetComponent<Collider>().Height = 48.0f;

            _uiText = OverlayEntityManager.Create("UIText");
            _uiText.Transform.Position = new(10.0f, 10.0f);
            _uiText.AddComponent<TextRenderer>().Text = $"FPS: {FPS.Current:n0}";

            Camera.IsFollowingTarget = true;
            Camera.IsKeyboardControlled = true;
            Camera.Target = _player;
            Camera.Entity.Transform.Position = _player.Transform.Position;
        }

        public override void Update()
        {
            base.Update();

            if (InputManager.IsKeyDown(Keys.W)) _player.GetComponent<Rigidbody>().AddForce(100.0f);
            if (InputManager.IsKeyDown(Keys.A)) _player.GetComponent<Rigidbody>().AddTorque(-5.0f);
            if (InputManager.IsKeyDown(Keys.D)) _player.GetComponent<Rigidbody>().AddTorque(5.0f);
            
            _uiText.GetComponent<TextRenderer>().Text = $"FPS: {FPS.Current:n0}";
        }
    }
}
