using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OctaveSettings
{
    public float scale;
    public float height;
}
[System.Serializable]
public struct Noise
{
    public OctaveSettings[] _octaves;

    public float GetNoiseValue(int xOffset,int yOffset)
    {
        float noiseValue = 1f;
        for (int i = 0;i<_octaves.GetLength(0);i++)
        {
            float xNoise = (xOffset)/_octaves[i].scale;
            float yNoise = (yOffset)/_octaves[i].scale;
            float calculatedNoise = Mathf.PerlinNoise(xNoise,yNoise)*_octaves[i].height;
            noiseValue*=calculatedNoise;
        }
        return noiseValue;
    }
}
