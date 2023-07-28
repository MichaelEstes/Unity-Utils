using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctaveNoise : INoise
{
	private INoise noise;
	private int octaves;
	private float persistence;

	public OctaveNoise(INoise noise, int octaves, float persistence)
	{
		this.noise = noise;
		this.octaves = octaves;
		this.persistence = persistence;
	}

	public float Noise(float x, float y)
	{
		float total = 0;
		float frequency = 1;
		float amplitude = 1;
		float maxValue = 0;

		for (int i = 0; i < octaves; i++)
		{
			total += noise.Noise(x * frequency, y * frequency) * amplitude;

			maxValue += amplitude;

			amplitude *= persistence;
			frequency *= 2;
		}

		return total / maxValue;
	}
}
