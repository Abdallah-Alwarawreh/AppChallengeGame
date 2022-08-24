using System;
using UnityEngine;


public class Spawnable : MonoBehaviour{
    [SerializeField] public float Speed;
    [SerializeField] public GameObject DestroyedEffect;

    public virtual void Start(){
        Destroy(gameObject, 5);
    }

    public virtual void Update(){
        if(GameManager.instance.IsGameOver && !GameManager.instance.IsFightingAliens) return;
        transform.Translate(-transform.up * Speed * Time.deltaTime);
    }

    public virtual void OnTriggerEnter(Collider col){ 
        if (col.CompareTag("Player")){
            OnHitPlayer();
        }
    }

    
    /// <summary>
    /// Called when hitting the player
    /// </summary>
    public virtual void OnHitPlayer() { }
}
