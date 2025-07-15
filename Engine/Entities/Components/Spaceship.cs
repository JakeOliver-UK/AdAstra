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
        public string Name { get; set; } = "Spaceship";
        public Color Color { get; set; } = Color.White;
        public SpaceshipController Controller { get; set; } = SpaceshipController.None;
        public SpaceshipState State { get; set; } = SpaceshipState.Idle;
        public float Speed { get; set; } = 100.0f;
        public float TurnSpeed { get; set; } = 1.0f;
        public float ArrivalThreshold { get; set; } = 5.0f;
        public Keys KeyboardForward { get; set; } = Keys.W;
        public Keys KeyboardLeft { get; set; } = Keys.A;
        public Keys KeyboardRight { get; set; } = Keys.D;
        public Vector2 Target { get; set; } = Vector2.Zero;
        public List<Vector2> NextTargets { get; set; } = [];
        public bool DrawTargetLine { get; set; } = true;
        public float TargetLineOpacity { get; set; } = 0.65f;
        public float TargetLineThickness { get; set; } = 1.0f;

        private Vector2 _velocity = Vector2.Zero;
        private float _timer = 0.0f;

        public override void Update()
        {
            base.Update();

            if (Controller == SpaceshipController.Player) HandlePlayer();
            else if (Controller == SpaceshipController.AI) HandleAI();

            HandleMovement();
        }

        private void HandlePlayer()
        {
            Vector2 mousePosition = SceneManager.Current.Camera.ScreenToWorld(InputManager.MousePosition);

            if (InputManager.IsKeyUp(Keys.LeftShift) && InputManager.IsMouseButtonPressed(MouseButton.Right))
            {
                NextTargets.Clear();
                if (InputManager.MousePosition.X < 0 || InputManager.MousePosition.Y < 0 || InputManager.MousePosition.X > App.Instance.GraphicsDevice.Viewport.Width || InputManager.MousePosition.Y > App.Instance.GraphicsDevice.Viewport.Height) return;
                if (Target != Vector2.Zero || _velocity != Vector2.Zero) _velocity *= 0.5f;
                Target = mousePosition;
            }
            else if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsMouseButtonPressed(MouseButton.Right))
            {
                if (InputManager.MousePosition.X < 0 || InputManager.MousePosition.Y < 0 || InputManager.MousePosition.X > App.Instance.GraphicsDevice.Viewport.Width || InputManager.MousePosition.Y > App.Instance.GraphicsDevice.Viewport.Height) return;
                if (Target == Vector2.Zero) Target = mousePosition;
                else NextTargets.Add(mousePosition);
            }
        }

        private void HandleAI()
        {
            switch (State)
            {
                case SpaceshipState.Idle:
                    _timer += Time.Delta;
                    if (_timer >= 2.0f)
                    {
                        _timer = 0.0f;
                        State = SpaceshipState.LookingForTrades;
                    }
                    break;
                case SpaceshipState.LookingForTrades:
                    Entity[] spaceStations = Entity.Manager.GetWithComponent<SpaceStation>();
                    if (spaceStations.Length > 0)
                    { 
                        float distance = float.MinValue;
                        Vector2 target = Vector2.Zero;
                        for (int i = 0; i < spaceStations.Length; i++)
                        {
                            float thisDistance = Vector2.Distance(Entity.Transform.Position, spaceStations[i].Transform.Position);
                            if (thisDistance > distance)
                            {
                                distance = thisDistance;
                                target = spaceStations[i].Transform.Position;
                            }
                        }
                        if (target != Vector2.Zero)
                        {
                            Target = target;
                            _velocity = Vector2.Zero;
                            State = SpaceshipState.Moving;
                        }
                        else State = SpaceshipState.Idle;
                    }
                    else State = SpaceshipState.Idle;
                    break;
                case SpaceshipState.Moving:
                    if (Target == Vector2.Zero)
                    {
                        State = SpaceshipState.Trading;
                    }
                    break;
                case SpaceshipState.Trading:
                    _timer += Time.Delta;
                    if (_timer >= 2.0f)
                    {
                        _timer = 0.0f;
                        State = SpaceshipState.LookingForTrades;
                    }
                    break;
                default:
                    break;

            }
        }

        private void HandleMovement()
        {
            if (Target != Vector2.Zero)
            {
                Vector2 pos = Entity.Transform.Position;
                Vector2 toTarget = Target - pos;
                float dist = toTarget.Length();

                if (dist <= ArrivalThreshold)
                {
                    if (NextTargets.Count > 0)
                    {
                        Target = NextTargets[0];
                        NextTargets.RemoveAt(0);
                        _velocity *= 0.5f;
                    }
                    else
                    {
                        Target = Vector2.Zero;
                        _velocity = Vector2.Zero;
                        return;
                    }
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

        private static float WrapAngle(float angle)
        {
            while (angle <= -MathF.PI) angle += MathHelper.TwoPi;
            while (angle > MathF.PI) angle -= MathHelper.TwoPi;
            return angle;
        }

        public override void Draw()
        {
            base.Draw();

            if (DrawTargetLine && Target != Vector2.Zero)
            {
                Color targetLineColor = Color * TargetLineOpacity;
                Vector2 position = Entity.Transform.Position;
                Vector2 directionToTarget = Target - position;
                float distanceToTarget = directionToTarget.Length();
                if (distanceToTarget > 0f)
                {
                    directionToTarget.Normalize();
                    Vector2 endPoint = position + directionToTarget * distanceToTarget;
                    App.Instance.SpriteBatch.DrawLine(position, endPoint, targetLineColor, TargetLineThickness, 10);
                }
                if (NextTargets.Count > 0)
                {
                    for (int i = 0; i < NextTargets.Count; i++)
                    {
                        targetLineColor = Color * (TargetLineOpacity * 0.75f);
                        Vector2 nextTarget = NextTargets[i];
                        if (i == 0)
                        {
                            App.Instance.SpriteBatch.DrawLine(Target, nextTarget, targetLineColor, TargetLineThickness, 10);
                        }
                        else if (nextTarget != Vector2.Zero)
                        {
                            App.Instance.SpriteBatch.DrawLine(NextTargets[i - 1], nextTarget, targetLineColor, TargetLineThickness, 10);
                        }
                    }
                }
            }
        }
    }

    internal enum SpaceshipController
    {
        None,
        Player,
        AI
    }

    internal enum SpaceshipState
    {
        Idle,
        LookingForTrades,
        Moving,
        Trading
    }
}
