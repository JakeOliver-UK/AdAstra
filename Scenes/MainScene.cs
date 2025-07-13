using AdAstra.Engine;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using Microsoft.Xna.Framework;

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
            _player.AddComponent<ImageRenderer>().Image = "ship_A";
            _player.AddComponent<ImageRenderer>().Color = Color.Lime;
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _player.GetComponent<ImageRenderer>().RotationAdjustment = MathHelper.PiOver2;
            _player.GetComponent<ImageRenderer>().Layer = 1;
            _player.AddComponent<Collider>().Sides = 100;
            _player.GetComponent<Collider>().Width = 64.0f;
            _player.GetComponent<Collider>().Height = 48.0f;
            _player.AddComponent<Spaceship>();

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

            _uiText.GetComponent<TextRenderer>().Text = $"FPS: {FPS.Current:n0}";
        }
    }
}
