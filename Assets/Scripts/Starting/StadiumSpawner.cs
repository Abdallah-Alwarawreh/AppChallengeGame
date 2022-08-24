using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StadiumSpawner : MonoBehaviour{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int Rows;
    [SerializeField] private int Cols;
    [SerializeField] private Vector3 ColOffset;
    [SerializeField] private Vector3 RowOffset;
    [SerializeField, Range(0, 100)] private int SpwanChance = 70;
    [SerializeField] private int MaxAmount;
    void Start() {
        for (int i = 0; i < Rows; i++) {
            for (int j = 0; j < Cols; j++) {
                if (Random.Range(0, 100) < SpwanChance) {
                    GameObject person = Instantiate(Prefab, SpawnPoint);
                    person.transform.localPosition = (j * ColOffset) + (i * RowOffset);
                    
                    if (MaxAmount > 0) MaxAmount--;
                    else return;
                }
                
                
            }
        }
    }
}
