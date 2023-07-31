using System.Collections;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

[BurstCompile]
struct NoiseJob<T> : IJobParallelFor where T : struct, INoise
{
    [NativeDisableParallelForRestriction]
    public NativeArray<T> noise;
    public NativeArray<Color32> greyScale;
    public int width;
    public float scale;

    public void Execute(int index)
    {
        int x = index % width;
        int y = index / width;

        float xf = (float)x / width * scale;
        float yf = (float)y / width * scale;

        float currNoise = noise[0].Noise(xf, yf);
        byte grey = (byte)(currNoise * 255);

        greyScale[index] = new Color32(grey, grey, grey, 255);
    }
}

[System.Serializable]
public class NoiseToTexture
{
    public MeshRenderer meshRenderer;
    public int textureRes;
    public float scale;

    public float density;
    public int iterations;
    public float passThreshold;


    public void SetNoiseToTexture(INoise noise)
    {
        Color32[] greyScale = NoiseToColorArr(noise);

        ColorsToTexture(greyScale);
    }

    private NoiseJob<Voronoi> CreateJobVoronoi(Voronoi noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<Voronoi> noiseArr = new NativeArray<Voronoi>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<Voronoi>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<Perlin> CreateJobPerlin(Perlin noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<Perlin> noiseArr = new NativeArray<Perlin>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<Perlin>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<SimplexNoise> CreateJobSimplexNoise(SimplexNoise noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<SimplexNoise> noiseArr = new NativeArray<SimplexNoise>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<SimplexNoise>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<WhiteNoise> CreateJobWhiteNoise(WhiteNoise noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<WhiteNoise> noiseArr = new NativeArray<WhiteNoise>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<WhiteNoise>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<HardWhiteNoise> CreateJobHardWhiteNoise(HardWhiteNoise noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<HardWhiteNoise> noiseArr = new NativeArray<HardWhiteNoise>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<HardWhiteNoise>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<OctaveNoise<Voronoi>> CreateJobOctaveNoiseVoronoi(OctaveNoise<Voronoi> noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<OctaveNoise<Voronoi>> noiseArr = new NativeArray<OctaveNoise<Voronoi>>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<OctaveNoise<Voronoi>>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<OctaveNoise<Perlin>> CreateJobOctaveNoisePerlin(OctaveNoise<Perlin> noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<OctaveNoise<Perlin>> noiseArr = new NativeArray<OctaveNoise<Perlin>>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<OctaveNoise<Perlin>>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<OctaveNoise<SimplexNoise>> CreateJobOctaveNoiseSimplexNoise(OctaveNoise<SimplexNoise> noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<OctaveNoise<SimplexNoise>> noiseArr = new NativeArray<OctaveNoise<SimplexNoise>>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<OctaveNoise<SimplexNoise>>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<OctaveNoise<WhiteNoise>> CreateJobOctaveNoiseWhiteNoise(OctaveNoise<WhiteNoise> noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<OctaveNoise<WhiteNoise>> noiseArr = new NativeArray<OctaveNoise<WhiteNoise>>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<OctaveNoise<WhiteNoise>>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    private NoiseJob<OctaveNoise<HardWhiteNoise>> CreateJobOctaveNoiseHardWhiteNoise(OctaveNoise<HardWhiteNoise> noise, int width, NativeArray<Color32> greyScale, out System.Action dispose)
    {
        NativeArray<OctaveNoise<HardWhiteNoise>> noiseArr = new NativeArray<OctaveNoise<HardWhiteNoise>>(1, Allocator.TempJob);
        noiseArr[0] = noise;
        var job = new NoiseJob<OctaveNoise<HardWhiteNoise>>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = width,
            scale = scale
        };

        dispose = () =>
        {
            noiseArr.Dispose();
        };

        return job;
    }

    public void SetNoiseToTextureMultiThread<T>(T noise) where T : struct, INoise
    {
        int res = textureRes * textureRes;
        int batchCount = 64;
        NativeArray<Color32> greyScale = new NativeArray<Color32>(res, Allocator.TempJob);

        JobHandle jobHandle;
        System.Action dispose;

        switch (noise)
        {
            case Voronoi n:
                var jobVoronoi = CreateJobVoronoi(n, textureRes, greyScale, out dispose);
                jobHandle = jobVoronoi.Schedule(res, batchCount);
                break;
            case Perlin n:
                var jobPerlin = CreateJobPerlin(n, textureRes, greyScale, out dispose);
                jobHandle = jobPerlin.Schedule(res, batchCount);
                break;
            case SimplexNoise n:
                var jobSimplexNoise = CreateJobSimplexNoise(n, textureRes, greyScale, out dispose);
                jobHandle = jobSimplexNoise.Schedule(res, batchCount);
                break;
            case WhiteNoise n:
                var jobWhiteNoise = CreateJobWhiteNoise(n, textureRes, greyScale, out dispose);
                jobHandle = jobWhiteNoise.Schedule(res, batchCount);
                break;
            case HardWhiteNoise n:
                var jobHardWhiteNoise = CreateJobHardWhiteNoise(n, textureRes, greyScale, out dispose);
                jobHandle = jobHardWhiteNoise.Schedule(res, batchCount);
                break;
            case OctaveNoise<Voronoi> n:
                var jobOctaveNoiseVoronoi = CreateJobOctaveNoiseVoronoi(n, textureRes, greyScale, out dispose);
                jobHandle = jobOctaveNoiseVoronoi.Schedule(res, batchCount);
                break;
            case OctaveNoise<Perlin> n:
                var jobOctaveNoisePerlin = CreateJobOctaveNoisePerlin(n, textureRes, greyScale, out dispose);
                jobHandle = jobOctaveNoisePerlin.Schedule(res, batchCount);
                break;
            case OctaveNoise<SimplexNoise> n:
                var jobOctaveNoiseSimplexNoise = CreateJobOctaveNoiseSimplexNoise(n, textureRes, greyScale, out dispose);
                jobHandle = jobOctaveNoiseSimplexNoise.Schedule(res, batchCount);
                break;
            case OctaveNoise<WhiteNoise> n:
                var jobOctaveNoiseWhiteNoise = CreateJobOctaveNoiseWhiteNoise(n, textureRes, greyScale, out dispose);
                jobHandle = jobOctaveNoiseWhiteNoise.Schedule(res, batchCount);
                break;
            case OctaveNoise<HardWhiteNoise> n:
                var jobOctaveNoiseHardWhiteNoise = CreateJobOctaveNoiseHardWhiteNoise(n, textureRes, greyScale, out dispose);
                jobHandle = jobOctaveNoiseHardWhiteNoise.Schedule(res, batchCount);
                break;
            default:
                var jobDefault = CreateJobPerlin(new Perlin(true), textureRes, greyScale, out dispose);
                jobHandle = jobDefault.Schedule(res, batchCount);
                break;
        }

        jobHandle.Complete();

        ColorsToTexture(greyScale.ToArray());

        noise.Dispose();
        dispose?.Invoke();
        greyScale.Dispose();
    }

    private Color32[] NoiseToColorArr(INoise noise)
    {
        Color32[] greyScale = new Color32[textureRes * textureRes];

        for (int y = 0; y < textureRes; y++)
        {
            for (int x = 0; x < textureRes; x++)
            {
                float xf = (float)x / textureRes * scale;
                float yf = (float)y / textureRes * scale;

                float currNoise = noise.Noise(xf, yf);
                byte grey = (byte)(currNoise * 255);

                int index = (y * textureRes) + x;
                greyScale[index] = new Color32(grey, grey, grey, 255);
                //greyscale[index] = RandValToColor(currNoise);
            }
        }

        //Debug.Log(string.Join("\n", greyscale));
        return greyScale;
    }

    private void ColorsToTexture(Color32[] colors)
    {
        Texture2D texture = new Texture2D(textureRes, textureRes, TextureFormat.RGB24, false);
        texture.SetPixels32(colors);
        texture.Apply();
        meshRenderer.material.SetTexture("_MainTex", texture);
    }

    public void CreateMooreCellularAutomata(INoise noise)
    {
        CellularAutomata cellularAutomata = new CellularAutomata();

        float[,] grid = cellularAutomata.Moore(noise, textureRes, scale, density, iterations, passThreshold);
        float[] arr = GridToArray(grid);
        Color32[] greyscale = FloatArrToColor(arr);

        ColorsToTexture(greyscale);

        //behaviour.StartCoroutine(Iterate(noise));
    }

    IEnumerator Iterate(INoise noise)
    {
        CellularAutomata cellularAutomata = new CellularAutomata();
        int iterations = 0;

        while (true)
        {
            float[,] grid = cellularAutomata.Moore(noise, textureRes, scale, density, iterations, passThreshold);
            float[] arr = GridToArray(grid);
            Color32[] greyscale = FloatArrToColor(arr);
            ColorsToTexture(greyscale);

            iterations++;
            yield return new WaitForSeconds(1.0f);
        }
    }

    private T[] GridToArray<T>(T[,] grid)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        T[] arr = new T[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                arr[y * width + x] = grid[x, y];

            }
        }

        return arr;
    }

    private Color32[] FloatArrToColor(float[] vals)
    {
        Color32[] greyScale = new Color32[vals.Length];

        for (int i = 0; i < greyScale.Length; i++)
        {
            byte grey = (byte)(vals[i] * 255);
            greyScale[i] = new Color32(grey, grey, grey, 255);
        }

        return greyScale;
    }

    Color32 RandValToColor(float value)
    {
        return new Color32(
            (byte)(NoiseUtils.Rand(value, 3.9812f) * 255),
            (byte)(NoiseUtils.Rand(value, 7.1536f) * 255),
            (byte)(NoiseUtils.Rand(value, 5.7241f) * 255),
            255
        );
    }

    public static void Print2DArray<T>(T[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Debug.Log(matrix[i, j] + "\t");
            }
        }
    }
}
