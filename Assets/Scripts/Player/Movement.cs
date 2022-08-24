using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour{
    public static Movement instance;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MoveThreshold = 0.5f;
    [SerializeField] private float RotateAmount = 45f;
    [SerializeField] public float MaxX;
    [SerializeField] public float MinX;
    [SerializeField] public float MaxY;
    [SerializeField] public float MinY;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private float ShootSpeed;
    public static float StunnedTime = 0;
    private float ShootTimer;
    private bool AutoFire;
    private void Awake() => instance = this;

    private Vector3 BaseRotation;

    private void Start() {
        BaseRotation = transform.rotation.eulerAngles;
    }

    private void Update(){
        if (GameManager.instance.HasWon){
            // keep moving up
            transform.position += Vector3.up * MovementSpeed * Time.deltaTime;
            return;
        }

        if (AutoFire && !GameManager.instance.HasWon){
            ShootTimer += Time.deltaTime;
            if (ShootTimer >= ShootSpeed){
                ShootTimer = 0f;
                Shoot();
            }
        }
        if(StunnedTime > 0){
            StunnedTime -= Time.deltaTime;
            return;
        }

        if (GameManager.instance.IsGameOver && !GameManager.instance.IsFightingAliens || QuestionManager.instance.IsQuestionActive) return;
        Vector3 TargetPosition = Vector3.zero;
        if (Input.touchCount > 0){
            TargetPosition = Input.GetTouch(0).position;
            
        } else if (Input.GetMouseButton(0)){
            TargetPosition = Input.mousePosition;
        }

        if (TargetPosition != Vector3.zero){
            TargetPosition.z = transform.position.z;
            TargetPosition.x = map(TargetPosition.x, MinX, MaxX, 0, Screen.width);
            TargetPosition.y = map(TargetPosition.y, MinY, MaxY, 0, Screen.height);
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, MovementSpeed * Time.unscaledDeltaTime);
        }

        if (Input.touchCount > 0){
            if (transform.position.x < TargetPosition.x - MoveThreshold){
                // Rotate Right
                Quaternion TargetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, BaseRotation.y + RotateAmount, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, MovementSpeed * Time.unscaledDeltaTime);
            } else if (transform.position.x > TargetPosition.x + MoveThreshold){
                // Rotate Left
                Quaternion TargetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, BaseRotation.y - RotateAmount, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, MovementSpeed * Time.unscaledDeltaTime);
            }else{
                // Reset
                Quaternion TargetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, BaseRotation.y, transform.rotation.eulerAngles.z);
                transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, MovementSpeed * Time.unscaledDeltaTime);
            }
        }else{
            // Reset
            Quaternion TargetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, BaseRotation.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, TargetRot, MovementSpeed * Time.unscaledDeltaTime);
        }
    }

    /// <summary>
    /// Map a value from one range to another
    /// </summary>
    /// <param name="Value">The value to map</param>
    /// <param name="NewMin">The new minimum value</param>
    /// <param name="NewMax">The new maximum value</param>
    /// <param name="OldMin">The old minimum value</param>
    /// <param name="OldMax">The old maximum value</param>
    /// <returns>The mapped value</returns>
    private float map(float Value, float NewMin, float NewMax, float OldMin, float OldMax){
        return ((Value - OldMin) / (OldMax - OldMin)) * (NewMax - NewMin) + NewMin;
    }

    /// <summary>
    /// Shoot a bullet and then destroy it
    /// </summary>
    public void Shoot(){
        AudioManager.Instance.PlayShoot();
        Destroy(Instantiate(Bullet, transform.position, Quaternion.identity), 5);
    }

    /// <summary> 
    /// Set the auto fire to true or false
    /// </summary>
    public void TurnOnOffAutoFire() => AutoFire = !AutoFire;
    
}
