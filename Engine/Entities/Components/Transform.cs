using Microsoft.Xna.Framework;
using System;

namespace AdAstra.Engine.Entities.Components
{
    internal class Transform : Component
    {
        public Vector2 Position
        {
            get => new(WorldMatrix.M41, WorldMatrix.M42);
            set
            {
                if (Entity.IsRoot)
                {
                    LocalPosition = value;
                }
                else
                {
                    Matrix parentWM = Entity.Parent.GetComponent<Transform>().WorldMatrix;
                    Matrix.Invert(ref parentWM, out Matrix invParent);
                    Vector3 local = Vector3.Transform(new Vector3(value, 0f), invParent);
                    LocalPosition = new Vector2(local.X, local.Y);
                }
            }
        }

        public Vector2 LocalPosition { get; set; } = Vector2.Zero;

        public float Rotation
        {
            get => MathF.Atan2(WorldMatrix.M12, WorldMatrix.M11);
            set
            {
                if (Entity.IsRoot || AlwaysUseLocalRotation)
                {
                    LocalRotation = value;
                }
                else
                {
                    float parentRot = Entity.Parent.GetComponent<Transform>().Rotation;
                    LocalRotation = value - parentRot;
                }
            }
        }

        public Matrix LocalMatrix => Matrix.CreateRotationZ(LocalRotation) * Matrix.CreateTranslation(new Vector3(LocalPosition, 0f));

        public Matrix WorldMatrix
        {
            get
            {
                if (Entity.IsRoot) return LocalMatrix;

                Transform parentT = Entity.Parent.GetComponent<Transform>();
                if (AlwaysUseLocalRotation)
                {
                    Vector2 parentPos = parentT.Position;
                    float parentRot = parentT.Rotation;
                    Vector2 worldPos = parentPos + Vector2.Transform(LocalPosition, Matrix.CreateRotationZ(parentRot));
                    return Matrix.CreateRotationZ(LocalRotation) * Matrix.CreateTranslation(new Vector3(worldPos, 0f));
                }
                else
                {
                    return LocalMatrix * parentT.WorldMatrix;
                }
            }
        }

        public float LocalRotation { get; set; } = 0.0f;

        public bool AlwaysUseLocalRotation { get; set; } = false;

        public Vector2 Forward => new (MathF.Cos(Rotation), MathF.Sin(Rotation));
        public Vector2 Backward => -Forward;
        public Vector2 Right => new(MathF.Cos(Rotation - MathF.PI / 2), MathF.Sin(Rotation - MathF.PI / 2));
        public Vector2 Left => -Right;
    }
}
