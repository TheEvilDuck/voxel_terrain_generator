using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Threshold layer handler", menuName = "Layer handlers/Threshold layer handler")]
public class ThresholdLayerHandler : LayerHandler
{
    [SerializeField][Range(0,1)]float _maxHeight;
    [SerializeField]BlockType _resulBlock;
    [SerializeField]bool _above;
    protected override bool TryHandle(BlockType[,,] blocks,Vector3Int blockPos,int maxHeight, Vector2Int chunkCoordinates,float chunkWidth)
    {
        float percent = (float)blockPos.y+1f/(float)maxHeight;
        if (blocks[blockPos.x,blockPos.y,blockPos.z]==BlockType.Air
        &&(_above&&percent>=_maxHeight||!_above&&percent<=_maxHeight))
        {
            blocks[blockPos.x,blockPos.y,blockPos.z] = _resulBlock;
            return true;
        }
        return false;
    }
}
