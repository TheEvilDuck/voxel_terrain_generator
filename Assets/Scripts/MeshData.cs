using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshData
{
    public List<int>_triangles = new List<int>();
    public List<Vector3>_verticies = new List<Vector3>();
    public List<Vector2>_uvs = new List<Vector2>();

    public void Clear()
    {
        _triangles.Clear();
        _verticies.Clear();
        _uvs.Clear();
    }
    public void GenerateMeshData(ChunkData chunkData, float blockSize, BlockDatabase blockDatabase)
    {
        for (int x = 0;x<chunkData._blocks.GetLength(0);x++)
        {
            for (int y = 0;y<chunkData._blocks.GetLength(1);y++)
            {
                for (int z = 0;z<chunkData._blocks.GetLength(2);z++)
                {
                    if (chunkData._blocks[x,y,z]!=0)
                        GenerateBlock(new Vector3Int(x,y,z),blockSize, chunkData, blockDatabase);
                }
            }
        }
    }
    private void GenerateBlock(Vector3Int blockPos,float blockSize, ChunkData chunkData, BlockDatabase blockDatabase)
    {
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.right)==BlockType.Air) GererateRightSide(blockPos,blockSize,chunkData,blockDatabase);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.left)==BlockType.Air)GererateLeftSide(blockPos,blockSize,chunkData,blockDatabase);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.forward)==BlockType.Air)GererateFrontSide(blockPos,blockSize,chunkData,blockDatabase);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.back)==BlockType.Air)GererateBackSide(blockPos,blockSize,chunkData,blockDatabase);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.down)==BlockType.Air)GenerateBottomSide(blockPos,blockSize,chunkData,blockDatabase);
        if (chunkData.GetBlockAtPosition(blockPos+Vector3Int.up)==BlockType.Air)GererateUpSide(blockPos,blockSize,chunkData,blockDatabase);
        
    }

    private void GererateLeftSide(Vector3Int blockPos,float blockSize, ChunkData chunkData, BlockDatabase blockDatabase)
    {
        _verticies.Add(blockSize*(new Vector3(0,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData,blockDatabase);
    }
    private void GererateRightSide(Vector3Int blockPos,float blockSize,ChunkData chunkData,BlockDatabase blockDatabase)
    {
        _verticies.Add(blockSize*(new Vector3(1,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData,blockDatabase);
    }
    private void GererateFrontSide(Vector3Int blockPos,float blockSize,ChunkData chunkData,BlockDatabase blockDatabase)
    {
        _verticies.Add(blockSize*(new Vector3(0,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData,blockDatabase);
    }
    private void GererateBackSide(Vector3Int blockPos,float blockSize,ChunkData chunkData,BlockDatabase blockDatabase)
    {
        _verticies.Add(blockSize*(new Vector3(0,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,0)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData,blockDatabase);
    }
    private void GererateUpSide(Vector3Int blockPos,float blockSize,ChunkData chunkData,BlockDatabase blockDatabase)
    {
        _verticies.Add(blockSize*(new Vector3(0,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,1,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,1,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData,blockDatabase);
    }
    private void GenerateBottomSide(Vector3Int blockPos,float blockSize,ChunkData chunkData,BlockDatabase blockDatabase)
    {
        if (blockPos.y==0)return;
        _verticies.Add(blockSize*(new Vector3(0,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,0)+blockPos));
        _verticies.Add(blockSize*(new Vector3(0,0,1)+blockPos));
        _verticies.Add(blockSize*(new Vector3(1,0,1)+blockPos));

        AddTriangles();
        AddUvs(blockPos,blockSize,chunkData,blockDatabase);
    }

    private void AddTriangles()
    {
        
        _triangles.Add(_verticies.Count-4);
        _triangles.Add(_verticies.Count-3);
        _triangles.Add(_verticies.Count-2);

        _triangles.Add(_verticies.Count-3);
        _triangles.Add(_verticies.Count-1);
        _triangles.Add(_verticies.Count-2);
    }
    private void AddUvs(Vector3Int blockPos,float blockSize, ChunkData chunkData, BlockDatabase blockDatabase)
    {
        Vector2 blockUvSize = new Vector2(16f/256f,16f/256f);

        Vector2Int blockId = new Vector2Int(0,3);

        BlockType block =  chunkData.GetBlockAtPosition(blockPos);

        if (block!=BlockType.Air)
        {
            BlockInfo blockInfo = blockDatabase.GetBlockInfo(block);
            if (blockInfo!=null)
                blockId = blockInfo.blockIdOnTexture;
        }

        Vector2 offset = new Vector2((float)blockId.x*blockUvSize.x,1f-(float)(blockId.y+1)*blockUvSize.y);
        

        _uvs.Add(offset);
        _uvs.Add(offset);
        _uvs.Add(offset);
        _uvs.Add(offset);
    }
}
