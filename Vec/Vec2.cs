using UnityEngine;

namespace ScrimVec
{
    public struct Vec2
    {
        private float _1, _2;

        public float x
        {
            get { return _1; }
            set { _1 = value; }
        }

        public float y
        {
            get { return _2; }
            set { _2 = value; }
        }

        public (float, float) xy
        {
            get { return (_1, _2); }
            set { (_1, _2) = value; }
        }

        public float this[int i]
        {
            get
            {
                float[] v = new float[] { _1, _2 };
                return v[i];
            }
        }

        public Vec2 normalized
        {
            get
            {
                return new Vec2(_1, _2) / length;
            }
        }

        public float length
        {
            get
            {
                return Mathf.Sqrt(sqrLength);
            }
        }

        public float sqrLength
        {
            get
            {
                return _1 * _1 + _2 * _2;
            }
        }

        public Vec2(float x)
        {
            _1 = x;
            _2 = x;
        }

        public Vec2(float x, float y)
        {
            _1 = x;
            _2 = y;
        }

        public static float Dot(Vec2 a, Vec2 b)
        {
            return a.x * b.x + a.y * b.y;
        }

        public static Vec2 Abs(Vec2 a)
        {
            return new Vec2(Mathf.Abs(a.x), Mathf.Abs(a.y));
        }

        public static Vec2 one
        {
            get { return new Vec2(1f); }
        }

        public static Vec2 zero
        {
            get { return new Vec2(0f); }
        }

        public static Vec2 operator *(Vec2 a, Vec2 b) => new Vec2(a.x * b.x, a.y * b.y);
        public static Vec2 operator *(Vec2 a, float b) => new Vec2(a.x * b, a.y * b);
        public static Vec2 operator *(float b, Vec2 a) => new Vec2(a.x * b, a.y * b);
        public static Vec2 operator /(Vec2 a, Vec2 b) => new Vec2(a.x / b.x, a.y / b.y);
        public static Vec2 operator /(Vec2 a, float b) => new Vec2(a.x / b, a.y / b);
        public static Vec2 operator /(float b, Vec2 a) => new Vec2(b / a.x, b / a.y);
        public static Vec2 operator +(Vec2 a, Vec2 b) => new Vec2(a.x + b.x, a.y + b.y);
        public static Vec2 operator +(Vec2 a, float b) => new Vec2(a.x + b, a.y + b);
        public static Vec2 operator +(float b, Vec2 a) => new Vec2(a.x + b, a.y + b);
        public static Vec2 operator -(Vec2 a, Vec2 b) => new Vec2(a.x - b.x, a.y - b.y);
        public static Vec2 operator -(Vec2 a, float b) => new Vec2(a.x - b, a.y - b);
        public static Vec2 operator -(float b, Vec2 a) => new Vec2(b - a.x, b - a.y);

        public static implicit operator Vector2(Vec2 vec) => new Vector2(vec.x, vec.y);
        public static implicit operator Vec2(Vector2 vec) => new Vec2(vec.x, vec.y);
        public static implicit operator Vec2((float, float) vec) => new Vec2(vec.Item1, vec.Item2);

        internal void Deconstruct(out float x, out float y)
        {
            x = _1;
            y = _2;
        }

        public override string ToString()
        {
            return "{ " + x + ", " + y + " }";
        }
    }
}