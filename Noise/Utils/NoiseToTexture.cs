using System.Collections;
using System.Collections.Generic;
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

    // Hacky way to enable burst compilation, not sure if this works outside of the editor
    void FillBurstCompiles()
    {
#pragma warning disable CS0219
        var job1 = new NoiseJob<Voronoi>();
        var job2 = new NoiseJob<Perlin>();
        var job3 = new NoiseJob<SimplexNoise>();
        var job4 = new NoiseJob<WhiteNoise>();
        var job5 = new NoiseJob<HardWhiteNoise>();
        var job6 = new NoiseJob<OctaveNoise<Voronoi>>();
        var job7 = new NoiseJob<OctaveNoise<Perlin>>();
        var job8 = new NoiseJob<OctaveNoise<SimplexNoise>>();
        var job9 = new NoiseJob<OctaveNoise<WhiteNoise>>();
        var job10 = new NoiseJob<OctaveNoise<HardWhiteNoise>>();
#pragma warning restore CS0219
    }

    public void SetNoiseToTextureMultiThread<T>(T noise) where T : struct, INoise
    {
        int res = textureRes * textureRes;
        NativeArray<T> noiseArr = new NativeArray<T>(1, Allocator.Persistent);
        NativeArray<Color32> greyScale = new NativeArray<Color32>(res, Allocator.Persistent);

        noiseArr[0] = noise;

        NoiseJob<T> job = new NoiseJob<T>()
        {
            noise = noiseArr,
            greyScale = greyScale,
            width = textureRes,
            scale = scale
        };

        JobHandle jobHandle = job.Schedule(res, 64);

        jobHandle.Complete();

        ColorsToTexture(job.greyScale.ToArray());

        noise.Dispose();
        noiseArr.Dispose();
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
