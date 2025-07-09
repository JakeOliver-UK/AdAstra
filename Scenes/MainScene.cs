using AdAstra.Engine;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;
using Microsoft.Xna.Framework;

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
    }
}
