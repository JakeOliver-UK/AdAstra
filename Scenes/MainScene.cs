using AdAstra.Engine;
using AdAstra.Engine.Entities;
using AdAstra.Engine.Entities.Components;

namespace AdAstra.Scenes
{
    internal class MainScene : Scene
    {
        private Entity _player;

        public override void Initialize()
        {
            base.Initialize();

            _player = WorldEntityManager.Create("Player");
            _player.Transform.Position = new(100, 100);
            _player.AddComponent<ImageRenderer>().Image = "playerShip1_blue";
        }
    }
}
