namespace AdAstra.Engine
{
    internal static  class FPS
    {
        public static int Current => _current;

        private static int _current;
        private static int _frameCount;
        private static float _elapsedTime;

        public static void Update()
        {
            _elapsedTime += Time.Delta;
            _frameCount++;
            
            if (_elapsedTime >= 1.0f)
            {
                _current = _frameCount;
                _frameCount = 0;
                _elapsedTime -= 1.0f;
            }
        }
    }
}
