
public struct WhiteNoise : INoise
{

    private float seed;

    public WhiteNoise(float seed)
    {
        this.seed = seed;
    }

    public float Noise(float x, float y)
    {
        return NoiseUtils.Rand(x * y, (x / y) + 23.2232f, seed);
    }
}
