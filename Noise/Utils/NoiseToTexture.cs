using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    private Color32[] NoiseToColorArr(INoise noise)
    {
        Color32[] greyscale = new Color32[textureRes * textureRes];

        for(int y = 0; y < textureRes; y++)
        {
            for(int x = 0; x < textureRes; x++)
            {
                float xf = (float)x / textureRes * scale;
                float yf = (float)y / textureRes * scale;

                byte grey = (byte)(noise.Noise(xf, yf) * 255);

                int index = (y * textureRes) + x;
                greyscale[index] = new Color32(grey, grey, grey, 255);
            }
        }

        //Debug.Log(string.Join("\n", greyscale));
        return greyscale;
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

        while(true)
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
        Color32[] greyscale = new Color32[vals.Length];

        for(int i = 0; i < greyscale.Length; i++)
        {
            byte grey = (byte)(vals[i] * 255);
            greyscale[i] = new Color32(grey, grey, grey, 255);
        }

        return greyscale;
    }
}
