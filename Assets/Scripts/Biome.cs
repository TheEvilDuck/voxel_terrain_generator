using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName ="Biomes/Create a biome")]
public class Biome : ScriptableObject
{
    [SerializeField]int _maxHeight = 0;
    [SerializeField]OctaveSettings[] _octaves;
    [SerializeField]LayerHandler _startLayerHandler;
    [SerializeField]BlockType _defaultBlock;
    
    public BlockType GenerateBlockAtPosition()
    {
        
    }
}
