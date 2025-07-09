using Microsoft.Xna.Framework;
using System;

namespace AdAstra.Engine.Entities.Components
{
    internal class Rigidbody : Component
    {
        public Vector2 Velocity => _velocity;
        public float AngularVelocity => _angularVelocity;
        public float LinearDrag { get; set; } = 0.5f;
        public float AngularDrag { get; set; } = 0.05f;
        public float Mass { get; set; } = 1.0f;
        public float MaxSpeed { get; set; } = 100.0f;
        public float MaxAngularSpeed { get; set; } = 1.0f;

        private Vector2 _velocity;
        private float _angularVelocity;
        private Vector2 _forceAccumulator;
        private float _torqueAccumulator;

        public override void Update()
        {
            base.Update();

            _velocity += (_forceAccumulator / Mass) * Time.Delta;
            _forceAccumulator = Vector2.Zero;
            float speedSq = _velocity.LengthSquared();
            float maxSpeedSq = MaxSpeed * MaxSpeed;
            if (speedSq > maxSpeedSq && maxSpeedSq > 0f) _velocity = Vector2.Normalize(_velocity) * MaxSpeed;
            float linearDragFactor = MathF.Pow(LinearDrag, Time.Delta);
            _velocity *= linearDragFactor;
            Entity.Transform.Position += _velocity * Time.Delta;
            
            _angularVelocity += (_torqueAccumulator / Mass) * Time.Delta;
            _torqueAccumulator = 0f;
            if (MathF.Abs(_angularVelocity) > MaxAngularSpeed && MaxAngularSpeed > 0f) _angularVelocity = MathF.Sign(_angularVelocity) * MaxAngularSpeed;
            float angularDragFactor = MathF.Pow(AngularDrag, Time.Delta);
            _angularVelocity *= angularDragFactor;
            Entity.Transform.Rotation += _angularVelocity * Time.Delta;
        }

        public void AddForce(Vector2 force) => _forceAccumulator += force;
        public void AddForce(Vector2 direction, float magnitude) => _forceAccumulator += Vector2.Normalize(direction) * magnitude;
        public void AddForce(float magnitude) => _forceAccumulator += Entity.Transform.Forward * magnitude;
        public void AddTorque(float torque) => _torqueAccumulator += torque;
        public void SetVelocity(Vector2 velocity) => _velocity = velocity;
        public void SetAngularVelocity(float angularVelocity) => _angularVelocity = angularVelocity;
    }
}

