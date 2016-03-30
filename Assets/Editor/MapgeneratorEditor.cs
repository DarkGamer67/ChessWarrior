using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator map = target as MapGenerator;
        if (DrawDefaultInspector())
        {
            map.GenerateMap();
        }
        if (GUILayout.Button("Generate Map"))
        {
            map.GenerateMap();
        }
    }

}