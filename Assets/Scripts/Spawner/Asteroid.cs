using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Spawnable {
	[SerializeField] private float AmountOfFuelToDecrease = 10f;
    public override void Start(){
        base.Start();
		transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

	public override void Update(){
        if(GameManager.instance.IsGameOver && !GameManager.instance.IsFightingAliens) return;
        transform.Translate(Vector3.down * Speed * Time.deltaTime, Space.World);
    }

    /// <summary>
    /// Called when hitting the player
    /// </summary>
    public override void OnHitPlayer() {
        AudioManager.Instance.PlayExpolsion();
        Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
        GameManager.instance.OnHitPlayer(gameObject, AmountOfFuelToDecrease);
    }
}