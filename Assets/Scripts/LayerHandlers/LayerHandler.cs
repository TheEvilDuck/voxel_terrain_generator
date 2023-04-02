using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LayerHandler : ScriptableObject
{
    [SerializeField] LayerHandler _next;
    public bool Handle(BlockType[,,] blocks,Vector2Int column,int height,int maxHeight)
    {
        if (TryHandle(blocks,column, height,maxHeight))
            return true;
        if (_next!=null)
            return _next.Handle(blocks,column,height, maxHeight);
        return false;
    }
    protected abstract bool TryHandle(BlockType[,,] blocks,Vector2Int column,int height,int maxHeight);
}
