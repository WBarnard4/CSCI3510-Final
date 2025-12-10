using UnityEngine;
using Unity.AI.Navigation;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
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
    [Range(0.01f, 0.1f)] public float scale = 0.05f; 
    [Range(5f, 30f)] public float heightMultiplier = 10f;
    public int octaves = 4;
    [Range(0.1f, 1f)] public float persistence = 0.5f;
    [Range(1f, 4f)] public float lacunarity = 2f;
    public bool randomizeOnStart = true;
    private Vector2 offset;

    [Header("Texture Settings")]
    public Texture2D[] terrainTextures; 
    public Texture2D normalMap;
    public float textureTiling = 1f;

    private Mesh mesh;

    void Start()
    {
        if (randomizeOnStart)
        {
            offset = new Vector2(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
        }
        
        var renderer = GetComponent<MeshRenderer>();
        if (renderer.sharedMaterial == null) 
            renderer.material = new Material(Shader.Find("Custom/TerrainShader"));
            
        BuildMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
        
        var navSurface = GetComponent<NavMeshSurface>();
        if (navSurface != null) navSurface.BuildNavMesh();
    }

    void BuildMesh()
    {
        int vertCount = (xSize + 1) * (zSize + 1);
        int indexCount = xSize * zSize * 6;

        var meshDataArray = Mesh.AllocateWritableMeshData(1);
        var meshData = meshDataArray[0];

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(3, Allocator.Temp);
        vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, stream: 0);
        vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, stream: 1);
        vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, stream: 2);
        meshData.SetVertexBufferParams(vertCount, vertexAttributes);
        vertexAttributes.Dispose();

        meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

        var vertices = meshData.GetVertexData<Vector3>(stream: 0);
        var uvs = meshData.GetVertexData<Vector2>(stream: 2);
        var indices = meshData.GetIndexData<int>();

        var job = new GenerateMeshJob
        {
            xSize = xSize,
            zSize = zSize,
            scale = scale,
            heightMultiplier = heightMultiplier,
            octaves = octaves,
            persistence = persistence,
            lacunarity = lacunarity,
            offset = offset,
            vertices = vertices,
            uvs = uvs,
            indices = indices
        };
        
        job.ScheduleParallel(vertices.Length, 64, default).Complete();

        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount), MeshUpdateFlags.DontRecalculateBounds);

        mesh = new Mesh { name = "ProceduralTerrain" };
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh, MeshUpdateFlags.DontRecalculateBounds);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        var renderer = GetComponent<MeshRenderer>();
        if (terrainTextures != null && terrainTextures.Length > 0)
        {
            Material mat = renderer.sharedMaterial;
            for (int i = 0; i < Mathf.Min(terrainTextures.Length, 4); i++)
            {
                mat.SetTexture("_Texture" + i, terrainTextures[i]);
            }
            if (normalMap != null) mat.SetTexture("_BumpMap", normalMap);
            mat.SetFloat("_Tiling", textureTiling);
        }

        GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    [BurstCompile]
    struct GenerateMeshJob : IJobFor
    {
        public int xSize, zSize;
        public float scale, heightMultiplier;
        public int octaves;
        public float persistence, lacunarity;
        public Vector2 offset;
        [NativeDisableContainerSafetyRestriction] public NativeArray<Vector3> vertices;
        [NativeDisableContainerSafetyRestriction] public NativeArray<Vector2> uvs;
        [NativeDisableContainerSafetyRestriction][NativeDisableParallelForRestriction] public NativeArray<int> indices;

        public void Execute(int i)
        {
            int z = i / (xSize + 1);
            int x = i - z * (xSize + 1);

            float xCoord = (x * scale) + offset.x;
            float zCoord = (z * scale) + offset.y;

            float y = 0f;
            float amplitude = 1f;
            float frequency = 1f;

            for (int o = 0; o < octaves; o++)
            {
                float sampleX = xCoord * frequency;
                float sampleZ = zCoord * frequency;
                y += Mathf.PerlinNoise(sampleX, sampleZ) * amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }

            y *= heightMultiplier;
            vertices[i] = new Vector3(x - xSize / 2f, y, z - zSize / 2f);
            uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);

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