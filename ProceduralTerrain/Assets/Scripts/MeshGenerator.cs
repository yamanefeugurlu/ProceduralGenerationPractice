using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private int width = 20;
    [SerializeField] private int depth = 20;
    
    [SerializeField] private float waveFrequency = 2f; 
    [SerializeField] private float waveAmplitude = 1f;

    int xOffset,zOffset;

    [Header("Perlin Params")]
    [SerializeField] private float scale = 20;
    [SerializeField] private float minTerrainHeight = 0;
    [SerializeField] private float maxTerrainHeight = 10;
    [SerializeField] private float terrainHeight = 10;
    [SerializeField] private int perlinOctave = 3;
    [SerializeField] private float lacunarity = 2;
    [SerializeField] private float persistance = 0.5f;

    Mesh mesh;
    Vector3[] verticies;
    int[] triangles;

    Vector2[] uv;
    Color[] colors;
    private Gradient terrainColorGradient;

    public void Init(int chunkDimention, float terrainHeight,int xOffset, int zOffset, Gradient gradient)
    {
        width = chunkDimention;
        depth = chunkDimention;
        this.terrainHeight = terrainHeight;
        this.xOffset = xOffset;
        this.zOffset = zOffset;
        terrainColorGradient = gradient;
    }
    

    

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        

        //CreateQuad();
        CreateGrid();
        UpdateMesh();

        
    }
    private void Update()
    {
        //mimic ocean waves
        //for (int z = 0, i = 0; z <= depth; z++)
        //{
        //    for (int x = 0; x <= width; x++, i++)
        //    {
        //        verticies[i].y = Mathf.Sin(Time.time * waveFrequency + x) * waveAmplitude
        //        + Mathf.Cos(Time.time * waveFrequency * 0.5f + z) * waveAmplitude * 0.5f
        //        + Mathf.Sin(Time.time * waveFrequency * 1.5f + x + z) * waveAmplitude * 0.9f;
        //    }
        //}

        UpdateMesh();
    }


    void CreateGrid()
    {
        verticies = new Vector3[(1 + width) * (1 + depth)];

        minTerrainHeight = terrainHeight;
        maxTerrainHeight = -terrainHeight;

        for (int z = 0, i = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                verticies[i] = new Vector3(x, 
                    GetHeight(x + (xOffset*width),z + (zOffset* depth), scale, perlinOctave, lacunarity, persistance) * terrainHeight,
                    z);

                if (verticies[i].y < minTerrainHeight) minTerrainHeight = verticies[i].y;
                else if(verticies[i].y > maxTerrainHeight) maxTerrainHeight = verticies[i].y;
            }
        }

        triangles = new int[6 * width * depth];

        int currentVertice = 0;
        int numOfTriangleVertices = 0;

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                triangles[numOfTriangleVertices + 0] = currentVertice + z + 0;
                triangles[numOfTriangleVertices + 1] = currentVertice + z + width + 1;
                triangles[numOfTriangleVertices + 2] = currentVertice + z + 1;
                triangles[numOfTriangleVertices + 3] = currentVertice + z + 1;
                triangles[numOfTriangleVertices + 4] = currentVertice + z + width + 1;
                triangles[numOfTriangleVertices + 5] = currentVertice + z + width + 2;

                numOfTriangleVertices += 6;
                currentVertice++;
            }
        }

        //for UV, for texture on our created mesh
        /*
        uv = new Vector2[verticies.Length];
        for(int z = 0, i=0; z<= depth; z++)
        {
            for (int x = 0;x <= width; x++, i++)
            {
                uv[i] = new Vector2(x*1f / width, z*1f / depth);
            }
        }
        */

        colors = new Color[verticies.Length];
        for (int z = 0, i = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                // you can put this line inside vertice calc loop but we do it seperately in class context
                colors[i] = terrainColorGradient.Evaluate(Mathf.InverseLerp(minTerrainHeight,maxTerrainHeight, verticies[i].y)); 
            }
        }
    }

    float GetHeight(float x, float z, float scale, int octaves, float lacunarity, float persistance)
    {
        if (scale <= 0) scale = 0.0001f;

        float height = 0;
        float frequency = 1f;
        float amplitude = 1f;

        for(int i = 0; i< octaves; i++)
        {
            float perlinValue = Mathf.PerlinNoise((x / scale) * frequency, (z / scale) * frequency);
            height += perlinValue * amplitude;

            frequency *= lacunarity;
            amplitude *= persistance;
        }

        

        return height;
    }

    void CreateQuad()
    {
       
        verticies = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1),
        };

        triangles = new int[]
        {
            0,1,2,
            1,3,2 // 1 3 2 because the triangles are calculated in clockwise direction 1 2 3 yields the face to look down but 1 3 2 will render the triangle face looking up
        };

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        //mesh.uv = uv;
        mesh.colors = colors;


        mesh.RecalculateNormals(); 
    }

    private void OnDrawGizmos()
    {
        //if (verticies == null) return;

        //for(int i = 0; i < verticies.Length; i++)
        //{
        //    Gizmos.DrawSphere(verticies[i], 0.1f);
        //}
    }
}
