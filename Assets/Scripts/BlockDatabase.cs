using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Blocks/Create blocks database")]
public class BlockDatabase:ScriptableObject
{
    [SerializeField] BlockInfo[]blocks;

    private Dictionary<BlockType,BlockInfo>blocks_dictionary= new Dictionary<BlockType, BlockInfo>();

    private void InitDictionary()
    {
        blocks_dictionary.Clear();
        for (int i = 0;i<blocks.GetLength(0);i++)
        {
            blocks_dictionary.Add(blocks[i].blockType,blocks[i]);
        }
    }

    private void OnEnable() {
        InitDictionary();
    }
    public BlockInfo GetBlockInfo(BlockType block)
    {
        return blocks_dictionary.GetValueOrDefault(block);
    }

}
