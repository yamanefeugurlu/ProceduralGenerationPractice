using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGenerator : MonoBehaviour
{
    public static PropGenerator instance;
    private void Awake()
    {
        if (instance != null &&instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    [Header("Trees")]
    public GameObject treeGreen;
    public float minHeight_Tree;
    public float maxHeight_Tree;

    [Header("Rocks")]
    public GameObject rock;
    public float minHeight_Rock;
    public float maxHeight_Rock;

    [Header("Water")]
    public float waterHeight = 2f;
    public GameObject waterPrefab;
    public void PlaceWater(MeshFilter meshFilter, Transform parent)
    {
        GameObject water = Instantiate(waterPrefab, meshFilter.transform.localPosition, Quaternion.identity, parent);
        water.transform.localPosition = new Vector3(water.transform.localPosition.x, water.transform.localPosition.y + waterHeight, water.transform.localPosition.z);
    }
    public void PlaceTrees(MapData map,MeshFilter meshFilter, float scale, Transform parent)
    {
        int index = 0;
        float r = 0;
        float currentHeight = 0;
        for (int x = 0; x < map.heightMap.GetLength(0); x++)
        {
            for(int y = 0; y < map.heightMap.GetLength(1); y++)
            {
                r =  Random.Range(0, 100f);
                currentHeight = meshFilter.mesh.vertices[index].y * scale;
                Vector3 offset = meshFilter.transform.position;

                if(currentHeight > minHeight_Tree && currentHeight < maxHeight_Tree)
                {
                    if (r >= 98)
                    {
                        Debug.Log(currentHeight);
                        GameObject tree = Instantiate(treeGreen, meshFilter.mesh.vertices[index] * scale, Quaternion.identity, parent);
                        tree.transform.localPosition = new Vector3(tree.transform.localPosition.x + offset.x /scale,
                            tree.transform.localPosition.y + offset.y/scale,
                            tree.transform.localPosition.z + offset.z/scale);

                    }
                }
                index++;
            }
            
        }
    }

    public void PlaceRocks(MapData map, MeshFilter meshFilter, float scale, Transform parent)
    {
        int index = 0;
        float r = 0;
        float currentHeight = 0;
        for (int x = 0; x < map.heightMap.GetLength(0); x++)
        {
            for (int y = 0; y < map.heightMap.GetLength(1); y++)
            {
                r = Random.Range(0, 100f);
                currentHeight = meshFilter.mesh.vertices[index].y * scale;
                Vector3 offset = meshFilter.transform.position;

                if (currentHeight > minHeight_Rock && currentHeight < maxHeight_Rock)
                {
                    if (r >= 99.4f)
                    {
                        Debug.Log(currentHeight);
                        GameObject tree = Instantiate(rock, meshFilter.mesh.vertices[index] * scale, Quaternion.identity, parent);
                        tree.transform.localPosition = new Vector3(tree.transform.localPosition.x + offset.x / scale,
                            tree.transform.localPosition.y + offset.y / scale,
                            tree.transform.localPosition.z + offset.z / scale);

                    }
                }
                index++;
            }

        }
    }
}
