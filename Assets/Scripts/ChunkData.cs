using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData
{
    public BlockType[,,]_blocks;
    public Vector2Int _chunkCoordinates;
    public GameWorld _world;
    public BlockType GetBlockAtPosition(Vector3Int blockPos)
    {
        if (blockPos.y<0||blockPos.y>=_blocks.GetLength(1))
        {
            return BlockType.Air;
        }
        if (blockPos.x<0)
        {
            return CheckNeighbor(new Vector3Int(_blocks.GetLength(0)-1,blockPos.y,blockPos.z),new Vector2Int(-1,0));
        }
        if (blockPos.x>=_blocks.GetLength(0))
        {
            return CheckNeighbor(new Vector3Int(0,blockPos.y,blockPos.z),new Vector2Int(1,0));
        }
        if (blockPos.z<0)
        {
            return CheckNeighbor(new Vector3Int(blockPos.x,blockPos.y,_blocks.GetLength(2)-1),new Vector2Int(0,-1));
        }
        if (blockPos.z>=_blocks.GetLength(2))
        {
            return CheckNeighbor(new Vector3Int(blockPos.x,blockPos.y,0),new Vector2Int(0,1));
            
        }
        return _blocks[blockPos.x,blockPos.y,blockPos.z];
    }
    private BlockType CheckNeighbor(Vector3Int blockPos,Vector2Int chunkOffset)
    {
        ChunkData neighborChunk = _world.GetChunkDataOfCertainChunk(_chunkCoordinates+ chunkOffset);
            if (neighborChunk==null)
                return BlockType.Air;
        return neighborChunk._blocks[blockPos.x,blockPos.y,blockPos.z];
    }
    public void ModifyBlock(Vector3Int blockPos, BlockType newValue)
    {
        _blocks[blockPos.x,blockPos.y,blockPos.z] = newValue;
    }
}
