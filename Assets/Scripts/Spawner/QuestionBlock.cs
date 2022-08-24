using UnityEngine;


public class QuestionBlock : Spawnable{
    [SerializeField] private GameObject GFX;
    [SerializeField] private float RotationSpeed;

    public override void Update(){
        base.Update();
        GFX.transform.Rotate(0, RotationSpeed * Time.deltaTime, 0);
    }


    /// <summary>
    /// Called when hitting the player
    /// </summary>
    public override void OnHitPlayer(){
        QuestionManager.instance.ShowQuestion();
        Destroy(Instantiate(DestroyedEffect, transform.position, Quaternion.identity), 2f);
        Destroy(gameObject);
    }
}