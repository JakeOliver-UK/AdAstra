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
        private Entity _uiText;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;

            _player = CreateSpaceship("Player", "ship_A", new Vector2(100.0f, 100.0f), Color.Lime, true);
            
            _ai = CreateSpaceship("AI", "ship_A", new Vector2(300.0f, 400.0f), Color.Cyan);

            _uiText = OverlayEntityManager.Create("UIText");
            _uiText.Transform.Position = new(15.0f, 15.0f);
            _uiText.AddComponent<TextRenderer>().Text = $"FPS: {FPS.Current:n0}" +
                $"{Environment.NewLine}X: {_player.Transform.Position.X:n2}" +
                $"{Environment.NewLine}Y: {_player.Transform.Position.Y:n2}";

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
        }

        private Entity CreateSpaceship(string name, string image, Vector2 position, Color color, bool isPlayer = false)
        {
            Entity main = WorldEntityManager.Create($"{name}_main");
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
            
            Entity label = WorldEntityManager.Create($"{name}_label");
            label.SetParent(main);
            label.Transform.AlwaysUseLocalRotation = true;
            label.AddComponent<TextRenderer>().Text = name;
            label.GetComponent<TextRenderer>().Color = color;
            label.GetComponent<TextRenderer>().Layer = main.GetComponent<ImageRenderer>().Layer + 100;
            label.GetComponent<TextRenderer>().Alignment = TextAlignment.CenterLeft;
            label.GetComponent<TextRenderer>().Offset = new Vector2(16.0f, 0.0f);

            return main;
        }
    }
}
