using AdAstra.Engine.Extensions;
using AdAstra.Engine.Managers.Global;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace AdAstra.Engine.Entities.Components
{
    internal class Spaceship : Component
    {
        public float Speed { get; set; } = 100.0f;
        public float TurnSpeed { get; set; } = 1.0f;
        public float ArrivalThreshold { get; set; } = 5.0f;
        public SpaceshipControlType ControlType { get; set; } = SpaceshipControlType.None;
        public Keys KeyboardForward { get; set; } = Keys.W;
        public Keys KeyboardLeft { get; set; } = Keys.A;
        public Keys KeyboardRight { get; set; } = Keys.D;
        public Vector2 Target { get; set; } = Vector2.Zero;
        public List<Vector2> NextTargets { get; set; } = [];
        public bool DrawTargetLine { get; set; } = true;
        public Color TargetLineColor { get; set; } = Color.Lime;
        public float TargetLineOpacity { get; set; } = 0.75f;
        public float TargetLineThickness { get; set; } = 1.0f;
        
        private Vector2 _velocity = Vector2.Zero;

        public override void Update()
        {
            base.Update();

            if (!Entity.HasComponent<Rigidbody>()) return;

            if (ControlType == SpaceshipControlType.Keyboard)
            {
                if (InputManager.IsKeyDown(KeyboardForward)) Entity.GetComponent<Rigidbody>().AddForce(Speed);
                if (InputManager.IsKeyDown(KeyboardLeft)) Entity.GetComponent<Rigidbody>().AddTorque(-TurnSpeed);
                if (InputManager.IsKeyDown(KeyboardRight)) Entity.GetComponent<Rigidbody>().AddTorque(TurnSpeed);
            }
            else if (ControlType == SpaceshipControlType.Mouse)
            {
                Vector2 mousePosition = SceneManager.Current.Camera.ScreenToWorld(InputManager.MousePosition);
                
                if (InputManager.IsMouseButtonPressed(MouseButton.Left))
                {
                    if (Target != Vector2.Zero) _velocity *= 0.5f;
                    Target = mousePosition;
                }

                if (Target != Vector2.Zero)
                {
                    Vector2 pos = Entity.Transform.Position;
                    Vector2 toTarget = Target - pos;
                    float dist = toTarget.Length();

                    if (dist <= ArrivalThreshold)
                    {
                        _velocity = Vector2.Zero;
                        Target = Vector2.Zero;
                        return;
                    }

                    float desiredAngle = MathF.Atan2(toTarget.Y, toTarget.X);
                    float current = Entity.Transform.Rotation;
                    float delta = WrapAngle(desiredAngle - current);
                    float maxDelta = TurnSpeed * Time.Delta;
                    
                    if (MathF.Abs(delta) < maxDelta) current = desiredAngle;
                    else current += MathF.Sign(delta) * maxDelta;

                    Entity.Transform.Rotation = current;

                    if (MathF.Abs(delta) < 0.1f)
                    {
                        float targetSpeed = Speed;
                        float decelerationRange = Speed / 2.0f;
                        if (dist < decelerationRange) targetSpeed *= dist / decelerationRange;

                        Vector2 forward = new(MathF.Cos(current), MathF.Sin(current));
                        Vector2 desiredVel = forward * targetSpeed;

                        float t = MathF.Min(1.0f, Speed * Time.Delta / targetSpeed);
                        _velocity = Vector2.Lerp(_velocity, desiredVel, t);
                    }

                    Entity.Transform.Position += _velocity * Time.Delta;
                }
            }
        }

        private static float WrapAngle(float angle)
        {
            while (angle <= -MathF.PI) angle += MathHelper.TwoPi;
            while (angle > MathF.PI) angle -= MathHelper.TwoPi;
            return angle;
        }

        public override void Draw()
        {
            base.Draw();
            
            if (ControlType == SpaceshipControlType.Mouse && DrawTargetLine && Target != Vector2.Zero)
            {
                Color targetLineColor = TargetLineColor * TargetLineOpacity;
                Vector2 position = Entity.Transform.Position;
                Vector2 directionToTarget = Target - position;
                float distanceToTarget = directionToTarget.Length();
                if (distanceToTarget > 0f)
                {
                    directionToTarget.Normalize();
                    Vector2 endPoint = position + directionToTarget * distanceToTarget;
                    App.Instance.SpriteBatch.DrawLine(position, endPoint, targetLineColor, TargetLineThickness, 10);
                }
            }
        }
    }

    internal enum SpaceshipControlType
    {
        None,
        Keyboard,
        Mouse
    }
}
