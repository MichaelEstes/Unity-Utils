using ScrimVec;
using UnityEngine;

public struct Voronoi : INoise
{
    private bool borders, invert;
    private Vec2 seedx;
    private Vec2 seedy;

    public Voronoi(bool borders = false, bool invert = false)
    {
        this.borders = borders;
        this.invert = invert;
        seedx = new Vec2(Random.Range(10.0f, 100.0f), Random.Range(10.0f, 100.0f));
        seedy = new Vec2(Random.Range(10.0f, 100.0f), Random.Range(10.0f, 100.0f));
    }

    public float Noise(float x, float y)
    {
        Vec2 point = new Vec2(x, y);
        Vec2 baseCell = new Vec2(NoiseUtils.FastFloor(x), NoiseUtils.FastFloor(y));
        Vec2 cell = Vec2.zero;
        Vec2 pointToClosestCell = Vec2.zero;
        float minDist = float.MaxValue;
        float noise;

        for (int x1 = -1; x1 <= 1; x1++)
        {
            for (int y1 = -1; y1 <= 1; y1++)
            {
                Vec2 currCell = baseCell + new Vec2(x1, y1);
                Vec2 cellPosition = currCell + NoiseUtils.RandVec(currCell, seedx, seedy);
                Vec2 pointToCell = cellPosition - point;
                float dist = pointToCell.sqrLength;
                if (dist < minDist)
                {
                    minDist = dist;
                    cell = currCell;
                    pointToClosestCell = pointToCell;
                }
            }
        }

        if (!borders)
        {
            minDist = Mathf.Clamp(Mathf.Sqrt(minDist), 0.0f, 1.0f);
            noise = invert ? 1 - minDist : minDist;
        }
        else
        {
            float minEdgeDist = float.MaxValue;
            for (int x2 = -1; x2 <= 1; x2++)
            {
                for (int y2 = -1; y2 <= 1; y2++)
                {
                    Vec2 currCell = baseCell + new Vec2(x2, y2);
                    Vec2 cellPosition = currCell + NoiseUtils.RandVec(currCell, seedx, seedy);
                    Vec2 neighbor = cellPosition - point;

                    Vec2 diff = Vec2.Abs(cell - currCell);
                    if (!(diff.x + diff.y < 0.1))
                    {
                        Vec2 center = (pointToClosestCell + neighbor) * 0.5f;
                        Vec2 cellToCenter = (neighbor - pointToClosestCell).normalized;
                        float edgeDist = Vec2.Dot(center, cellToCenter);

                        minEdgeDist = Mathf.Min(minEdgeDist, edgeDist);
                    }
                }
            }

            noise = invert ? 1 - minEdgeDist : minEdgeDist;
        }

        return noise;
    }
}
