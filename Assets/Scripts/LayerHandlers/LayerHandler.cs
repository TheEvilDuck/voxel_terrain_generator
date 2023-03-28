using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LayerHandler : ScriptableObject
{
    [SerializeField] LayerHandler _next;
    public BlockType? Handle(int currentHeight,int maxHeight)
    {
        BlockType? result = TryHandle(currentHeight,maxHeight);
        if (result==null)
        {
            if (_next!=null)
                result = _next.Handle(currentHeight,maxHeight);
        }
        return result;


    }
    protected abstract BlockType? TryHandle(int currentHeight,int maxHeight);
}
