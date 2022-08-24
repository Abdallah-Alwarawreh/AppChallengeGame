using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Astronaut : MonoBehaviour{
    [SerializeField] private CharacterController Controller;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform Flag;
    [SerializeField] private float Speed;
    [SerializeField] private Transform PointToGoTo;
    [SerializeField] private float RotationSpeed;
    [SerializeField] private float Gravity = 9.81f;
    [SerializeField] private LayerMask GroundLayer;
    [SerializeField, Range(0f, 1f)] private float PlaceOffset = 0.5f;
    private bool DoneMoving = false;
    public bool IsMoving = false;
    private void Start(){
        anim.SetInteger("State", (int)AnimStates.Walking);
    }

    private void Update(){
        Controller.Move(Vector3.down * Gravity * Time.unscaledDeltaTime);
        if(DoneMoving || !IsMoving) return;
        Vector3 Pos2 = new Vector3(PointToGoTo.position.x, transform.position.y, PointToGoTo.position.z);
        if(Vector3.Distance(transform.position, Pos2) > 0.25f){
            Controller.Move(transform.forward * Speed * Time.unscaledDeltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(PointToGoTo.position - transform.position), RotationSpeed * Time.unscaledDeltaTime);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        } else{
            DoneMoving = true;
            ReachedPoint();
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -Vector3.up, out hit, 5f, GroundLayer)){
            Gizmos.DrawLine(transform.position, hit.point);
            Gizmos.DrawWireSphere(hit.point, 0.3f);
        }
    }
    #endif

    /// <summary>
    /// Called when the astronaut reaches the point to go to.
    /// </summary>
    private void ReachedPoint(){
        anim.SetInteger("State", (int)AnimStates.PuttingFlag);
    }

    /// <summary>
    /// Called when the astronaut is done putting the flag.
    /// </summary>
    public void PlacedFlag(){
        AudioManager.Instance.PlayPlaceFlag();
        anim.SetInteger("State", (int)AnimStates.Idle);
        RaycastHit hit;
        if(Physics.Raycast(Flag.position, -Vector3.up, out hit, 5f, GroundLayer)){
            Flag.SetParent(transform.parent.parent);
            Flag.position = hit.point + new Vector3(0,0,PlaceOffset);
            Flag.rotation = Quaternion.Euler(0,0,0);
        }

        StartCoroutine(GameManager.instance.PlacedFlag());
    }
    

    public void FootStep(){
        if (IsMoving) AudioManager.Instance.PlayFootStep();
    }
}

enum AnimStates{
    Idle=0,
    Walking=1,
    PuttingFlag=2,
}