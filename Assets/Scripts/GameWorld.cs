using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


public class GameWorld : MonoBehaviour
{

    [SerializeField] TerrainGenerator _terainGenerator;

    public GameObject _chunkPrefab;
    public GameObject _playerPrefab;
    public int _chunkWidth = 10;
    public int _chunkHeight = 150;
    public float _blockSize = 1f;
    public int _renderDistance = 4;
    public int _loadDistance = 6;

    private Dictionary<Vector2Int,ChunkRenderer>_world;

    

    private void Start()
    {
        _world = new Dictionary<Vector2Int, ChunkRenderer>();
        SpawnPlayer();
        
    }
    private void SpawnPlayer()
    {
        var player = Instantiate(_playerPrefab);
        player.transform.position = new Vector3(_chunkWidth*_blockSize/2,_chunkHeight*_blockSize+1,_chunkWidth*_blockSize/2);
        player.GetComponent<PlayerMovement>().SetWorld(this);
    }
    public ChunkData GetChunkDataOfCertainChunk(Vector2Int coordinates)
    {
        if ( !_world.ContainsKey(coordinates))
        {
            return null;
        }
        return _world[coordinates].GetChunkData();
    }
    public bool ModifyBlock(Vector2Int chunkCoordinates,Vector3Int blockpos,BlockType newBlock)
    {
        if (_world.TryGetValue(chunkCoordinates, out ChunkRenderer chunk))
        {
            chunk._chunkData.ModifyBlock(blockpos,newBlock);
            chunk.Render(_blockSize);
            if (blockpos.x==_chunkWidth-1)
            {
                if (_world.TryGetValue(chunkCoordinates+Vector2Int.left, out ChunkRenderer neighbor))
                {
                    neighbor.Render(_blockSize);
                }
            }
            if (blockpos.x==0)
            {
                if (_world.TryGetValue(chunkCoordinates+Vector2Int.right, out ChunkRenderer neighbor))
                {
                    neighbor.Render(_blockSize);
                }
            }
            if (blockpos.z==_chunkWidth-1)
            {
                if (_world.TryGetValue(chunkCoordinates+Vector2Int.up, out ChunkRenderer neighbor))
                {
                    neighbor.Render(_blockSize);
                }
            }
            if (blockpos.z==0)
            {
                if (_world.TryGetValue(chunkCoordinates+Vector2Int.down, out ChunkRenderer neighbor))
                {
                    neighbor.Render(_blockSize);
                }
            }
        }
        return false;
    }
    /*public void UnrenderLoadedChunks(Vector2Int chunkCoordinatesCenter)
    {
        foreach (KeyValuePair<Vector2Int,ChunkRenderer>chunk in _world)
        {
            if (chunk.Key.x<chunkCoordinatesCenter.x-_renderDistance||
            chunk.Key.x>chunkCoordinatesCenter.x+_renderDistance||
            chunk.Key.y<chunkCoordinatesCenter.y-_renderDistance||
            chunk.Key.y>chunkCoordinatesCenter.y+_renderDistance)
            chunk.Value.UnRender();
        }
    }*/
    public void RenderLoadedChunks(Vector2Int chunkCoordinatesCenter)
    {
        for (int x = chunkCoordinatesCenter.x-_renderDistance;x<chunkCoordinatesCenter.x+_renderDistance;x++)
        {
            for (int y = chunkCoordinatesCenter.y-_renderDistance;y<chunkCoordinatesCenter.y+_renderDistance;y++)
            {
                if (_world.TryGetValue(new Vector2Int(x,y),out ChunkRenderer chunk))
                {
                    if (chunk.Rendered)continue;
    
                }
                chunk.Render(_blockSize);
                chunk.Rendered = true;

                /*if (_world.TryGetValue(new Vector2Int(x,y)+Vector2Int.left, out ChunkRenderer neighbor))
                {
                    if (!neighbor.Rendered)
                    neighbor.Render(_blockSize);
                    neighbor.Rendered = true;
                }
                if (_world.TryGetValue(new Vector2Int(x,y)+Vector2Int.right, out ChunkRenderer neighbor2))
                {
                    if (!neighbor2.Rendered)
                    neighbor2.Render(_blockSize);
                    neighbor2.Rendered = true;
                }
                if (_world.TryGetValue(new Vector2Int(x,y)+Vector2Int.up, out ChunkRenderer neighbor3))
                {
                    if (!neighbor3.Rendered)
                    neighbor3.Render(_blockSize);
                    neighbor3.Rendered = true;
                }
                if (_world.TryGetValue(new Vector2Int(x,y)+Vector2Int.down, out ChunkRenderer neighbor4))
                {
                    if (!neighbor4.Rendered)
                    neighbor4.Render(_blockSize);
                    neighbor4.Rendered = true;
                }*/
            }
        }
        
    }
    public void LoadChunksAround(Vector2Int chunkCoordinatesCenter)
    {
        for (int x = chunkCoordinatesCenter.x-_loadDistance;x<chunkCoordinatesCenter.x+_loadDistance;x++)
        {
            for (int y = chunkCoordinatesCenter.y-_loadDistance;y<chunkCoordinatesCenter.y+_loadDistance;y++)
            {
                if (_world.ContainsKey(new Vector2Int(x,y))) continue;
                LoadChunkAtPosition(new Vector2Int(x,y));
            }
        }
    }

    private void LoadChunkAtPosition(Vector2Int chunkCoordinates)
    {
        var chunk =  Instantiate(_chunkPrefab);
        chunk.transform.position = new Vector3(_chunkWidth*chunkCoordinates.x*_blockSize,0,_chunkWidth*chunkCoordinates.y*_blockSize);
        ChunkRenderer chunkRenderer = chunk.GetComponent<ChunkRenderer>();
        chunkRenderer.Rendered = false;
        BlockType[,,]blocks = new BlockType[_chunkWidth,_chunkHeight,_chunkWidth];
        blocks = _terainGenerator.GenerateTerrain(chunkCoordinates.x,chunkCoordinates.y,_chunkWidth,_chunkHeight);
        chunkRenderer.SetChunkData(blocks);
        chunkRenderer.SetWorld(this);
        chunkRenderer.SetChunkCoordinates(new Vector2Int(chunkCoordinates.x,chunkCoordinates.y));
        _world.Add(new Vector2Int(chunkCoordinates.x,chunkCoordinates.y),chunkRenderer);
        

    }

    public Vector2Int worldPosToChunkCoordinates(Vector3 worldPos)
    {
        Vector3Int blockWorldPos = Vector3Int.FloorToInt(worldPos/_blockSize);
        return new Vector2Int(blockWorldPos.x/_chunkWidth,blockWorldPos.z/_chunkWidth);
    }
}
