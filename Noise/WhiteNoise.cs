using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteNoise : INoise
{
    public float Noise(float x, float y)
    {
        return Random.value;
    }
}
