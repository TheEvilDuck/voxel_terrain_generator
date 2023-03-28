using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]

public class ChunkRenderer: MonoBehaviour
{
    
    public  ChunkData _chunkData;
    public MeshData _meshData;
    public bool Rendered;
    private static ProfilerMarker MeshingMarker = new ProfilerMarker(ProfilerCategory.Loading,"Meshing chunks");
    [SerializeField]BlockDatabase _blockDatabase;
    public void Render(float blockSize)
    {
        MeshingMarker.Begin();
        Mesh mesh = new Mesh();
        _meshData = new MeshData();
        _meshData.Clear();

        _meshData.GenerateMeshData(_chunkData,blockSize,_blockDatabase);

        
        mesh.vertices = _meshData._verticies.ToArray();
        mesh.triangles = _meshData._triangles.ToArray();
        mesh.uv = _meshData._uvs.ToArray();
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
        GetComponent<MeshFilter>().mesh = mesh;
        
        MeshingMarker.End();
    }
    /*public void UnRender()
    {
        _triangles.Clear();
        _verticies.Clear();
        _uvs.Clear();
        Destroy(gameObject);
    }*/

    public void SetChunkData(BlockType[,,]blocks)
    {
        _chunkData = new ChunkData();
        _chunkData._blocks = blocks;
    }
    public void SetWorld(GameWorld world)
    {
        _chunkData._world = world;
    }
    public void SetChunkCoordinates(Vector2Int coordinates)
    {
        _chunkData._chunkCoordinates = coordinates;
    }
    public ChunkData GetChunkData()
    {
        return _chunkData;
    }
    
}
