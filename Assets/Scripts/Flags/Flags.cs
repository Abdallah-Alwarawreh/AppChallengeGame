using System;
using System.Collections.Generic;
using UnityEngine;

public class Flags : MonoBehaviour{
    [SerializeField] List<string> FlagCodes = new List<string>();
    private void Start(){
        TextAsset flags = (TextAsset)Resources.Load("flags");
        string[] lines = flags.text.Split('\n');
        foreach(string line in lines){
            string[] data = line.Split('|');
            string flagCode = data[1].Replace(".png", "");
            FlagCodes.Add(flagCode);
        }
    }
}