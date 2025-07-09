using Microsoft.Xna.Framework;
using System;

namespace AdAstra.Engine.Drawing
{
    internal struct Polygon : IEquatable<Polygon>
    {
        public Vector2 Position { readonly get => _position; set { _position = value; UpdateVertices(); } }
        public int Sides { readonly get => _sides; set { _sides = Math.Max(3, value); UpdateVertices(); } }
        public float Radius { readonly get => _radius; set { _radius = value; UpdateVertices(); } }
        public float Rotation { readonly get => _rotation; set { _rotation = value; UpdateVertices(); } }
        public readonly Vector2[] Vertices => _vertices;

        private Vector2 _position;
        private int _sides;
        private float _radius;
        private float _rotation;
        private readonly Vector2[] _vertices;

        public Polygon(Vector2 position, int sides, float radius, float rotation)
        {
            _position = position;
            _sides = Math.Max(3, sides);
            _radius = radius;
            _rotation = rotation;
            _vertices = new Vector2[_sides];
            UpdateVertices();
        }

        private readonly void UpdateVertices()
        {
            float angleStep = MathHelper.TwoPi / _sides;
            for (int i = 0; i < _sides; i++)
            {
                float angle = _rotation + i * angleStep;
                _vertices[i] = new Vector2(
                    _position.X + (float)Math.Cos(angle) * _radius,
                    _position.Y + (float)Math.Sin(angle) * _radius
                );
            }
        }

        public readonly bool Intersects(Polygon other) => !HasSeparatingAxis(_vertices, other.Vertices) && !HasSeparatingAxis(other.Vertices, _vertices);

        public readonly bool Contains(Vector2 point)
        {
            bool inside = false;
            int j = _sides - 1;

            for (int i = 0; i < _sides; i++)
            {
                Vector2 vi = _vertices[i];
                Vector2 vj = _vertices[j];

                if ((vi.Y > point.Y) != (vj.Y > point.Y) && point.X < (vj.X - vi.X) * (point.Y - vi.Y) / (vj.Y - vi.Y) + vi.X)
                {
                    inside = !inside;
                }

                j = i;
            }
            return inside;
        }

        private static bool HasSeparatingAxis(Vector2[] a, Vector2[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                Vector2 p1 = a[i];
                Vector2 p2 = a[(i + 1) % a.Length];
                Vector2 edge = p2 - p1;
                Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                Project(axis, a, out float minA, out float maxA);
                Project(axis, b, out float minB, out float maxB);

                if (maxA < minB || maxB < minA) return true;
            }
            return false;
        }

        private static void Project(Vector2 axis, Vector2[] verts, out float min, out float max)
        {
            min = max = Vector2.Dot(axis, verts[0]);
            for (int i = 1; i < verts.Length; i++)
            {
                float proj = Vector2.Dot(axis, verts[i]);
                if (proj < min) min = proj;
                if (proj > max) max = proj;
            }
        }

        public readonly bool Equals(Polygon other)
        {
            if (_sides == other.Sides && _radius == other.Radius && _rotation == other.Rotation) return true;
            return false;
        }

        public override readonly bool Equals(object obj) => obj is Polygon polygon && Equals(polygon);
        public override readonly int GetHashCode() => HashCode.Combine(_position, _sides, _radius, _rotation);
    }
}
