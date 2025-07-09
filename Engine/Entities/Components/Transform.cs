using Microsoft.Xna.Framework;
using System;

namespace AdAstra.Engine.Entities.Components
{
    internal class Transform : Component
    {
        public Vector2 Position
        { 
            get
            {
                if (Entity.IsRoot) return LocalPosition;
                else
                {
                    return Entity.Parent.GetComponent<Transform>().Position + Vector2.Transform(LocalPosition, Matrix.CreateRotationZ(Entity.Parent.GetComponent<Transform>().Rotation));
                }
            }
            set
            {
                if (Entity.IsRoot) LocalPosition = value;
                else
                {
                    LocalPosition = Vector2.Transform(value - Entity.Parent.GetComponent<Transform>().Position, Matrix.CreateRotationZ(-Entity.Parent.GetComponent<Transform>().Rotation));
                }
            }
        }

        public Vector2 LocalPosition { get; set; } = Vector2.Zero;
        
        public float Rotation
        {
            get
            {
                if (Entity.IsRoot) return LocalRotation;
                else
                {
                    return Entity.Parent.GetComponent<Transform>().Rotation + LocalRotation;
                }
            }
            set
            {
                if (Entity.IsRoot) LocalRotation = value;
                else
                {
                    LocalRotation = value - Entity.Parent.GetComponent<Transform>().Rotation;
                }
            }
        }

        public float LocalRotation { get; set; } = 0.0f;

        public Vector2 Forward => new (MathF.Cos(Rotation - MathF.PI / 2), MathF.Sin(Rotation - MathF.PI / 2));
        public Vector2 Backward => -Forward;
        public Vector2 Right => new(MathF.Cos(Rotation), MathF.Sin(Rotation));
        public Vector2 Left => -Right;
    }
}
