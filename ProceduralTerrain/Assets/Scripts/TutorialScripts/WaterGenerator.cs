using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    private int width = MapGenerator.mapChunkSize -1 ;
    private int depth = MapGenerator.mapChunkSize -1;

    [SerializeField] private float waveFrequency = 2f;
    [SerializeField] private float waveAmplitude = 1f;

    

    Mesh mesh;
    Vector3[] verticies;
    int[] triangles;

    Vector2[] uv;
    Color[] colors;
    


    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        width = MapGenerator.mapChunkSize - 1;
        depth = MapGenerator.mapChunkSize - 1;


        CreateGrid();
        UpdateMesh();

    }
    private void Update()
    {
        
        for (int z = 0, i = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                verticies[i].y = Mathf.Sin(Time.time * waveFrequency + x) * waveAmplitude
                + Mathf.Cos(Time.time * waveFrequency * 0.5f + z) * waveAmplitude * 0.5f
                + Mathf.Sin(Time.time * waveFrequency * 1.5f + x + z) * waveAmplitude * 0.9f;
            }
        }

        UpdateMesh();
    }


    void CreateGrid()
    {
        verticies = new Vector3[(1 + width) * (1 + depth)];

        for (int z = 0, i = 0; z <= depth; z++)
        {
            for (int x = 0; x <= width; x++, i++)
            {
                verticies[i] = new Vector3(x,0,z);

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
        
        uv = new Vector2[verticies.Length];
        for(int z = 0, i=0; z<= depth; z++)
        {
            for (int x = 0;x <= width; x++, i++)
            {
                uv[i] = new Vector2(x*1f / width, z*1f / depth);
            }
        }
        
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.colors = colors;


        mesh.RecalculateNormals();
    }

}

