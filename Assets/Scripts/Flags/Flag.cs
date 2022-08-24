using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour {
    [SerializeField] MeshRenderer[] Renderers;
    private void Start(){
        string CurrentFlagCode = PlayerPrefs.GetString("FlagCode", "jo");
        Texture2D flag = (Texture2D)Resources.Load("Flags/" + CurrentFlagCode);
        foreach(MeshRenderer renderer in Renderers){
            renderer.material.mainTexture = flag;
        }
        
    }
}