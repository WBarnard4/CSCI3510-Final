using UnityEngine;
using Unity.AI.Navigation;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe; // Required for Safety Restriction attribute
using Unity.Jobs;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(NavMeshSurface))]
public class TerrainGenerator : MonoBehaviour
{
    [Header("Map Settings")]
    public int xSize = 200;
    public int zSize = 200;
    public float scale = 0.05f; 
    public float heightMultiplier = 10f;
    public bool randomizeOnStart = true;
    private Vector2 offset;

    [Header("Texture Settings")]
    public Gradient terrainColor; 

    private Mesh mesh;

    void Start()
    {

        if (randomizeOnStart)
        {
            offset = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        }
        BuildMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        
        // Check for NavMeshSurface to avoid errors if missing
        var navSurface = GetComponent<NavMeshSurface>();
        if (navSurface != null) navSurface.BuildNavMesh();
    }

    void BuildMesh()
    {
        int vertCount = (xSize + 1) * (zSize + 1);
        int indexCount = xSize * zSize * 6;

        var meshDataArray = Mesh.AllocateWritableMeshData(1);
        var meshData = meshDataArray[0];
        

        // 1. Setup Vertex Attributes (Positions in Stream 0, Normals in Stream 1)
        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(2, Allocator.Temp);
        vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, stream: 0);
        vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, stream: 1);
        
        meshData.SetVertexBufferParams(vertCount, vertexAttributes);
        vertexAttributes.Dispose();

        meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

        // 2. Get Data Views
        // We explicitly request stream 0 for vertices to avoid writing to the Normal buffer
        var vertices = meshData.GetVertexData<Vector3>(stream: 0);
        var indices = meshData.GetIndexData<int>();

        var job = new GenerateMeshJob
        {
            xSize = xSize,
            zSize = zSize,
            scale = scale,
            heightMultiplier = heightMultiplier,
            offset = offset,
            vertices = vertices,
            indices = indices
        };
        
        job.ScheduleParallel(vertices.Length, 64, default).Complete();

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount), MeshUpdateFlags.DontRecalculateBounds);

        mesh = new Mesh { name = "ProceduralTerrain" };
        
        // 3. Apply Data & Calculate Normals
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh, MeshUpdateFlags.DontRecalculateBounds);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals(); // This now works because we allocated Stream 1

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    [BurstCompile]
    struct GenerateMeshJob : IJobFor
    {
        public int xSize, zSize;
        public float scale, heightMultiplier;
        public Vector2 offset;

        // Fix for "Aliasing" error: Disable safety check for overlapping memory pointers
        [NativeDisableContainerSafetyRestriction] 
        public NativeArray<Vector3> vertices;

        // Fix for "Aliasing" error + Parallel writing restriction
        [NativeDisableContainerSafetyRestriction]
        [NativeDisableParallelForRestriction] 
        public NativeArray<int> indices;

        public void Execute(int i)
        {
int z = i / (xSize + 1);
            int x = i - z * (xSize + 1);

            // Add the offset to the coordinates here
            float xCoord = (x * scale) + offset.x;
            float zCoord = (z * scale) + offset.y;

            float y = Mathf.PerlinNoise(xCoord, zCoord) * heightMultiplier;
            vertices[i] = new Vector3(x - xSize / 2f, y, z - zSize / 2f);

            if (x < xSize && z < zSize)
            {
                int vert = i;
                int tris = (z * xSize + x) * 6;
                indices[tris + 0] = vert;
                indices[tris + 1] = vert + xSize + 1;
                indices[tris + 2] = vert + 1;
                indices[tris + 3] = vert + 1;
                indices[tris + 4] = vert + xSize + 1;
                indices[tris + 5] = vert + xSize + 2;
            }
        }
    }
}