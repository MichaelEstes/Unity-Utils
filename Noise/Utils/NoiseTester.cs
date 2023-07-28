using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTester : MonoBehaviour
{
    public NoiseToTexture noiseToTexture;

    private void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        //noiseToTexture.SetNoiseToTexture(new Perlin(false));
        //noiseToTexture.SetNoiseToTexture(new WhiteNoise());
        //noiseToTexture.SetNoiseToTexture(new HardWhiteNoise());
        //noiseToTexture.SetNoiseToTexture(new OctaveNoise(new SimplexNoise(), 4, 0.5f));
        //noiseToTexture.SetNoiseToTexture(new SimplexNoise());

        noiseToTexture.CreateMooreCellularAutomata(new WhiteNoise());
    }
}
