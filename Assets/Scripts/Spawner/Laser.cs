using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Spawnable {
	[SerializeField] private float AmountOfFuelToDecrease = 10f;
	[SerializeField] private LineRenderer lr;
    [SerializeField] private GameObject WarningSign;
    [SerializeField] private Vector3 Offset;
    private BoxCollider col;
    private float Height = 0;
    private float timer = 0;
    private bool Shoot = false;
    private bool HasHit = false;
    public override void Start(){
		lr.SetPosition(0, Vector3.zero);
		lr.SetPosition(1, Vector3.zero);

        Vector3 top = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 16)) - Offset;
        Destroy (Instantiate(WarningSign, new Vector3(transform.position.x, top.y, top.z) , Quaternion.identity), 3);
    }
	public override void Update(){
        if(GameManager.instance.IsGameOver && !GameManager.instance.IsFightingAliens) return;
        if(!Shoot){
            timer+=Time.deltaTime;
            if(timer>=3) {
                Shoot = true;
                AudioManager.Instance.PlayLaser();
            }

            return;
        }
        lr.SetPosition(1, Vector3.MoveTowards(lr.GetPosition(1), new Vector3(lr.GetPosition(1).x,-40f, lr.GetPosition(1).z), Speed * Time.deltaTime));
        Height = -lr.GetPosition(1).y;
        
        if(40 - Height < 0.1f){
            Destroy(gameObject, 1);
        }else{
            if(HasHit) return;
            RaycastHit[] hits;
            hits = Physics.BoxCastAll(transform.position + new Vector3(0, -Height / 2, 0), new Vector3(lr.startWidth/2, -Height/2, lr.startWidth/2), Vector3.down, Quaternion.identity, Height);
            foreach(RaycastHit h in hits){
                if(h.collider.gameObject.tag == "Player"){
                    HasHit = true;
                    GameManager.instance.OnHitPlayer(null, AmountOfFuelToDecrease);
                }
                // else{
                //     if(h.collider.gameObject == gameObject) continue;
                //     Destroy(h.collider.gameObject);
                // }
            }
        }

        if(Height > 30 && col == null){
            col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(lr.startWidth, 40, lr.startWidth);
            col.center = new Vector3(0, -40 / 2, 0);
        }
    }

	private void OnDrawGizmos() {
        Gizmos.color = Color.red;
    
        Gizmos.DrawWireCube(transform.position + new Vector3(0, -Height / 2, 0), new Vector3(lr.startWidth, -Height, lr.startWidth));
	}
    public override void OnTriggerEnter(Collider col){ 
        if (col.CompareTag("Player")){
            if(HasHit)return;
            HasHit = true;
            GameManager.instance.OnHitPlayer(null, AmountOfFuelToDecrease);
        }
    }
}