using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AdAstra.Engine.Managers.Global
{
    internal static class InputManager
    {
        public static KeyboardState CurrentKeyboardState => _currentKeyboardState;
        public static KeyboardState PreviousKeyboardState => _previousKeyboardState;
        public static MouseState CurrentMouseState => _currentMouseState;
        public static MouseState PreviousMouseState => _previousMouseState;
        public static GamePadState CurrentGamePadState => _currentGamePadState;
        public static GamePadState PreviousGamePadState => _previousGamePadState;

        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;
        private static MouseState _currentMouseState;
        private static MouseState _previousMouseState;
        private static GamePadState _currentGamePadState;
        private static GamePadState _previousGamePadState;

        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            _previousGamePadState = _currentGamePadState;
            _currentGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsKeyDown(Keys key) => _currentKeyboardState.IsKeyDown(key);
        public static bool IsKeyUp(Keys key) => _currentKeyboardState.IsKeyUp(key);
        public static bool IsKeyPressed(Keys key) => _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        public static bool IsKeyReleased(Keys key) => _currentKeyboardState.IsKeyUp(key) && _previousKeyboardState.IsKeyDown(key);

        public static bool IsMouseButtonDown(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Pressed,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Pressed,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Pressed,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Pressed,
                _ => false
            };
        }

        public static bool IsMouseButtonUp(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Released,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Released,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Released,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Released,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Released,
                _ => false
            };
        }

        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Pressed && _previousMouseState.RightButton == ButtonState.Released,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Pressed && _previousMouseState.MiddleButton == ButtonState.Released,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Pressed && _previousMouseState.XButton1 == ButtonState.Released,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Pressed && _previousMouseState.XButton2 == ButtonState.Released,
                _ => false
            };
        }

        public static bool IsMouseButtonReleased(MouseButton button)
        {
            return button switch
            {
                MouseButton.Left => _currentMouseState.LeftButton == ButtonState.Released && _previousMouseState.LeftButton == ButtonState.Pressed,
                MouseButton.Right => _currentMouseState.RightButton == ButtonState.Released && _previousMouseState.RightButton == ButtonState.Pressed,
                MouseButton.Middle => _currentMouseState.MiddleButton == ButtonState.Released && _previousMouseState.MiddleButton == ButtonState.Pressed,
                MouseButton.XButton1 => _currentMouseState.XButton1 == ButtonState.Released && _previousMouseState.XButton1 == ButtonState.Pressed,
                MouseButton.XButton2 => _currentMouseState.XButton2 == ButtonState.Released && _previousMouseState.XButton2 == ButtonState.Pressed,
                _ => false
            };
        }

        public static bool IsMouseScrollWheelUp() => _currentMouseState.ScrollWheelValue > _previousMouseState.ScrollWheelValue;
        public static bool IsMouseScrollWheelDown() => _currentMouseState.ScrollWheelValue < _previousMouseState.ScrollWheelValue;
        public static Vector2 MousePosition => new(_currentMouseState.X, _currentMouseState.Y);

        public static bool IsGamePadButtonDown(Buttons button) => _currentGamePadState.IsButtonDown(button);
        public static bool IsGamePadButtonUp(Buttons button) => _currentGamePadState.IsButtonUp(button);
        public static bool IsGamePadButtonPressed(Buttons button) => _currentGamePadState.IsButtonDown(button) && _previousGamePadState.IsButtonUp(button);
        public static bool IsGamePadButtonReleased(Buttons button) => _currentGamePadState.IsButtonUp(button) && _previousGamePadState.IsButtonDown(button);
        public static Vector2 GamePadLeftThumbstick => _currentGamePadState.ThumbSticks.Left;
        public static Vector2 GamePadRightThumbstick => _currentGamePadState.ThumbSticks.Right;
        public static float GamePadLeftTrigger => _currentGamePadState.Triggers.Left;
        public static float GamePadRightTrigger => _currentGamePadState.Triggers.Right;
        public static bool IsGamePadConnected() => _currentGamePadState.IsConnected;
    }

    internal enum MouseButton
    {
        Left,
        Right,
        Middle,
        XButton1,
        XButton2
    }
}
