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
        private Entity _child1;
        private Entity _child2;

        public override void Initialize()
        {
            base.Initialize();

            BackgroundColor = Color.Black;
            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100.0f);
            _player.AddComponent<ImageRenderer>().Image = "playerShip1_blue";
            _player.GetComponent<ImageRenderer>().Scale = new(0.5f);
            
            _child1 = WorldEntityManager.Create("Child1");
            _child1.SetParent(_player);
            _child1.Transform.LocalPosition = new(-50.0f, 25.0f);
            _child1.AddComponent<ImageRenderer>().Image = "playerShip2_red";
            _child1.GetComponent<ImageRenderer>().Scale = new(0.25f);

            _child2 = WorldEntityManager.Create("Child2");
            _child2.SetParent(_player);
            _child2.Transform.LocalPosition = new(50.0f, 25.0f);
            _child2.AddComponent<ImageRenderer>().Image = "playerShip3_green";
            _child2.GetComponent<ImageRenderer>().Scale = new(0.25f);
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
