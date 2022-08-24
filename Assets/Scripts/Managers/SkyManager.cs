using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour{
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float MaterialSpeed;
    [SerializeField] private Material SkyMaterial;
    [SerializeField] private MeshRenderer[] SkyDomes;
    
    Material ClonedSkyMaterial;
    private float offset;
    private void Start(){
        ClonedSkyMaterial = new Material(SkyMaterial);
        foreach (var SkyDome in SkyDomes){
            SkyDome.material = ClonedSkyMaterial;
        }
    }

    void Update(){
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
        offset += MaterialSpeed * Time.deltaTime;
        ClonedSkyMaterial.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
