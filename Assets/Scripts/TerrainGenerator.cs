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
        
        
        GeneratingMarker.End();
        return resultBlocksOfChunk;
    }
}
