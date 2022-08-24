using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Spawnable {
	[SerializeField] private float AmountOfFuelToDecrease = 10f;
    [SerializeField] private float RotationSpeedMax;
    [SerializeField] private float RotationSpeedMin;
    private Vector3 RotationSpeed;
    public override void Start(){
        base.Start();
        RotationSpeed = new Vector3(Random.Range(RotationSpeedMin, RotationSpeedMax), Random.Range(RotationSpeedMin, RotationSpeedMax), Random.Range(RotationSpeedMin, RotationSpeedMax));
    }

	public override void Update(){
        if(GameManager.instance.IsGameOver && !GameManager.instance.IsFightingAliens) return;
        transform.Translate(Vector3.down * Speed * Time.deltaTime, Space.World);
        transform.Rotate(RotationSpeed * Time.deltaTime);

    }

    /// <summary>
    /// Called when hitting the player
    /// </summary>
    public override void OnHitPlayer() {
        AudioManager.Instance.PlayPlateImpact();
        Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
        GameManager.instance.OnHitPlayer(gameObject, AmountOfFuelToDecrease);
    }
}