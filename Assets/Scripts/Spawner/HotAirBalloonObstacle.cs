using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HotAirBalloonObstacle : Spawnable{
    [SerializeField] private List<Color> Colors = new List<Color>();
    [SerializeField] private float AmountOfFuelToDecrease = 10f;
    public override void Start(){
        base.Start();
        GetComponentInChildren<MeshRenderer>().materials[1].color = Colors[Random.Range(0, Colors.Count)];
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
