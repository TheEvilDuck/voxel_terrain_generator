using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Surface layer handler", menuName = "Layer handlers/Surface layer handler")]
public class SurfaceLayerHandler : LayerHandler
{
    [SerializeField] BlockType _surfaceBlock;
    protected override BlockType? TryHandle(int currentHeight,int maxHeight)
    {
        if (currentHeight>=maxHeight-1)
            {
                return _surfaceBlock;
            }
        return null;
    }
}
