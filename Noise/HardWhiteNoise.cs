using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HardWhiteNoise : INoise
{

    private float seed;

    public HardWhiteNoise(float seed)
    {
        this.seed = seed;
    }

    public float Noise(float x, float y)
    {
        return NoiseUtils.Rand(x * y, (x / y) + 23.2232f, seed) > 0.5 ? 1 : 0;
    }
}
