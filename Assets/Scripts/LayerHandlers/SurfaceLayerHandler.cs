using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Surface layer handler", menuName = "Layer handlers/Surface layer handler")]
public class SurfaceLayerHandler : LayerHandler
{
    [SerializeField] BlockType _surfaceBlock;
    protected override bool TryHandle(BlockType[,,] blocks,Vector3Int blockPos,int maxHeight, Vector2Int chunkCoordinates,float chunkWidth)
    {
        if (blockPos.y==maxHeight-1)
        {
            if (blocks[blockPos.x,blockPos.y,blockPos.z]==BlockType.Air)
            {
                blocks[blockPos.x,blockPos.y,blockPos.z] = _surfaceBlock;
                return true;
            }
        }
        return false;
    }
}
