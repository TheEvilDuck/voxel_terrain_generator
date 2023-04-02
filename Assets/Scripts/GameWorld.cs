using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;


public class GameWorld : MonoBehaviour
{

    [SerializeField] TerrainGenerator _terainGenerator;
    [SerializeField]BlockDatabase _blockDatabase;

    public GameObject _chunkPrefab;
    public GameObject _playerPrefab;
    public int _chunkWidth = 10;
    public int _chunkHeight = 150;
    public float _blockSize = 1f;
    public int _renderDistance = 4;
    public int _loadDistance = 6;

    private Dictionary<Vector2Int,ChunkRenderer>_chunkRenderers;
    private Dictionary<Vector2Int,ChunkData>_chunkDatas;
    private List<ChunkData>_renderQueue = new List<ChunkData>();
    private bool _isRendering = false;

    

    private void Awake()
    {
        _chunkRenderers = new Dictionary<Vector2Int, ChunkRenderer>();
        _chunkDatas = new Dictionary<Vector2Int, ChunkData>();
        SpawnPlayer();
        
    }
    private void SpawnPlayer()
    {
        var player = Instantiate(_playerPrefab);
        player.transform.position = new Vector3(_chunkWidth*_blockSize/2,1.5f*_chunkHeight*_blockSize+1,_chunkWidth*_blockSize/2);
        player.GetComponent<PlayerMovement>().SetWorld(this);
    }
    public ChunkData GetChunkDataOfCertainChunk(Vector2Int coordinates)
    {
        _chunkDatas.TryGetValue(coordinates,out ChunkData chunkData);
        return chunkData;
    }
    private void UpdateChunk(ChunkData chunkData)
    {
        chunkData.UpdateMeshData(_blockSize,_blockDatabase);
        _chunkRenderers.TryGetValue(chunkData._chunkCoordinates,out ChunkRenderer chunkRenderer);
        if (chunkRenderer==null)
        {
            RenderChunk(chunkData);
        }
        else
        {
            chunkRenderer.Render(_blockSize,chunkData);
        }
    }
    public bool ModifyBlock(Vector2Int chunkCoordinates,Vector3Int blockpos,BlockType newBlock)
    {
        if (_chunkDatas.TryGetValue(chunkCoordinates, out ChunkData chunkData))
        {
            chunkData.ModifyBlock(blockpos,newBlock);
            UpdateChunk(chunkData);
            if (blockpos.x==_chunkWidth-1)
            {
                if (_chunkDatas.TryGetValue(chunkCoordinates+Vector2Int.left, out ChunkData neighbor))
                {
                    UpdateChunk(neighbor);
                }
            }
            if (blockpos.x==0)
            {
                if (_chunkDatas.TryGetValue(chunkCoordinates+Vector2Int.right, out ChunkData neighbor))
                {
                    UpdateChunk(neighbor);
                }
            }
            if (blockpos.z==_chunkWidth-1)
            {
                if (_chunkDatas.TryGetValue(chunkCoordinates+Vector2Int.up, out ChunkData neighbor))
                {
                    UpdateChunk(neighbor);
                }
            }
            if (blockpos.z==0)
            {
                if (_chunkDatas.TryGetValue(chunkCoordinates+Vector2Int.down, out ChunkData neighbor))
                {
                    UpdateChunk(neighbor);
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
    /*ipublic void RenderLoadedChunks(Vector2Int chunkCoordinatesCenter)
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

                f (_world.TryGetValue(new Vector2Int(x,y)+Vector2Int.left, out ChunkRenderer neighbor))
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
                }
            }
        }
        
    }*/
    private ChunkData GetChunkNeighbor(Vector2Int chunkCoordinates,Vector2Int offset)
    {
        _chunkDatas.TryGetValue(chunkCoordinates+offset,out ChunkData result);
        return result;
    }
    private List<ChunkData> GetChunkNeighbors(Vector2Int chunkCoordinates)
    {
        List<ChunkData>result = new List<ChunkData>();
        ChunkData neighborRight = GetChunkNeighbor(chunkCoordinates,new Vector2Int(1,0));
        ChunkData neighborLeft = GetChunkNeighbor(chunkCoordinates,new Vector2Int(-1,0));
        ChunkData neighborUp = GetChunkNeighbor(chunkCoordinates,new Vector2Int(0,1));
        ChunkData neighborDown = GetChunkNeighbor(chunkCoordinates,new Vector2Int(0,-1));
        if (neighborRight!=null)
            result.Add(neighborRight);
        if (neighborLeft!=null)
            result.Add(neighborLeft);
        if (neighborUp!=null)
            result.Add(neighborUp);
        if (neighborDown!=null)
            result.Add(neighborDown);
        return result;
    }
    public void LoadChunksAround(Vector2Int chunkCoordinatesCenter)
    {
        List<ChunkData>newChunks = new List<ChunkData>();
        for (int x = chunkCoordinatesCenter.x-_loadDistance;x<chunkCoordinatesCenter.x+_loadDistance;x++)
        {
            for (int y = chunkCoordinatesCenter.y-_loadDistance;y<chunkCoordinatesCenter.y+_loadDistance;y++)
            {
                if (_chunkDatas.ContainsKey(new Vector2Int(x,y))) continue;
                LoadChunkAtPosition(new Vector2Int(x,y));
                _chunkDatas.TryGetValue(new Vector2Int(x,y),out ChunkData chunkData);
                if (chunkData!=null)
                    newChunks.Add(chunkData);
            }
        }
        foreach (ChunkData chunkData in newChunks)
        {
            chunkData.UpdateMeshData(_blockSize,_blockDatabase);
            AddChunkToRenderQueue(chunkData);
        }
        List<ChunkData>chunksToUpdate = new List<ChunkData>();
        foreach (ChunkData chunkData in newChunks)
        {
            List<ChunkData>chunkNeighbors = GetChunkNeighbors(chunkData._chunkCoordinates);
            foreach (ChunkData neighbor in chunkNeighbors)
            {
                if (!newChunks.Contains(neighbor)
                    &&!chunksToUpdate.Contains(neighbor))
                {
                    chunksToUpdate.Add(neighbor);
                }
            }
        }
        foreach (ChunkData chunkData in chunksToUpdate)
        {
            UpdateChunk(chunkData);
        }
        if (!_isRendering)
        {
            _isRendering = true;
            StartCoroutine(StartRender());
        }
    }

    private void LoadChunkAtPosition(Vector2Int chunkCoordinates)
    {
        BlockType[,,]blocks = new BlockType[_chunkWidth,_chunkHeight,_chunkWidth];
        blocks = _terainGenerator.GenerateTerrain(chunkCoordinates.x,chunkCoordinates.y,_chunkWidth,_chunkHeight);
        ChunkData chunkData = new ChunkData(blocks,this);
        chunkData._chunkCoordinates = chunkCoordinates;
        _chunkDatas.Add(new Vector2Int(chunkCoordinates.x,chunkCoordinates.y),chunkData);

    }
    private void RenderChunk(ChunkData chunkData)
    {
        Vector2Int chunkCoordinates = chunkData._chunkCoordinates;
        var chunk =  Instantiate(_chunkPrefab);
        chunk.transform.position = new Vector3(_chunkWidth*chunkCoordinates.x*_blockSize,0,_chunkWidth*chunkCoordinates.y*_blockSize);
        ChunkRenderer chunkRenderer = chunk.GetComponent<ChunkRenderer>();
        chunkRenderer.Render(_blockSize,chunkData);
        _chunkRenderers.Add(new Vector2Int(chunkCoordinates.x,chunkCoordinates.y),chunkRenderer);
    }

    public Vector2Int worldPosToChunkCoordinates(Vector3 worldPos)
    {
        Vector3Int blockWorldPos = Vector3Int.FloorToInt(worldPos/_blockSize);
        return new Vector2Int(blockWorldPos.x/_chunkWidth,blockWorldPos.z/_chunkWidth);
    }
    private void AddChunkToRenderQueue(ChunkData chunkData)
    {
        if (_chunkRenderers.ContainsKey(chunkData._chunkCoordinates))
            return;
        _renderQueue.Add(chunkData);
        //StartCoroutine(StartRender());

        
    }
    private IEnumerator StartRender()
    {
        while (_renderQueue.Count>0)
        {
            ChunkData chunkData = _renderQueue[0];
            _renderQueue.RemoveAt(0);
            if (!_chunkRenderers.ContainsKey(chunkData._chunkCoordinates))
            {
                RenderChunk(chunkData);
                yield return new WaitForEndOfFrame();
            }
        }
        _isRendering = false;
        yield return null;
    }
}
