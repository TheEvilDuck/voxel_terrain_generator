using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OctaveSettings
{
    public float frequency;
    public float height;
}
[System.Serializable]
public struct Noise
{
    public OctaveSettings[] _octaves;
    public float GetNoiseValue(int xOffset,int yOffset)
    {
        float noiseValue = 1f;
        float amplitudeSum = 0;
        for (int i = 0;i<_octaves.GetLength(0);i++)
        {
            float xNoise = xOffset*_octaves[i].frequency;
            float yNoise = yOffset*_octaves[i].frequency;
            float calculatedNoise = Mathf.PerlinNoise(xNoise*_octaves[i].height,yNoise*_octaves[i].height)/_octaves[i].height;
            amplitudeSum+=1f/_octaves[i].height;
            noiseValue+=calculatedNoise;
        }
        Debug.Log(noiseValue/amplitudeSum);
        return noiseValue/amplitudeSum;
    }
}
