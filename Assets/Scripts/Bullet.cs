using UnityEngine;

public class Bullet : MonoBehaviour{
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool isEnemyBullet = false;
    private void Update(){
        if(isEnemyBullet) transform.Translate(Vector3.down * speed * Time.deltaTime);
        else transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other){
        if (other.CompareTag("Player") && isEnemyBullet){
            
            GameManager.instance.TakeDamage();
            Destroy(gameObject);
        }else if (other.CompareTag("Enemy") && !isEnemyBullet){
            other.GetComponent<AlienShip>().TakeDamage();
            Destroy(gameObject);
        }
    }
}