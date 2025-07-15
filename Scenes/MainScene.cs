using AdAstra.Engine;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using System;

namespace AdAstra.Scenes
{
    internal class MainScene : Scene
    {
        private Entity _player;
        private Entity _ai;
        private Entity _spaceStationA;
        private Entity _spaceStationB;
        private Entity _uiText;
        private Entity _uiTooltip;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            _player = CreateSpaceship("Player", "ship_A", new Vector2(100.0f, 100.0f), Color.Lime, true);

            _ai = CreateSpaceship("AI", "ship_A", new Vector2(300.0f, 400.0f), Color.Cyan);

            _spaceStationA = CreateSpaceStation("Space Station A", "station_A", new Vector2(-100.0f, -100.0f), Color.Yellow);

            _spaceStationB = CreateSpaceStation("Space Station B", "station_A", new Vector2(250.0f, -225.0f), Color.Yellow);

            _uiText = OverlayEntityManager.Create("UIText");
            _uiText.Transform.Position = new(15.0f, 15.0f);
            _uiText.AddComponent<TextRenderer>().Text = $"FPS: {FPS.Current:n0}" +
                $"{Environment.NewLine}X: {_player.Transform.Position.X:n2}" +
                $"{Environment.NewLine}Y: {_player.Transform.Position.Y:n2}";

            _uiTooltip = OverlayEntityManager.Create("UITooltip");
            _uiTooltip.Transform.Position = InputManager.MousePosition;
            _uiTooltip.AddComponent<TextRenderer>().IsVisible = false;
            _uiTooltip.GetComponent<TextRenderer>().Size = 12.0f;

            Camera.IsFollowingTarget = true;
            Camera.IsKeyboardControlled = true;
            Camera.Target = _player;
            Camera.Entity.Transform.Position = _player.Transform.Position;
        }

        public override void Update()
        {
            base.Update();

            _uiText.AddComponent<TextRenderer>().Text = $"FPS: {FPS.Current:n0}" +
                $"{Environment.NewLine}X: {_player.Transform.Position.X:n2}" +
                $"{Environment.NewLine}Y: {_player.Transform.Position.Y:n2}";

            _uiTooltip.Transform.Position = InputManager.MousePosition;
        }

        private Entity CreateSpaceship(string name, string image, Vector2 position, Color color, bool isPlayer)
        {
            string id = Guid.NewGuid().ToString();
            Entity main = WorldEntityManager.Create($"{id}_main");
            main.Transform.Position = position;
            main.AddComponent<ImageRenderer>().Image = image;
            main.GetComponent<ImageRenderer>().Color = color;
            main.GetComponent<ImageRenderer>().Scale = new(0.5f);
            main.GetComponent<ImageRenderer>().RotationAdjustment = MathHelper.PiOver2;
            main.GetComponent<ImageRenderer>().Layer = 10;
            if (isPlayer) main.GetComponent<ImageRenderer>().Layer = 20;
            main.AddComponent<Collider>().Sides = 100;
            main.GetComponent<Collider>().Width = 16.0f;
            main.GetComponent<Collider>().Height = 16.0f;
            main.AddComponent<Spaceship>().Color = color;
            if (isPlayer) main.GetComponent<Spaceship>().Controller = SpaceshipController.Player;
            else main.GetComponent<Spaceship>().Controller = SpaceshipController.AI;

            main.AddComponent<MouseEvents>().OnMouseStayConstant += () =>
            {
                _uiTooltip.GetComponent<TextRenderer>().Text = $"/tu{name}/td/n{main.GetComponent<Spaceship>().GetCargoInfo()}";
                _uiTooltip.GetComponent<TextRenderer>().Color = color;
                _uiTooltip.GetComponent<TextRenderer>().Offset = new(12.0f, 12.0f);
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = true;
            };
            main.GetComponent<MouseEvents>().OnMouseLeave += () =>
            {
                _uiTooltip.GetComponent<TextRenderer>().Text = string.Empty;
                _uiTooltip.GetComponent<TextRenderer>().Color = Color.White;
                _uiTooltip.GetComponent<TextRenderer>().Offset = Vector2.Zero;
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = false;
            };

            return main;
        }

        private Entity CreateSpaceship(string name, string image, Vector2 position, Color color) => CreateSpaceship(name, image, position, color, false);

        private Entity CreateSpaceStation(string name, string image, Vector2 position, Color color)
        {
            string id = Guid.NewGuid().ToString();
            Entity main = WorldEntityManager.Create($"{id}_main");
            main.Transform.Position = position;
            main.AddComponent<ImageRenderer>().Image = image;
            main.GetComponent<ImageRenderer>().Color = color;
            main.GetComponent<ImageRenderer>().Scale = new(0.5f);
            main.GetComponent<ImageRenderer>().RotationAdjustment = MathHelper.PiOver2;
            main.GetComponent<ImageRenderer>().Layer = 5;
            main.AddComponent<Collider>().Sides = 100;
            main.GetComponent<Collider>().Width = 16.0f;
            main.GetComponent<Collider>().Height = 16.0f;
            main.AddComponent<SpaceStation>().Color = color;

            main.AddComponent<MouseEvents>().OnMouseStayConstant += () =>
            {
                _uiTooltip.GetComponent<TextRenderer>().Text = $"/tu{name}/td/n{main.GetComponent<SpaceStation>().GetMarketItemInfo()}";
                _uiTooltip.GetComponent<TextRenderer>().Color = color;
                _uiTooltip.GetComponent<TextRenderer>().Offset = new(12.0f, 12.0f);
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = true;
            };

            main.GetComponent<MouseEvents>().OnMouseLeave += () =>
            {
                _uiTooltip.GetComponent<TextRenderer>().Text = string.Empty;
                _uiTooltip.GetComponent<TextRenderer>().Color = Color.White;
                _uiTooltip.GetComponent<TextRenderer>().Offset = Vector2.Zero;
                _uiTooltip.GetComponent<TextRenderer>().IsVisible = false;
            };

            return main;
        }
    }
}
