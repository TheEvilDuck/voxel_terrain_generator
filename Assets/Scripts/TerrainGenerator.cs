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
                    biomeValues.TryAdd(biome,1f);
                }
                foreach (NoiseMap map in _maps)
                {
                    float noiseValue = map._map.GetNoiseValue(x+xOffset*chunkWidth,z+yOffset*chunkWidth);
                    foreach (Affect affect in map._affects)
                    {
                        float decreaseAffect = 1f-affect._value;
                        float increaseAffect = 1f+affect._value;

                        float decreaseValue = (1f-noiseValue)*decreaseAffect;
                        float increaseValue = noiseValue*increaseAffect;

                        biomeValues[affect._biome]+= (decreaseValue+increaseValue)*affect._multiplier;
                    }
                }
                float maxValue = biomeValues[_biomes[0]];
                Biome maxBiome = _biomes[0];
                foreach (KeyValuePair<Biome,float>pair in biomeValues)
                {
                    if (pair.Value>maxValue)
                    {
                        maxValue=pair.Value;
                        maxBiome = pair.Key;
                    }
                }
                resultBlocksOfChunk[x,0,z] = BlockType.Bedrock;
                resultBlocksOfChunk = maxBiome.GenerateBlocks(resultBlocksOfChunk,new Vector2Int(x,z),new Vector2Int(xOffset,yOffset));
            }
        }
        
        GeneratingMarker.End();
        return resultBlocksOfChunk;
    }
}
