using UnityEngine;
using Unity.Burst;

public class NoiseTester : MonoBehaviour
{
    public NoiseToTexture noiseToTexture;

    private void Start()
    {
        SetNoise();
    }

    private void SetNoise()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        float seed = Random.Range(100000.0f, 200000.0f);

        //noiseToTexture.SetNoiseToTexture(new Perlin(false));
        //noiseToTexture.SetNoiseToTexture(new WhiteNoise(seed));
        //noiseToTexture.SetNoiseToTexture(new HardWhiteNoise(seed));
        //noiseToTexture.SetNoiseToTexture(new OctaveNoise<SimplexNoise>(new SimplexNoise(), 4, 0.5f));
        //noiseToTexture.SetNoiseToTexture(new SimplexNoise(true));
        //noiseToTexture.SetNoiseToTexture(new Voronoi(true, true));
        //noiseToTexture.SetNoiseToTexture(new OctaveNoise<Voronoi>(new Voronoi(true, true), 4, 0.5f));

        //noiseToTexture.SetNoiseToTexture(new OctaveNoise(new Voronoi(false, true), 4, 0.5f));
        //noiseToTexture.CreateMooreCellularAutomata(new Voronoi(false, true));
        //noiseToTexture.CreateMooreCellularAutomata(new OctaveNoise(new Voronoi(false, true), 4, 0.5f));

        //noiseToTexture.SetNoiseToTextureMultiThread(new Voronoi(false, false));
        //noiseToTexture.SetNoiseToTextureMultiThread(new Perlin(true));
        //noiseToTexture.SetNoiseToTextureMultiThread(new OctaveNoise<SimplexNoise>(new SimplexNoise(), 4, 0.5f));
        //noiseToTexture.SetNoiseToTextureMultiThread(new WhiteNoise(seed));
        //noiseToTexture.SetNoiseToTextureMultiThread(new HardWhiteNoise(seed));
        noiseToTexture.SetNoiseToTextureMultiThread(new OctaveNoise<Voronoi>(new Voronoi(true, true), 4, 0.5f));

        //Vec2 vec = new Vec2(4f, 6f);
        //Debug.Log("Vector 2: " + vec);
        //(float x, float y) = vec;
        //Debug.Log("x: " + x + ", y: " + y);
        //(float x2, float y2) = vec.xy;
        //Debug.Log("x: " + x2 + ", y: " + y2);

        //vec.xy = (3f, 4f);
        //Debug.Log("Vec 2: " + vec);

        //vec.x = 6f;
        //Debug.Log("Vec 2: " + vec);

        //Debug.Log("Vec 2 normalized: " + vec.normalized);

        //Debug.Log("Vec 2 multipled scalar: " + (vec * 3f));

        //Debug.Log("Vec 2 add scalar: " + (vec + 3f));

        //Debug.Log("Vec 2 divide scalar: " + (vec / 3f));

        //Debug.Log("Vec 2 sub scalar: " + (vec - 3f));

        //Vector2 vec1 = vec;

        //Debug.Log("Vector 2 unity: " + vec1);

        //Vec2 vec2 = vec1;

        //Debug.Log("Vec 2 from unity: " + vec2);

        //Debug.Log("Vec 2 zero: " + Vec2.zero);
    }
}
