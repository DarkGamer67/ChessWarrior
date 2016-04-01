using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{

    public Transform groundPrefabWhite;
    public Transform groundPrefabBlack;
    public Vector2 mapSize;
    public Coord startingCoord;

    public float tileSize;

    void Start()
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
                Coord tileCoord = new Coord(x, y);

                if ((x + y) % 2 == 1)
                {
                    newTile = Instantiate(groundPrefabBlack, Coord.ToVector3(tileCoord), Quaternion.Euler(Vector3.right * 90)) as Transform;

                }
                else
                {
                    newTile = Instantiate(groundPrefabWhite, Coord.ToVector3(tileCoord), Quaternion.Euler(Vector3.right * 90)) as Transform;
                }
                newTile.parent = mapHolder;
                newTile.localScale = Vector3.one * tileSize;
            }
        }
    }

    bool MapIsFullyAccesible(bool[,] obstacleMap, int currentObstacleCount)
    {
        int accessibleTileCount = 1;
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Coord> queue = new Queue<Coord>();
        queue.Enqueue(startingCoord);
        mapFlags[startingCoord.x, startingCoord.y] = true;

        while(queue.Count > 0)
        {
            Coord currentCoord = queue.Dequeue();
            for(int x = -2; x <= 2; x++)
            {
                for (int y = -2; y <= 2; y++)
                {
                    if(!(Mathf.Abs(x) == 2 && Mathf.Abs(y) == 2) || (Mathf.Abs(x) == 2 && y == 0) || (x == 0 && Mathf.Abs(y) == 2) || (x == 0 && y == 0))
                    {
                        int neighbourX = currentCoord.x + x;
                        int neighbourY = currentCoord.y + y;
                        if ((neighbourX >= 0 && neighbourX < mapSize.x) && (neighbourY >= 0 && neighbourY < mapSize.y))
                        {
                            accessibleTileCount++;
                            queue.Enqueue(new Coord(neighbourX, neighbourY));
                            mapFlags[neighbourX, neighbourY] = true;
                        }
                    }
                }
            }
        }
        return accessibleTileCount == mapSize.y * mapSize.x - currentObstacleCount;

    }
}