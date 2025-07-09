using AdAstra.Engine.Extensions;
using System;
using System.Numerics;

namespace AdAstra.Engine.Maths
{
    internal class PRNG
    {
        public string Seed { get => _seed; set { _seed = value; _random = new(value.ToSeed()); } }

        private string _seed;
        private Random _random;

        public PRNG()
        {
            _seed = DateTime.Now.Ticks.ToString();
            _random = new(_seed.ToSeed());
        }

        public PRNG(string seed)
        {
            if (string.IsNullOrWhiteSpace(seed)) seed = DateTime.Now.Ticks.ToString();
            _seed = seed;
            _random = new(_seed.ToSeed());
        }

        public int Int() => _random.Next();
        public int Int(int max) => _random.Next(max);
        public int Int(int min, int max) => _random.Next(min, max);
        public double Double() => _random.NextDouble();
        public double Double(double max) => _random.NextDouble() * max;
        public double Double(double min, double max) => _random.NextDouble() * (max - min) + min;
        public float Float() => (float)_random.NextDouble();
        public float Float(float max) => (float)_random.NextDouble() * max;
        public float Float(float min, float max) => (float)_random.NextDouble() * (max - min) + min;

        public bool Bool() => _random.Next(0, 2) == 1;

        public char LowerAlphaChar()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public char UpperAlphaChar()
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public char AlphaChar()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public char NumericChar()
        {
            char[] chars = "0123456789".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public char LowerAlphanumericChar()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public char UperAlphanumericChar()
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public char AlphanumericChar()
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            return chars[_random.Next(0, chars.Length)];
        }

        public string LowerAlphaString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public string UpperAlphaString(int length)
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public string AlphaString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public string NumericString(int length)
        {
            char[] chars = "0123456789".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public string LowerAlphanumericString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public string UpperAlphanumericString(int length)
        {
            char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public string AlphanumericString(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            char[] result = new char[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = chars[_random.Next(0, chars.Length)];
            }
            return new string(result);
        }

        public Vector2 Vector2(float minX, float maxX, float minY, float maxY)
        {
            float x = (float)_random.NextDouble() * (maxX - minX) + minX;
            float y = (float)_random.NextDouble() * (maxY - minY) + minY;
            return new(x, y);
        }

        public Vector2 Vector2(float min, float max)
        {
            float x = (float)_random.NextDouble() * (max - min) + min;
            float y = (float)_random.NextDouble() * (max - min) + min;
            return new(x, y);
        }

        public Vector2 Vector2InsideCicle(Vector2 origin, float radius, bool isCenterConcentrated = false)
        {
            float angle = (float)_random.NextDouble() * MathF.PI * 2;
            float r = (float)_random.NextDouble() * radius;
            if (!isCenterConcentrated) r = MathF.Sqrt((float)_random.NextDouble()) * radius;
            float x = origin.X + r * MathF.Cos(angle);
            float y = origin.Y + r * MathF.Sin(angle);
            return new(x, y);
        }

        public Vector2 Vector2OnCircle(Vector2 origin, float radius)
        {
            float angle = (float)_random.NextDouble() * MathF.PI * 2;
            float x = origin.X + radius * MathF.Cos(angle);
            float y = origin.Y + radius * MathF.Sin(angle);
            return new(x, y);
        }

        public Vector2 Vector2InsideRing(Vector2 origin, float innerRadius, float outerRadius)
        {
            float angle = (float)_random.NextDouble() * MathF.PI * 2;
            float r = (float)_random.NextDouble() * (outerRadius - innerRadius) + innerRadius;
            float x = origin.X + r * MathF.Cos(angle);
            float y = origin.Y + r * MathF.Sin(angle);
            return new(x, y);
        }
    }
}
