using UnityEngine;


[System.Serializable]
public struct RangedFloat
{
    public float minValue;
    public float maxValue;

    public float GetRandom()
    {
        return Random.Range(minValue, maxValue);
    }
}
