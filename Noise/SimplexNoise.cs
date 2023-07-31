using ScrimVec;

public struct SimplexNoise : INoise
{
    //0.5f * (Mathf.Sqrt(3.0f) - 1.0f)
    private const float skew = 0.366025f;
    //(3.0f - Mathf.Sqrt(3.0f)) / 6.0f
    private const float unskew = 0.2113225f;

    private static readonly Vec2[] grad2D = new Vec2[] {
        (1.0f, 0.0f), (1.0f, 1.0f), (0.0f, 1.0f), (-1.0f, 1.0f), (-1.0f, 0.0f), (-1.0f, -1.0f), (0.0f, -1.0f), (1.0f, -1.0f)
    };
    private static readonly Vec3[] grad3D = new Vec3[] {
        (1.0f, 0.0f, 0.0f), (-1.0f, 0.0f, 0.0f), (0.0f, 1.0f, 0.0f), (0.0f, -1.0f, 0.0f), (0.0f, 0.0f, 1.0f), (0.0f, 0.0f, -1.0f),
        (1.0f, 1.0f, 1.0f), (-1.0f, -1.0f, 1.0f), (1.0f, -1.0f, -1.0f), (-1.0f, 1.0f, -1.0f),
        (1.0f, 1.0f, -1.0f), (-1.0f, -1.0f, -1.0f), (1.0f, -1.0f, 1.0f), (-1.0f, 1.0f, 1.0f)
    };

    private Permutations permutations;

    public SimplexNoise(bool randomize)
    {
        permutations = new Permutations(randomize);
    }

    public float Noise(float x, float y)
    {
        float skewOffset = (x + y) * skew;
        float skewedX = x + skewOffset;
        float skewedY = y + skewOffset;

        int originX = NoiseUtils.FastFloor(skewedX);
        int originY = NoiseUtils.FastFloor(skewedY);

        float unskewOffset = (originX + originY) * unskew;
        float realX = originX - unskewOffset;
        float realY = originY - unskewOffset;

        int simplexX, simplexY;
        if (skewedX - originX > skewedY - originY)
        {
            simplexX = 1;
            simplexY = 0;
        }
        else
        {
            simplexX = 0;
            simplexY = 1;
        }

        // Create distance vectors for triangle
        float x0 = x - realX;
        float y0 = y - realY;

        float x1 = x0 - simplexX + unskew;
        float y1 = y0 - simplexY + unskew;

        float x2 = x0 - 1.0f + 2.0f * unskew;
        float y2 = y0 - 1.0f + 2.0f * unskew;

        // Get gradient vectors
        int xIndex = originX & permutations.Length - 1;
        int yIndex = originY & permutations.Length - 1;

        int index1 = permutations[xIndex + permutations[yIndex]];
        int index2 = permutations[xIndex + simplexX + permutations[yIndex + simplexY]];
        int index3 = permutations[Increment(xIndex) + permutations[Increment(yIndex)]];

        var (grad0x, grad0y) = grad2D[index1 % 8];
        var (grad1x, grad1y) = grad2D[index2 % 8];
        var (grad2x, grad2y) = grad2D[index3 % 8];


        float noise = 0.0f;
        // Update noise with contribution from each vector
        for (int i = 0; i < 3; i++)
        {
            float dx, dy, gradientX, gradientY;

            switch (i)
            {
                case 0:
                    dx = x0;
                    dy = y0;
                    gradientX = grad0x;
                    gradientY = grad0y;
                    break;
                case 1:
                    dx = x1;
                    dy = y1;
                    gradientX = grad1x;
                    gradientY = grad1y;
                    break;
                default:
                    dx = x2;
                    dy = y2;
                    gradientX = grad2x;
                    gradientY = grad2y;
                    break;
            }

            float dist = 0.5f - dx * dx - dy * dy;
            if (dist > 0.0f)
            {
                float dot = dx * gradientX + dy * gradientY;
                dist *= dist;
                noise += dist * dist * dot;
            }
        }

        //Scale to 0 and 1
        return ((noise * 70.0f) + 1) * 0.5f;
    }

    private int Increment(int num)
    {
        return num + 1 > permutations.Length ? num % permutations.Length : num + 1;
    }

    public void Dispose()
    {
        permutations.Dispose();
    }
}
