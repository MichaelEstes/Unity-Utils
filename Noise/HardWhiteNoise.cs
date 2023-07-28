using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardWhiteNoise : INoise
{
    public float Noise(float x, float y)
    {
        return Random.value > 0.5 ? 1 : 0;
    }
}
