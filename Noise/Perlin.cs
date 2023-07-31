
public struct Perlin : INoise
{
    private Permutations permutations;

    public Perlin(bool randomize)
    {
        permutations = new Permutations(randomize);
    }

    public float Noise(float x, float y)
    {
        int xInd = (int)x & (permutations.Length - 1);
        int yInd = (int)y & (permutations.Length - 1);

        float xRel = x - (int)x;
        float yRel = y - (int)y;

        float u = Fade(xRel);
        float v = Fade(yRel);

        int topLeft = permutations[permutations[xInd] + yInd];
        int topRight = permutations[permutations[xInd] + Increment(yInd)];
        int botLeft = permutations[permutations[Increment(xInd)] + yInd];
        int botRight = permutations[permutations[Increment(xInd)] + Increment(yInd)];

        float left = Lerp(Gradient(topLeft, xRel, yRel), Gradient(botLeft, xRel - 1, yRel), u);
        float right = Lerp(Gradient(topRight, xRel, yRel - 1), Gradient(botRight, xRel - 1, yRel - 1), u);

        return (Lerp(left, right, v) + 1) * 0.5f;
    }

    private int Increment(int num)
    {
        return num + 1 > permutations.Length ? num % permutations.Length : num + 1;
    }

    private float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    private float Lerp(float a, float b, float t)
    {
        return a + t * (b - a);
    }

    private float Gradient(int hash, float x, float y)
    {
        int h = hash & 15;
        float u = h < 8 ? x : y;
        float v = h < 4 ? y : h == 12 || h == 14 ? x : 0;

        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    public void Dispose()
    {
        permutations.Dispose();
    }
}
