using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{

    public int Width;
    public int Depth;
    public int ChunkDimention;
    public float TerrainHeight;
    public Gradient TerrainColorGradient;
    public Material mTerrain;

    private void Start()
    {
        StartCoroutine(GenerateTerrain());

        Camera.main.transform.position = new Vector3(Width / 2f * ChunkDimention,TerrainHeight, Depth / 10f * ChunkDimention);
        Camera.main.transform.LookAt(new Vector3(Width / 2f * ChunkDimention, 0, Depth / 10f * ChunkDimention));
    }

    IEnumerator GenerateTerrain()
    {
        for(int z = 0; z<Depth; z++)
        {
            for (int x = 0; x<Width; x++)
            {
                GameObject go = new GameObject("Chunk_"+x+"_"+z, 
                    typeof(MeshFilter),
                    typeof(MeshRenderer),
                    typeof(MeshGenerator));

                go.transform.parent = transform;
                go.transform.localPosition = new Vector3(x * ChunkDimention, 0, z * ChunkDimention);
                go.GetComponent<MeshRenderer>().material = mTerrain;
                go.GetComponent<MeshGenerator>().Init(ChunkDimention, TerrainHeight, x, z, TerrainColorGradient);
                yield return new WaitForEndOfFrame();
            }
        }

        yield return true;
    }
}
