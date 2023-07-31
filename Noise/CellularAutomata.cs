
public class CellularAutomata
{

    public float[,] Moore(INoise noise, int resolution, float scale, float density, int iterations, float passThreshold = 0.25f, int neighborsNeeded = 4)
    {
        float[,] grid = CreateGrid(noise, resolution, scale, density);

        for (int i = 0; i < iterations; i++)
        {
            float[,] temp = grid.Clone() as float[,];

            for (int y = 0; y < resolution; y++)
            {
                for (int x = 0; x < resolution; x++)
                {
                    int neighborPassCount = GetNeighborPassedCount(temp, x, y, passThreshold);

                    if (neighborPassCount <= neighborsNeeded)
                    {
                        grid[x, y] = 0.0f;
                    }
                }
            }
        }

        return grid;
    }

    private float[,] CreateGrid(INoise noise, int resolution, float scale, float density)
    {
        float[,] grid = new float[resolution, resolution];

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                float xf = (float)x / resolution * scale;
                float yf = (float)y / resolution * scale;

                float val = noise.Noise(xf, yf);

                grid[x, y] = val > density ? val : 0.0f;
            }
        }

        return grid;
    }

    private int GetNeighborPassedCount(float[,] grid, int x, int y, float passThreshold)
    {
        int passedNeighbors = 0;

        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int newX = x + i;
                int newY = y + j;

                // Verify it's within the grid bounds
                if (newX >= 0 && newY >= 0 && newX < rows && newY < cols)
                {
                    float val = grid[newX, newY];

                    if (val > passThreshold)
                    {
                        passedNeighbors += 1;
                    }
                }
                else
                {
                    passedNeighbors += 1;
                }
            }
        }

        return passedNeighbors;
    }
}
