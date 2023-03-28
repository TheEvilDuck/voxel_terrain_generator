using UnityEngine;
using System.Collections.Generic;
using Unity.Profiling;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]

public class ChunkRenderer: MonoBehaviour
{
    
    public bool Rendered;
    private static ProfilerMarker MeshingMarker = new ProfilerMarker(ProfilerCategory.Loading,"Meshing chunks");
    public void Render(float blockSize, ChunkData chunkData)
    {
        MeshingMarker.Begin();
        Mesh mesh = new Mesh();

        
        mesh.vertices = chunkData._meshData._verticies.ToArray();
        mesh.triangles = chunkData._meshData._triangles.ToArray();
        mesh.uv = chunkData._meshData._uvs.ToArray();
        
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

    
}
