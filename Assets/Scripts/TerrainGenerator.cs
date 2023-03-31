using UnityEngine;
using Unity.Profiling;
using System.Collections.Generic;


public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]Biome[] _biomes;
    [SerializeField]NoiseMap[] _maps;
    [SerializeField]int _seed = 0;

    private static ProfilerMarker GeneratingMarker = new ProfilerMarker(ProfilerCategory.Loading,"Generating chunks");

    public BlockType[,,] GenerateTerrain(int xOffset,int yOffset,int chunkWidth,int chunkHeight)
    {
        GeneratingMarker.Begin();
        BlockType[,,] resultBlocksOfChunk = new BlockType[chunkWidth,chunkHeight,chunkWidth];
        for (int x = 0;x<chunkWidth;x++)
        {
            for (int z = 0;z<chunkWidth;z++)
            {
                Dictionary<Biome,float>biomeValues = new Dictionary<Biome, float>();
                foreach (Biome biome in _biomes)
                {
                    biomeValues.Add(biome,1);
                }
                Vector2Int seedOffset = new Vector2Int(
                    (xOffset+_seed)*(_seed%10),
                    (yOffset+_seed)*(_seed%10)
                );
                foreach (NoiseMap noiseMap in _maps)
                {
                    float globalValue = noiseMap._map.GetNoiseValue(x+seedOffset.x*chunkWidth,z+seedOffset.y*chunkWidth);

                    foreach (Affect affect in noiseMap._affects)
                    {
                        if (biomeValues.ContainsKey(affect._biome))
                        {
                            if (affect._value<0)
                            {
                            biomeValues[affect._biome]*=Mathf.Abs(affect._multiplier)/globalValue;
                            }
                            if (affect._value>0)
                            {
                                biomeValues[affect._biome]*=(affect._multiplier*globalValue);
                            }
                        }
                    }
                }

                float maxValue = -1;
                Biome maxValueBiome = _biomes[0];
                float maxValue_2 = -1;
                Biome maxValueBiome_2 = _biomes[0];

                foreach (KeyValuePair<Biome,float> biomeValue in biomeValues)
                {
                    if (biomeValue.Value>maxValue)
                    {
                        maxValue = biomeValue.Value;
                        maxValueBiome = biomeValue.Key;
                    }
                    else if (biomeValue.Value>maxValue_2)
                    {
                        maxValue_2 = biomeValue.Value;
                        maxValueBiome_2 = biomeValue.Key;
                    }
                }

                int heightInPositionXY = maxValueBiome.GenerateHeightInPositionXY(x,z,xOffset,yOffset,chunkWidth,chunkHeight);
                int heightInPositionXY_2 = maxValueBiome_2.GenerateHeightInPositionXY(x,z,xOffset,yOffset,chunkWidth,chunkHeight);

                float percent = maxValue/(maxValue+maxValue_2);

                float maxHeightPercent = (float)heightInPositionXY*percent;
                float maxHeightPercent_2 = (float)heightInPositionXY_2*(1-percent);

                int averageHeight = (int)(maxHeightPercent+maxHeightPercent_2);

                if (averageHeight>chunkHeight)
                    averageHeight = chunkHeight;
                if (averageHeight<=0)
                    averageHeight = 1;

                BlockType[] blocks = maxValueBiome.GenerateBlocksInPositionXY(averageHeight,chunkHeight);

                for (int y = 0;y<averageHeight;y++)
                {
                    resultBlocksOfChunk[x,y,z] = blocks[y];
                }
            }
        }

        GeneratingMarker.End();
        return resultBlocksOfChunk;
    }
}
