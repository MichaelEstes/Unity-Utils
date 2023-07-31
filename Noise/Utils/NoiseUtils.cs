using ScrimVec;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseUtils
{
    public static int FastFloor(float x)
    {
        return x > 0 ? (int)x : (int)x - 1;
    }

   public static float Frac(float v)
    {
        return v - Mathf.Floor(v);
    }

    public static float Rand(float value, float mutator, float seed = 143758.5453f)
    {
        float random = Frac(Mathf.Sin(value + mutator) * seed);
        return random;
    }

    public static float Rand(Vec2 value, Vec2 dotDir)
    {
        Vec2 smallValue = new Vec2(Mathf.Cos(value.x), Mathf.Cos(value.y));
        float random = Vec2.Dot(smallValue, dotDir);
        random = Frac(Mathf.Sin(random) * 143758.5453f);
        return random;
    }

    public static Vec2 RandVec(Vec2 value, Vec2 seedx, Vec2 seedy)
    {
        return new Vec2(
            Rand(value, seedx),
            Rand(value, seedy)
        );
    }
}
