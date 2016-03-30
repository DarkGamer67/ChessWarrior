using UnityEngine;
using System.Collections;

public class MapGenerator : MonoBehaviour
{

    public Transform groundPrefabWhite;
    public Transform groundPrefabBlack;
    public Vector2 mapSize;

    public float tileSize;

    // Use this for initialization
    void Start ()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        string holderName = "Generated Map";
        if (transform.FindChild(holderName))
        {
            DestroyImmediate(transform.FindChild(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Transform newTile;
                if ((x + y) % 2 == 1)
                {
                    newTile = Instantiate(groundPrefabBlack, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(Vector3.right * 90)) as Transform;

                }
                else
                {
                    newTile = Instantiate(groundPrefabWhite, new Vector3(x * tileSize, 0, y * tileSize), Quaternion.Euler(Vector3.right * 90)) as Transform;
                }
                newTile.parent = mapHolder;
                newTile.localScale = new Vector3(tileSize, 0, tileSize);
            }
        }


    }
}
