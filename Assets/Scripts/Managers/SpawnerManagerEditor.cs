#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnerManager))]
public class SpawnerManagerEditor : Editor {
    public override void OnInspectorGUI(){
        base.OnInspectorGUI();
        
        
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        
        if (Application.isPlaying) GUI.enabled = false;
        
        
        if (GUILayout.Button("Test spawning rates.")){
            SpawnerManager spawnerManager = (SpawnerManager)target;
            string result = spawnerManager.TestSpawning();
            EditorUtility.DisplayDialog("Results", result, "Close");
            
        }
    }
}

#endif