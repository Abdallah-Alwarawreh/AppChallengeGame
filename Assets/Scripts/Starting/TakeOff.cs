using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;
using TMPro;

public class TakeOff : MonoBehaviour {
    
    [SerializeField] private Transform ObjectToRotate;
    [SerializeField] private Transform Rocket;
    [SerializeField] private Rigidbody RocketThrustersRB;
    [SerializeField] private List<GameObject> ObjectsToActivateInGame = new List<GameObject>();
    [SerializeField] private List<Transform> Lines = new List<Transform>();
    [SerializeField] private List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] private Transform LineGroup;
    [SerializeField] private TMP_Text CountDownText;
    [SerializeField] private CinemachineVirtualCamera VCam;
    [SerializeField] private GameObject Plane;
    [SerializeField] private float NewFollowObjectOffset = 3;
    [SerializeField] private float NewTrackedObjectOffset = 5;
    [SerializeField] private GameObject[] Dusts;
    private CinemachineTransposer VCamTransposer;
    private CinemachineComposer VCamComposer;
    private float CurrentRotation;
    private bool DoneRotating;
    private bool DoneTransitioning;
    private float timer;

    private float OldFollowOffset;
    private float OldTrackedObjectOffset;
    void Start(){
        CurrentRotation = 0;
        VCamTransposer = VCam.GetCinemachineComponent<CinemachineTransposer>();
        VCamComposer = VCam.GetCinemachineComponent<CinemachineComposer>();
        OldFollowOffset = VCamTransposer.m_FollowOffset.y;
        OldTrackedObjectOffset = VCamComposer.m_TrackedObjectOffset.y;
        ObjectsToActivateInGame.ForEach(o => o.SetActive(false));

        ChangeCameraToFitLines();
        ScreenManager.instance.ShowTakeOffScreen();
        
    }

    
    public float horizontalFoV = 35.93811f;

    /// <summary>
    /// Changes the camera to fit the lines
    /// </summary>
    void ChangeCameraToFitLines(){
        float halfWidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);
        float halfHeight = halfWidth * Screen.height / Screen.width;
        float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;
        VCam.m_Lens.FieldOfView = verticalFoV;
    }

    bool HasPlayed1;
    bool HasPlayed2;
    bool HasPlayed3;
    bool HasPlayed4;
    bool HasPlayed5;
    public Vector3 trq;
    void Update() {
        ChangeCameraToFitLines();
        if (!DoneTransitioning){
            timer += Time.deltaTime;
            CountDownText.text = ((5 - Mathf.CeilToInt(timer)) + 1).ToString();
            
            switch ((5 - Mathf.CeilToInt(timer)) + 1) {
                case 5:
                    if (!HasPlayed5){
                        AudioManager.Instance.PlayVoiceOver("5");
                        HasPlayed5 = true;
                    }
                    break;
                case 4:
                    if (!HasPlayed4){
                        AudioManager.Instance.PlayVoiceOver("4");
                        HasPlayed4 = true;
                    }
                    break;
                case 3:
                    if (!HasPlayed3){
                        AudioManager.Instance.PlayVoiceOver("3");
                        HasPlayed3 = true;
                    }
                    break;
                case 2:
                    if (!HasPlayed2){
                        AudioManager.Instance.PlayVoiceOver("2");
                        HasPlayed2 = true;
                    }
                    break;
                case 1:
                    if (!HasPlayed1){
                        AudioManager.Instance.PlayVoiceOver("1");
                        HasPlayed1 = true;
                    }
                    break;                
            }

            if(!DoneRotating){
                CurrentRotation += Time.deltaTime * 360 / 5;
                ObjectToRotate.localRotation = Quaternion.Euler(0, CurrentRotation, 0);
                if(CurrentRotation >= 360){
                    DoneRotating = true;
                    timer = 0;
                    ObjectToRotate.localRotation = Quaternion.Euler(0, 0, 0);
                    ScreenManager.instance.HideAllScreens();
                    AudioManager.Instance.PlayTakeOff();
                }
            } else{
                if (timer >= 2.5f && !RocketThrustersRB.useGravity){
                    RocketThrustersRB.useGravity = true;
                    RocketThrustersRB.isKinematic = false;
                    RocketThrustersRB.AddForce(Vector3.right * 10, ForceMode.Impulse);
                    RocketThrustersRB.AddTorque(trq, ForceMode.Impulse);
                }

                if (timer < 3 && timer > 2.5f){
                    Vector3 TargetRotation = new Vector3(-90, -90, 0);
                    Rocket.localRotation = Quaternion.Slerp(Rocket.localRotation, Quaternion.Euler(TargetRotation), Time.deltaTime * 5);
                }

                if (timer >= 3){
                    DoneTransitioning = true;
                    FinishedTransition();
                } else{
                    LineGroup.position += Vector3.up * Time.deltaTime * 10;
                    Plane.transform.position += Vector3.down * Time.deltaTime * 25;
                    VCamComposer.m_TrackedObjectOffset.y = Mathf.Lerp(OldTrackedObjectOffset, NewTrackedObjectOffset, timer / 3);
                    VCamTransposer.m_FollowOffset.y = Mathf.Lerp(OldFollowOffset, NewFollowObjectOffset, timer / 3);
                    
                }
            }
        }
    }
    
    /// <summary>
    /// Called when the transition is finished
    /// </summary>
    private void FinishedTransition(){
        Destroy(RocketThrustersRB.gameObject, 5);
        Rocket.localRotation = Quaternion.Euler(-90, -90, 0);
        DoneTransitioning = true;
        Rocket.GetComponent<Movement>().enabled = true;
        Plane.SetActive(false);
        VCamComposer.m_TrackedObjectOffset.y = NewTrackedObjectOffset;
        VCamTransposer.m_FollowOffset.y = NewFollowObjectOffset;
        VCam.Follow = null;
        VCam.LookAt = null;
        // cinemachine change from composer to "do nothing" 
        
        

        float h = Screen.height;
        float w = Screen.width;
        
        Vector3 RealPoint = Camera.main.ScreenToWorldPoint(new Vector3(w, h, 0));
        Lines.ForEach(x => {
            x.transform.localScale = new Vector3(x.transform.localScale.x, RealPoint.y, x.transform.localScale.z);
            x.transform.position = new Vector3(x.transform.position.x, RealPoint.y, x.transform.position.z);
        });

        Movement player = Rocket.GetComponent<Movement>();
        player.MaxX = Lines[0].position.x;
        player.MinX = Lines[3].position.x;
        SpawnPoints.ForEach(x => x.transform.localPosition = new Vector3(x.transform.position.x, RealPoint.y / 2, x.transform.position.z));
        Vector3 TopWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(w, h, -16));
        Vector3 BottomWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -16));
        Vector3 MiddleWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(w / 2, h / 2, -16));
        player.MinY = TopWorldPosition.y;
        player.MaxY = BottomWorldPosition.y;
        
        Dusts[0].SetActive(false);
        Dusts[1].SetActive(false);
        StartCoroutine(MoveRocketToPointSmoothly(new Vector3(Rocket.position.x, MiddleWorldPosition.y, Rocket.position.z), 1));

    }

    /// <summary>
    /// Moves the rocket to the given point smoothly
    /// </summary>
    IEnumerator MoveRocketToPointSmoothly(Vector3 pos, float time){
        float elapsedTime = 0;
        Vector3 startPos = Rocket.position;
        while(elapsedTime < time){
            elapsedTime += Time.deltaTime;
            Rocket.position = Vector3.Lerp(startPos, pos, elapsedTime / time);
            yield return null;
        }
        Rocket.position = pos;
        ObjectsToActivateInGame.ForEach(o => o.SetActive(true));
        ScreenManager.instance.ShowGameScreen();
        GameManager.instance.Setup();
        Destroy(gameObject);
    }
}