using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStar : Spawnable {
    [SerializeField] private float AmountOfFuelToDecrease = 10f;
    [SerializeField] private GameObject WarningSign;
    [SerializeField] private Vector3 SignOffset;
    [SerializeField] private Vector3 TransformOffset;
    [SerializeField] private Vector3 MoveOffset;
    private float timer = 0;
    private bool Go;
    private bool IsRight;
    private Vector3 TargetPos;
    public override void Start(){
        float RandomY = Random.Range(0, Screen.height);
        
        float y = Camera.main.ScreenToWorldPoint(new Vector3(0, RandomY, 16)).y;
        Vector3 Left = Camera.main.ScreenToWorldPoint(new Vector3(0, RandomY, 16));
        Vector3 Right = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, RandomY, 16));
        int r = Random.Range(0, 2);
        IsRight = r == 1;
        GameObject Sign = Instantiate(WarningSign, new Vector3(IsRight ? Right.x : Left.x, y, IsRight ? Right.z : Left.z) - (IsRight ? SignOffset : -SignOffset), Quaternion.identity);
        Destroy (Sign, 3);

        transform.position = Sign.transform.position  + (IsRight ? TransformOffset : -TransformOffset);
        TargetPos = new Vector3(IsRight ? Left.x : Right.x, y + Random.Range(-2.5f, 3.5f), IsRight ? Left.z : Right.z) + (IsRight ? MoveOffset : -MoveOffset);
    }

    public override void Update(){
        if(GameManager.instance.IsGameOver && !GameManager.instance.IsFightingAliens) return;
        if(!Go){
            timer+=Time.deltaTime;
            if(timer>=3) Go = true;

            return;
        }

        
        transform.position = Vector3.MoveTowards(transform.position, TargetPos, Speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, TargetPos) < 0.1f){
            Destroy(gameObject);
        }

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, TargetPos);
    }

    public override void OnHitPlayer() {
        AudioManager.Instance.PlayExpolsion();
        Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
        GameManager.instance.OnHitPlayer(gameObject, AmountOfFuelToDecrease);
    }
}