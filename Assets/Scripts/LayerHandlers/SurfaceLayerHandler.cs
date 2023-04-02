using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Surface layer handler", menuName = "Layer handlers/Surface layer handler")]
public class SurfaceLayerHandler : LayerHandler
{
    [SerializeField] BlockType _surfaceBlock;
    protected override bool TryHandle(BlockType[,,] blocks,Vector2Int column,int height,int maxHeight)
    {
        if (height==maxHeight-1)
        {
            if (blocks[column.x,height,column.y]==BlockType.Air)
            {
                blocks[column.x,height,column.y] = _surfaceBlock;
                return true;
            }
        }
        return false;
    }
}
