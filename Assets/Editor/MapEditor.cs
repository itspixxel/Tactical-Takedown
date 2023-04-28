using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof(TileSpawner))]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TileSpawner tileSpawner = (TileSpawner)target;

        tileSpawner.GenerateLevel();
    }
}
