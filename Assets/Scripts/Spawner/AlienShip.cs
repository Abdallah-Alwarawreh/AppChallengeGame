using System.Collections;
using UnityEngine;


public class AlienShip : Spawnable{
    [SerializeField] private float Health = 3;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject WarningSign;
    [SerializeField] private float ShootSpeed;
    [SerializeField] private float PlayerYOffset;

    private Vector3 SpawnedPos;
    private float ShootTimer;
    private bool CanShoot;

    public void Setup(int health, float shootSpeed, float speed){
        Health = health;
        ShootSpeed = shootSpeed;
        Speed = speed;
    }

    public override void Start(){
        SpawnedPos = transform.position;
        CanShoot = true;
    }

    public override void Update(){
        transform.Translate(-transform.up * Speed * Time.deltaTime);
        ShootTimer += Time.deltaTime;
        if(ShootTimer >= ShootSpeed && CanShoot){
            ShootTimer = 0;
            Shoot();
        }
        
        if(Movement.instance.transform.position.y - transform.position.y > PlayerYOffset && CanShoot){
            GameObject Warning = Instantiate(WarningSign, new Vector3(transform.position.x, Movement.instance.transform.position.y, transform.position.z), Quaternion.identity);
            StartCoroutine(WarningSignCountDown(Warning));
        }
    }

    /// <summary>
    /// Warning sign countdown
    /// </summary>
    IEnumerator WarningSignCountDown(GameObject Warning){
        CanShoot = false;
        yield return new WaitForSeconds(3);
        
        Destroy(Warning);
        StartCoroutine(MoveToSpawnedPos(SpawnedPos));
    }
    
    /// <summary>
    /// Moves the ship to the SpawnedPos
    /// </summary>
    /// <param name="TargetPos">The position to move to</param>
    IEnumerator MoveToSpawnedPos(Vector3 TargetPos){
        while(Vector3.Distance(transform.position, TargetPos) > 1f){
            transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * 5);
            yield return null;
        }
        
        transform.position = SpawnerManager.instance.GetRandomSpawnPoint();
        SpawnedPos = transform.position;
        CanShoot = true;
    }

    /// <summary>
    /// Called when hitting the player.
    /// </summary>
    public override void OnHitPlayer(){
        
        GameManager.instance.TakeDamage();
    }

    /// <summary>
    /// Spawns a bullet at the ship's position
    /// </summary>
    public void Shoot(){
        GameObject bullet = Instantiate(Bullet, transform.position, Quaternion.identity);
        Destroy(bullet, 10);
    }

    /// <summary>
    /// Takes Damage from the player
    /// </summary>
    public void TakeDamage(){
        Health--;
        if(Health <= 0){
            AudioManager.Instance.PlayExpolsion();
            GameManager.instance.AlienDied();
            Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
            Destroy(gameObject);
        }
    }
}
