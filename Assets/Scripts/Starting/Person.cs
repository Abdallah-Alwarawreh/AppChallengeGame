using System;
using System.Collections;
using UnityEngine;

public class Person : MonoBehaviour{
    [SerializeField] private float JumpIntervalMax = 1f;
    [SerializeField] private float JumpIntervalMin = 0.5f;
    [SerializeField] private Color[] Colors;
    private float JumpInterval;
    private float JumpIntervalTimer;
    private bool IsJumping;
    Vector3 OriginalPlace;
    private void Start(){
        JumpInterval = UnityEngine.Random.Range(JumpIntervalMin, JumpIntervalMax);
        JumpIntervalTimer = JumpInterval;
        OriginalPlace = transform.position;
        GetComponent<Renderer>().material.color = Colors[UnityEngine.Random.Range(0, Colors.Length)];
    }
    
    private void Update(){
        if(!IsJumping) JumpIntervalTimer -= Time.deltaTime;
        if (JumpIntervalTimer <= 0){
            JumpIntervalTimer = JumpInterval;
            IsJumping = true;
            StartCoroutine(Jump());
        }
    }

    IEnumerator Jump(){
        
        // move random distance up randomly
        float jumpHeight = UnityEngine.Random.Range(0.5f, 1.0f);
        float jumpTime = 0.5f;
        
        while (jumpTime > 0){
            jumpTime -= Time.deltaTime;
            transform.Translate(Vector3.up * jumpHeight * Time.deltaTime);
            yield return null;
        }

        // move down to original place
        while (transform.position.y > OriginalPlace.y){
            transform.Translate(Vector3.down * Time.deltaTime);
            yield return null;
        }
        IsJumping = false;
        

    }
}