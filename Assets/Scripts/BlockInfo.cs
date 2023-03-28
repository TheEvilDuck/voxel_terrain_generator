using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/Create block")]
public class BlockInfo : ScriptableObject
{
    public BlockType blockType;
    public Vector2Int blockIdOnTexture;

}
