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

/// <summary>
/// This is the main Class for the random map generation
/// </summary>
public class MapGenerator : MonoBehaviour
{
    /// <summary>White ground plane as a prefab </summary>
    public Transform groundPrefabWhite;
    /// <summary>black ground plane as a prefab </summary>
    public Transform groundPrefabBlack;
    /// <summary>the X and Y Size of the map to generate </summary>
    public Coord mapSize;
    /// <summary>The position where the main character starts from </summary>
    public Coord startingCoord;

    /// <summary>Size of a ground plane </summary>
    public float tileSize;

    void Start()
    {
        GenerateMap();
    }
    /// <summary>
    /// This generates the map based on the given parameters in this class
    /// </summary>
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

    /// <summary>
    /// This checks with the FloodFill algorithm whether the map is fully accessable or not
    /// </summary>
    /// <param name="obstacleMap">this is a map of the NEW obstacle placement (false = no obstacle ; true = obstacle)</param>
    /// <param name="currentObstacleCount">this is the total number of placed obstacle</param>
    /// <returns>this function returns whether the map is fully accessable or not</returns>
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
