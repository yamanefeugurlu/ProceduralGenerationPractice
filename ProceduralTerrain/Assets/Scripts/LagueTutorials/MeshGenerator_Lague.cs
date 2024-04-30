using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator_Lague 
{
    public static MeshData GenerateTerrainMesh ( float [,] heightMap, float heightMultiplier, AnimationCurve _heightCurve, int levelOfDetail)
    {
        AnimationCurve heightCurve = new AnimationCurve(_heightCurve.keys);
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail == 0)? 1 : levelOfDetail * 2;
        int verteciesPerLine = (width - 1) / meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verteciesPerLine, verteciesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y< height; y += meshSimplificationIncrement)
        {
            for (int x= 0; x<width; x += meshSimplificationIncrement)
            {
                meshData.verticies[vertexIndex] = new Vector3(topLeftX+x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ-y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if(x < width -1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verteciesPerLine + 1, vertexIndex + verteciesPerLine);
                    meshData.AddTriangle(vertexIndex + verteciesPerLine + 1,vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uvs;


    int triangleIndex;

    public MeshData (int meshWidth, int meshHeight)
    {
        verticies = new Vector3[meshHeight * meshWidth];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}
