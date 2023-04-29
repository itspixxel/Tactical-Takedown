using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof(TileSpawner))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TileSpawner tileSpawner = (TileSpawner)target;

        if (DrawDefaultInspector())
        { 
            tileSpawner.GenerateLevel();
        }

        if (GUILayout.Button("Generate Level"))
        {
            tileSpawner.GenerateLevel();
        }

    }
}
