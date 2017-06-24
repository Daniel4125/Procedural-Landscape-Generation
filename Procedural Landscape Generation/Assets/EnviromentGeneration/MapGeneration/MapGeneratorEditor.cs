using UnityEngine;
using System.Collections;
using UnityEditor;

// Adds stuff to MapGenerator unity GUI
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI()
    {
        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector())
            if (mapGen.autoUpdate)
                mapGen.GenerateMap();

        if (GUILayout.Button("Generate"))
            mapGen.GenerateMap();       
    }              
}
