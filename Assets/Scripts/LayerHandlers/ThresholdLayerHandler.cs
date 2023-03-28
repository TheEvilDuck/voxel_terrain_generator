using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Threshold layer handler", menuName = "Layer handlers/Threshold layer handler")]
public class ThresholdLayerHandler : LayerHandler
{
    [SerializeField][Range(0,1)]float _maxHeight;
    [SerializeField]BlockType _resulBlock;
    [SerializeField]bool _above;
    protected override BlockType? TryHandle(int currentHeight, int maxHeight)
    {
        float percent = (float)currentHeight/(float)maxHeight;
        if (_above)
        {
            if (percent>=_maxHeight)
                return _resulBlock;
        }
        else
        {
            if (percent<=_maxHeight)
                return _resulBlock;
        }
        return null;
    }
}
