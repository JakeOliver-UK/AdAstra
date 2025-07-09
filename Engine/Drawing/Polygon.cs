using Microsoft.Xna.Framework;
using System;

namespace AdAstra.Engine.Drawing
{
    internal struct Polygon : IEquatable<Polygon>
    {
        public Vector2 Position { readonly get => _position; set { _position = value; UpdateVertices(); } }
        public int Sides { readonly get => _sides; set { _sides = Math.Max(3, value); UpdateVertices(); } }
        public float Width { readonly get => _width; set { _width = value; UpdateVertices(); } }
        public float Height { readonly get => _height; set { _height = value; UpdateVertices(); } }
        public float Rotation { readonly get => _rotation; set { _rotation = value; UpdateVertices(); } }
        public readonly Vector2[] Vertices => _vertices;

        private Vector2 _position;
        private int _sides;
        private float _width;
        private float _height;
        private float _rotation;
        private readonly Vector2[] _vertices;

        public Polygon(Vector2 position, int sides, float width, float height, float rotation)
        {
            _position = position;
            _sides = Math.Max(3, sides);
            _width = width;
            _height = height;
            _rotation = rotation;
            _vertices = new Vector2[_sides];
            UpdateVertices();
        }

        private readonly void UpdateVertices()
        {
            float angleStep = MathHelper.TwoPi / _sides;
            float rx = _width * 0.5f;
            float ry = _height * 0.5f;
            float cosR = MathF.Cos(_rotation);
            float sinR = MathF.Sin(_rotation);

            for (int i = 0; i < _sides; i++)
            {
                float theta = i * angleStep;
                float ex = rx * MathF.Cos(theta);
                float ey = ry * MathF.Sin(theta);

                float x = ex * cosR - ey * sinR;
                float y = ex * sinR + ey * cosR;

                _vertices[i] = new Vector2(_position.X + x, _position.Y + y);
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

                if ((vi.Y > point.Y) != (vj.Y > point.Y) &&
                    point.X < (vj.X - vi.X) * (point.Y - vi.Y) / (vj.Y - vi.Y) + vi.X)
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
            if (verts == null || verts.Length < 2)
            {
                min = max = 0.0f;
                return;
            }

            min = max = Vector2.Dot(axis, verts[0]);
            for (int i = 1; i < verts.Length; i++)
            {
                float proj = Vector2.Dot(axis, verts[i]);
                if (proj < min) min = proj;
                if (proj > max) max = proj;
            }
        }

        public readonly bool Equals(Polygon other) => _sides == other.Sides && _width == other.Width && _height == other.Height && _rotation == other.Rotation;
        public override readonly bool Equals(object obj) => obj is Polygon polygon && Equals(polygon);
        public override readonly int GetHashCode() => HashCode.Combine(_position, _sides, _width, _height, _rotation);
    }
}
