using Microsoft.Xna.Framework;

namespace AdAstra.Engine
{
    internal static class Time
    {
        public static float Delta => _delta;
        public static float DeltaMS => _deltaMS;

        private static float _delta;
        private static float _deltaMS;

        public static void Update(GameTime gameTime)
        {
            _delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _deltaMS = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }
    }
}
