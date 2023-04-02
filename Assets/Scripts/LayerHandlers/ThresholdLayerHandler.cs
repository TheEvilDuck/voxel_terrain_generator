using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Threshold layer handler", menuName = "Layer handlers/Threshold layer handler")]
public class ThresholdLayerHandler : LayerHandler
{
    [SerializeField][Range(0,1)]float _maxHeight;
    [SerializeField]BlockType _resulBlock;
    [SerializeField]bool _above;
    protected override bool TryHandle(BlockType[,,] blocks,Vector2Int column,int height,int maxHeight)
    {
        float percent = (float)height+1f/(float)maxHeight;
        if (blocks[column.x,height,column.y]==BlockType.Air
        &&(_above&&percent>=_maxHeight||!_above&&percent<=_maxHeight))
        {
            blocks[column.x,height,column.y] = _resulBlock;
            return true;
        }
        return false;
    }
}
