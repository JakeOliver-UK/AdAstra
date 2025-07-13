using AdAstra.Engine;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;

namespace AdAstra.Scenes
{
    internal class MainScene : Scene
    {
        private Entity _player;
        private Entity _uiText;
        private Entity _uiTooltip;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            _uiTooltip = OverlayEntityManager.Create("UITooltip");
            _uiTooltip.AddComponent<TextRenderer>();

            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100.0f);
            _player.AddComponent<ImageRenderer>().Image = "ship_A";
            _player.AddComponent<ImageRenderer>().Color = Color.Lime;
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
            _player.GetComponent<ImageRenderer>().RotationAdjustment = MathHelper.PiOver2;
            _player.GetComponent<ImageRenderer>().Layer = 1;
            _player.AddComponent<Collider>().Sides = 100;
            _player.GetComponent<Collider>().Width = 16.0f;
            _player.GetComponent<Collider>().Height = 16.0f;
            _player.AddComponent<Spaceship>();
            
            _player.AddComponent<MouseEvents>().OnMouseStayConstant += () =>
            {
                _uiTooltip.Transform.Position = new(InputManager.MousePosition.X + 10.0f, InputManager.MousePosition.Y + 10.0f);
                _uiTooltip.GetComponent<TextRenderer>().Text = "Player";
                _uiTooltip.GetComponent<TextRenderer>().Color = Color.Lime;
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = true;
            };
            
            _player.GetComponent<MouseEvents>().OnMouseLeave += () =>
            {
                _uiTooltip.GetComponent<TextRenderer>().Text = string.Empty;
                _uiTooltip.GetComponent<TextRenderer>().Color = Color.Transparent;
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = false;
            };

            _uiText = OverlayEntityManager.Create("UIText");
            _uiText.Transform.Position = new(15.0f, 15.0f);
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
