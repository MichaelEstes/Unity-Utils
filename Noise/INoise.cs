using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INoise
{
    float Noise(float x, float y);

    void Dispose() { }
}
