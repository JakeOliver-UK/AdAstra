using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace AdAstra.Engine.Entities.Components
{
    internal class Camera : Component
    {
        public Matrix TransformMatrix => _transformMatrix;
        public Matrix InverseMatrix => _inverseMatrix;
        public float Zoom { get => _zoom; set => _zoom = MathF.Max(0.1f, value); }
        public bool IsKeyboardControlled { get; set; } = false;
        public Keys KeyboardUp { get; set; } = Keys.Up;
        public Keys KeyboardDown { get; set; } = Keys.Down;
        public Keys KeyboardLeft { get; set; } = Keys.Left;
        public Keys KeyboardRight { get; set; } = Keys.Right;
        public bool IsMouseControlled { get; set; } = false;
        public float Speed { get; set; } = 100.0f;
        public float EdgeMargin { get; set; } = 30.0f;
        public bool IsFollowingTarget { get; set; } = false;
        public Entity Target { get; set; } = null;

        private float _zoom = 1.0f;
        private Matrix _transformMatrix;
        private Matrix _inverseMatrix;

        public Vector2 ScreenToWorld(Vector2 screenPosition) => Vector2.Transform(screenPosition, _inverseMatrix);
        public Vector2 WorldToScreen(Vector2 worldPosition) => Vector2.Transform(worldPosition, _transformMatrix);

        public override void Update()
        {
            base.Update();

            _transformMatrix = Matrix.CreateTranslation(new Vector3(-Entity.Transform.Position, 0.0f)) *
                                Matrix.CreateRotationZ(Entity.Transform.Rotation) *
                                Matrix.CreateScale(Zoom) *
                                Matrix.CreateTranslation(new Vector3(App.Instance.GraphicsDevice.Viewport.Width / 2.0f, App.Instance.GraphicsDevice.Viewport.Height / 2.0f, 0.0f));

            _inverseMatrix = Matrix.Invert(_transformMatrix);

            if (IsKeyboardControlled)
            {
                Vector2 movement = Vector2.Zero;
                if (InputManager.IsKeyDown(KeyboardUp)) movement.Y -= Speed * Time.Delta;
                if (InputManager.IsKeyDown(KeyboardDown)) movement.Y += Speed * Time.Delta;
                if (InputManager.IsKeyDown(KeyboardLeft)) movement.X -= Speed * Time.Delta;
                if (InputManager.IsKeyDown(KeyboardRight)) movement.X += Speed * Time.Delta;
                Entity.Transform.Position += movement;
            }

            if (IsMouseControlled)
            {
                Vector2 mousePosition = InputManager.MousePosition;
                if (mousePosition.Y < EdgeMargin) Entity.Transform.Position += Entity.Transform.Forward * Speed * Time.Delta;
                if (mousePosition.Y > App.Instance.GraphicsDevice.Viewport.Height - EdgeMargin) Entity.Transform.Position += Entity.Transform.Backward * Speed * Time.Delta;
                if (mousePosition.X < EdgeMargin) Entity.Transform.Position += Entity.Transform.Left * Speed * Time.Delta;
                if (mousePosition.X > App.Instance.GraphicsDevice.Viewport.Width - EdgeMargin) Entity.Transform.Position += Entity.Transform.Right * Speed * Time.Delta;
            }

            if (IsFollowingTarget && Target != null)
            {
                Vector2 targetPosition = Target.Transform.Position;
                Vector2 direction = targetPosition - Entity.Transform.Position;
                if (direction.LengthSquared() > 0.01f)
                {
                    Entity.Transform.Position += Vector2.Normalize(direction) * Speed * Time.Delta;
                }
            }
        }
    }
}
