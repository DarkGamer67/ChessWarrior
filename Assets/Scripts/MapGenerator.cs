using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/*
*
*  Copyright 2016 Utku Pazarci
*
*  Licensed under the Apache License, Version 2.0 (the "License");
*  you may not use this file except in compliance with the License.
*  You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
*  Unless required by applicable law or agreed to in writing, software
*  distributed under the License is distributed on an "AS IS" BASIS,
*  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
*  See the License for the specific language governing permissions and
*  limitations under the License.
*/
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
