using AdAstra.Engine.Drawing;
using AdAstra.Engine.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace AdAstra.Engine.Entities.Components
{
    internal class Collider : Component
    {
        public int Sides { get => _sides; set => _sides = Math.Max(3, value); }
        public float Width { get => _width; set => _width = MathF.Max(0.0f, value); }
        public float Height { get => _height; set => _height = MathF.Max(0.0f, value); }
        public float AdjustedRotationDegrees { get; set; } = 0.0f;
        public Polygon Bounds => _bounds;
        public bool DrawBounds { get; set; } = false;
        public Color BoundsColor { get; set; } = Color.Lime;
        public int BoundsLayer { get; set; } = 0;

        private Polygon _bounds;
        private int _sides = 100;
        private float _width = 64.0f;
        private float _height = 32.0f;
        private readonly List<Collider> _collisions = [];

        public delegate void CollisionEventHandler(Collider other);

        public event CollisionEventHandler OnCollisionEnter;
        public event CollisionEventHandler OnCollisionStay;
        public event CollisionEventHandler OnCollisionExit;

        public bool IsCollidingWith(Collider other) => _collisions.Contains(other);
        
        public void AddCollision(Collider other)
        {
            if (other == null || other == this || !other.Entity.IsActive) return;
            if (!_collisions.Contains(other)) _collisions.Add(other);
        }

        public void RemoveCollision(Collider other)
        {
            if (other == null || other == this || !other.Entity.IsActive) return;
            _collisions.Remove(other);
        }

        public void CallOnCollisionEnter(Collider other) => OnCollisionEnter?.Invoke(other);
        public void CallOnCollisionStay(Collider other) => OnCollisionStay?.Invoke(other);
        public void CallOnCollisionExit(Collider other) => OnCollisionExit?.Invoke(other);

        public override void Update()
        {
            base.Update();

            UpdateBounds();
            //CheckCollisions();
        }

        public override void Draw()
        {
            base.Draw();

            if (DrawBounds) App.Instance.SpriteBatch.DrawPolygon(_bounds, BoundsColor, 1.0f, BoundsLayer);
        }

        private void UpdateBounds()
        {
            _bounds = new(Entity.Transform.Position, _sides, _width, _height, Entity.Transform.Rotation + MathHelper.ToRadians(AdjustedRotationDegrees));
        }

        /*private void CheckCollisions()
        {
            Entity[] entities = Entity.Manager.GetWithComponent<Collider>();

            for (int i = 0; i < entities.Length; i++)
            {
                if (entities[i] == null || entities[i] == Entity || !entities[i].IsActive) continue;
                if (entities[i].GetComponent<Collider>() == null) continue;
                if (entities[i].GetComponent<Collider>() == this) continue;
                Collider otherCollider = entities[i].GetComponent<Collider>();
                if (otherCollider == null || otherCollider == this || !otherCollider.Entity.IsActive) continue;
                
                if (_bounds.Intersects(otherCollider._bounds))
                {
                    if (!_collisions.Contains(otherCollider))
                    {
                        _collisions.Add(otherCollider);
                        OnCollisionEnter?.Invoke(otherCollider);
                    }
                    else
                    {
                        OnCollisionStay?.Invoke(otherCollider);
                    }
                }
                else
                {
                    if (_collisions.Contains(otherCollider))
                    {
                        OnCollisionExit?.Invoke(otherCollider);
                        _collisions.Remove(otherCollider);
                    }
                }
            }
        }*/
    }
}
