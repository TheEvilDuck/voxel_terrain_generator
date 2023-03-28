using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator
{
   
    public static Texture2D GeneretaBlockTexture(float xOffset, float yOffset,int resolution)
    {
        Texture2D result = new Texture2D(resolution,resolution);
        for (int x = 0;x<resolution;x++)
        {
            for (int y = 0;y<resolution;y++)
            {
                float pixelValue = Mathf.PerlinNoise(xOffset+(float)x/(float)resolution*5f,yOffset+(float)y/(float)resolution*5f);
                Debug.Log(pixelValue);
                result.SetPixel(x,y,new Color(pixelValue,pixelValue,pixelValue));
               
            }
        }
        result.filterMode = FilterMode.Point;
        result.Apply();
        return result;
    }
}
