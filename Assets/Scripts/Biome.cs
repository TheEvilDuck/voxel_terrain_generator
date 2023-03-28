using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="Biomes/Create a biome")]
public class Biome : ScriptableObject
{
    [SerializeField]int _maxHeight = 0;
    [SerializeField]OctaveSettings[] _octaves;
    [SerializeField]LayerHandler _startLayerHandler;
    [SerializeField]BlockType _defaultBlock;
    public int GenerateHeightInPositionXY(int x,int z,int xOffset,int yOffset,int chunkWidth,int chunkHeight)
    {
        int height = 1;

        float resultHeight = (float)chunkHeight/2f;
        for (int i = 0;i<_octaves.GetLength(0);i++)
        {
            float xNoise = (x+xOffset*chunkWidth)/_octaves[i].scale;
            float yNoise = (z+yOffset*chunkWidth)/_octaves[i].scale;
            resultHeight*=Mathf.PerlinNoise(xNoise,yNoise)*_octaves[i].height;
        }

        height = (int)resultHeight;
        if (height<=0)height = 1;
        if (height>chunkHeight)height = chunkHeight;
         if (_maxHeight>0)
        {
            if (height>_maxHeight)
            {
                height = _maxHeight;
            }
        }
        return height;
    }
    public BlockType[] GenerateBlocksInPositionXY(int height,int chunkHeight)
    {
        BlockType[]blocks = new BlockType[chunkHeight];
        float heightOfHeightMapToChunkHeight = (float)height/(float)chunkHeight;
        for (int y = 0;y<height;y++)
        {
            BlockType? resultBlock = BlockType.Bedrock;
            resultBlock = _startLayerHandler.Handle(y,height);
            if (resultBlock==null)
                resultBlock = _defaultBlock;   
            blocks[y] = (BlockType)resultBlock;
        }
        return blocks;
    }   
}
