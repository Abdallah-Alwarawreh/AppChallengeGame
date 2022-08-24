using UnityEngine;


public class FuelCanSpawnable : Spawnable{
    [SerializeField] private float AddFuelAmount = 20f;

    public override void OnTriggerEnter(Collider col){ 
        if (col.CompareTag("Player")){
            OnHitPlayer();
        }else if (!col.CompareTag("Player") && !col.CompareTag("Alien")){
            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// Called when hitting the player
    /// </summary>
    public override void OnHitPlayer(){
        AudioManager.Instance.PlayFuelPickup();
        GameManager.instance.AddFuel(AddFuelAmount);
        Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
        Destroy(gameObject);
    }
}
