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
        private Entity _ai;
        private Entity _uiText;
        private Entity _uiTooltip;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            _uiTooltip = OverlayEntityManager.Create("UITooltip");
            _uiTooltip.AddComponent<TextRenderer>();

            _player = CreateSpaceship("Player", "ship_A", new Vector2(100.0f, 100.0f), Color.Lime, true);
            
            _ai = CreateSpaceship("AI", "ship_A", new Vector2(300.0f, 400.0f), Color.Cyan);

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

        private Entity CreateSpaceship(string name, string image, Vector2 position, Color color, bool isPlayer = false)
        {
            Entity spaceship = WorldEntityManager.Create(name);
            spaceship.Transform.Position = position;
            spaceship.AddComponent<ImageRenderer>().Image = image;
            spaceship.GetComponent<ImageRenderer>().Color = color;
            spaceship.GetComponent<ImageRenderer>().Scale = new(0.5f);
            spaceship.GetComponent<ImageRenderer>().RotationAdjustment = MathHelper.PiOver2;
            spaceship.GetComponent<ImageRenderer>().Layer = 10;
            if (isPlayer) spaceship.GetComponent<ImageRenderer>().Layer = 20;
            spaceship.AddComponent<Collider>().Sides = 100;
            spaceship.GetComponent<Collider>().Width = 16.0f;
            spaceship.GetComponent<Collider>().Height = 16.0f;
            spaceship.AddComponent<Spaceship>().Color = color;
            if (isPlayer) spaceship.GetComponent<Spaceship>().Controller = SpaceshipController.Player;
            else spaceship.GetComponent<Spaceship>().Controller = SpaceshipController.AI;

            spaceship.AddComponent<MouseEvents>().OnMouseStayConstant += () =>
            {
                _uiTooltip.Transform.Position = new(InputManager.MousePosition.X + 10.0f, InputManager.MousePosition.Y + 10.0f);
                _uiTooltip.GetComponent<TextRenderer>().Text = name;
                _uiTooltip.GetComponent<TextRenderer>().Color = color;
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = true;
            };

            spaceship.GetComponent<MouseEvents>().OnMouseLeave += () =>
            {
                _uiTooltip.GetComponent<TextRenderer>().Text = string.Empty;
                _uiTooltip.GetComponent<TextRenderer>().Color = Color.Transparent;
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = false;
            };

            return spaceship;
        }
    }
}
