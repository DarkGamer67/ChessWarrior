using System;
using UnityEngine;

public static class Utility
{
    public static T[] ShuffleArray<T>(T[] arrayToShuffle, int seed)
    {
        System.Random random = new System.Random(seed);
        for(int i = 0; i < arrayToShuffle.Length - 1; i++)
        {
            int randomNumber = random.Next(i, arrayToShuffle.Length);

            T tempObject = arrayToShuffle[i];
            arrayToShuffle[i] = arrayToShuffle[randomNumber];
            arrayToShuffle[randomNumber] = tempObject;
        }
        return arrayToShuffle;
    }
    public static T[] ShuffleArray<T>(T[] arrayToShuffle)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < arrayToShuffle.Length - 1; i++)
        {
            int randomNumber = random.Next(i, arrayToShuffle.Length);

            T tempObject = arrayToShuffle[i];
            arrayToShuffle[i] = arrayToShuffle[randomNumber];
            arrayToShuffle[randomNumber] = tempObject;
        }
        return arrayToShuffle;
    }
}

[Serializable]
public class Coord
{
    
    public int x;
    public int y;

    public Coord(int x, int y)
    {
        this.x = x;
        this.y = y;
        
    }

    public static Vector3 ToVector3(Coord coord)
    {
        float tileSize = GameObject.FindObjectOfType<MapGenerator>().GetComponent<MapGenerator>().tileSize;
        return new Vector3(coord.x * tileSize, 0, coord.y * tileSize);
    }

    public static bool operator ==(Coord c1, Coord c2)
    {
        return ((c1.x == c2.x) && (c1.y == c2.y));
    }
    public static bool operator !=(Coord c1, Coord c2)
    {
        return !(c1 == c2);
    }

}