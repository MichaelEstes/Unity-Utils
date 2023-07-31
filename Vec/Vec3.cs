using UnityEngine;

namespace ScrimVec
{
    public struct Vec3
    {
        private float _1, _2, _3;

        public float x
        {
            get { return _1; }
            set { _1 = value; }
        }

        public float r
        {
            get { return _1; }
            set { _1 = value; }
        }

        public float y
        {
            get { return _2; }
            set { _2 = value; }
        }

        public float g
        {
            get { return _2; }
            set { _2 = value; }
        }

        public float z
        {
            get { return _3; }
            set { _3 = value; }
        }

        public float b
        {
            get { return _3; }
            set { _3 = value; }
        }

        public (float, float) xy
        {
            get { return (_1, _2); }
            set { (_1, _2) = value; }
        }

        public (float, float) rg
        {
            get { return (_1, _2); }
            set { (_1, _2) = value; }
        }

        public (float, float) xz
        {
            get { return (_1, _3); }
            set { (_1, _3) = value; }
        }

        public (float, float) rb
        {
            get { return (_1, _3); }
            set { (_1, _3) = value; }
        }

        public (float, float) yz
        {
            get { return (_2, _3); }
            set { (_2, _3) = value; }
        }

        public (float, float) bg
        {
            get { return (_2, _3); }
            set { (_2, _3) = value; }
        }

        public (float, float, float) xyz
        {
            get { return (_1, _2, _3); }
            set { (_1, _2, _3) = value; }
        }

        public (float, float, float) rgb
        {
            get { return (_1, _2, _3); }
            set { (_1, _2, _3) = value; }
        }

        public float this[int i]
        {
            get
            {
                float[] v = new float[] { _1, _2, _3 };
                return v[i];
            }
        }

        public Vec3 normalized
        {
            get
            {
                return new Vec3(_1, _2, _3) / length;
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
                return _1 * _1 + _2 * _2 + _3 * _3;
            }
        }

        public Vec3(float x)
        {
            _1 = x;
            _2 = x;
            _3 = x;
        }

        public Vec3(float x, float y, float z)
        {
            _1 = x;
            _2 = y;
            _3 = z;
        }

        public static float Dot(Vec3 a, Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vec3 Abs(Vec3 a)
        {
            return new Vec3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
        }

        public static Vec3 one
        {
            get { return new Vec3(1f); }
        }

        public static Vec3 zero
        {
            get { return new Vec3(0f); }
        }

        public static Vec3 operator *(Vec3 a, Vec3 b) => new Vec3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vec3 operator *(Vec3 a, float b) => new Vec3(a.x * b, a.y * b, a.z * b);
        public static Vec3 operator *(float b, Vec3 a) => new Vec3(a.x * b, a.y * b, a.z * b);
        public static Vec3 operator /(Vec3 a, Vec3 b) => new Vec3(a.x / b.x, a.y / b.y, a.z / b.z);
        public static Vec3 operator /(Vec3 a, float b) => new Vec3(a.x / b, a.y / b, a.z / b);
        public static Vec3 operator /(float b, Vec3 a) => new Vec3(b / a.x, b / a.y, b / a.z);
        public static Vec3 operator +(Vec3 a, Vec3 b) => new Vec3(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vec3 operator +(Vec3 a, float b) => new Vec3(a.x + b, a.y + b, a.z + b);
        public static Vec3 operator +(float b, Vec3 a) => new Vec3(a.x + b, a.y + b, a.z + b);
        public static Vec3 operator -(Vec3 a, Vec3 b) => new Vec3(a.x - b.x, a.y - b.y, a.z - b.z);
        public static Vec3 operator -(Vec3 a, float b) => new Vec3(a.x - b, a.y - b, a.z - b);
        public static Vec3 operator -(float b, Vec3 a) => new Vec3(b - a.x, b - a.y, b - a.z);

        public static implicit operator Vector2(Vec3 vec) => new Vector2(vec.x, vec.y);
        public static implicit operator Vec2(Vec3 vec) => new Vec2(vec.x, vec.y);

        public static implicit operator Vector3(Vec3 vec) => new Vector3(vec.x, vec.y, vec.z);
        public static implicit operator Vec3(Vector3 vec) => new Vec3(vec.x, vec.y, vec.z);
        public static implicit operator Vec3((float, float, float) vec) => new Vec3(vec.Item1, vec.Item2, vec.Item3);

        internal void Deconstruct(out float x, out float y, out float z)
        {
            x = _1;
            y = _2;
            z = _3;
        }

        public override string ToString()
        {
            return "{ " + x + ", " + y + ", " + z + " }";
        }
    }
}
