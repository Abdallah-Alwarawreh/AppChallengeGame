using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunBall : Spawnable {
	
    [SerializeField] private float StunTime = 1f;

	public override void OnHitPlayer() {
        AudioManager.Instance.PlayZap();
        Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
        GameManager.instance.StunPlayer(StunTime);
		Destroy(gameObject);
    }
}