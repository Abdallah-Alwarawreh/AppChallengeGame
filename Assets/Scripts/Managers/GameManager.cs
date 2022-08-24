using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour{
    public static GameManager instance;
    public bool IsEndlessMode = false;
    public FloatSo ScoreSo;

    [Header("Score & Fuel")]
    [SerializeField] private Slider DestSlider;
    [SerializeField] private int MaxScore;
    [SerializeField] private int MaxHealth;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI PercentageText;
    [SerializeField, Range(0f, 0.15f)] private float ScoreTime = 0.1f;
    [SerializeField] private GameObject FuelBarUI;
    [SerializeField, Range(0f, 0.15f)] private float FuelTime = 0.1f;
    [SerializeField] private float FuelDecreaseAmount = 0.01f;
    [Header("End Game")]
    [SerializeField] private GameObject AlienWarningSign;
    [SerializeField] private GameObject Alien;
    [SerializeField] private int AmountOfAliensToSpawn;
    [SerializeField] private float DelayBetweenEachAlien;
    [SerializeField] private int AliensHealth = 10;
    [SerializeField] private float AliensShootSpeed = 2;
    [SerializeField] private float AliensSpeed = 3.5f;
    [Header("Game Over")]
    [SerializeField] private TextMeshProUGUI GameOverScoreText;
    [SerializeField] private TextMeshProUGUI WinScoreText;
    [HideInInspector] public int Score;
    [HideInInspector] public int Health = 3;
    [HideInInspector] public bool IsGameOver;
    [HideInInspector] public bool IsFightingAliens;
    [HideInInspector] public bool HasWon;
    [Header("Landing")]
    [SerializeField] private CanvasGroup BlackBox;
    [SerializeField] private GameObject LandingScene;
    [SerializeField] private GameObject Astronaut;
    [SerializeField] private CinemachineVirtualCamera AstroVCam;
    [SerializeField] private CinemachineVirtualCamera MainVCam;
    [SerializeField] private string Planet;
    [SerializeField] private int FuelAmount = 100;

    private bool HasShield = false;
    private float ScoreTimer;
    private float FuelTimer;
    private float Fuel = 0;
    private float MaxFuel = 0;
    private bool DecreaseFuel = true;
    private int AmountOfAliensDead = 0;
    private void Awake() => instance = this;

    /// <summary>
    ///     Initializes the game.
    /// </summary>
    public void Setup(){
        FuelBarUI.SetActive(true);
        MaxFuel = FuelAmount;
        Fuel = MaxFuel;
        FuelBar.instance.Setup(Fuel);

        DestSlider.maxValue = MaxScore;
        DestSlider.value = 0;

        Health = MaxHealth;
    }

    private void Update(){

        if (IsGameOver || IsFightingAliens || QuestionManager.instance.IsQuestionActive) return;

        ScoreTimer += Time.unscaledDeltaTime;
        FuelTimer += Time.unscaledDeltaTime;
        if (ScoreTimer >= ScoreTime){
            Score++;
            if(IsEndlessMode){
                ScoreText.SetText("Score: " + Score.ToString());
            }else{
                int PercentageScore =(int) (((float)Score / (float)MaxScore) * 100);
                PercentageText.SetText(PercentageScore + "%");
            }
            DestSlider.value = Score;
            ScoreTimer = 0;
        }

        if (FuelTimer >= FuelTime && DecreaseFuel){
            Fuel -= FuelDecreaseAmount;
            FuelBar.instance.SetFuel(Fuel);
            FuelTimer = 0;

            if (Fuel <= 0){
                GameOver(gameObject, true);
            }
        }

        if (Score >= MaxScore && !IsEndlessMode){
            StartCoroutine(ReachedDestination());
        }
    }


    /// <summary>
    ///     Called when the player reaches the destination.
    /// </summary>
    public IEnumerator ReachedDestination(){
        Time.timeScale = 1;
        IsFightingAliens = true;
        AlienWarningSign.SetActive(true);
        AudioManager.Instance.PlayWarning();
        yield return new WaitForSeconds(3f);
        AlienWarningSign.SetActive(false);
        FuelBar.instance.ConvertToHealth(Health);
        Movement.instance.TurnOnOffAutoFire();
        ChangeDecreaseFuel(false);
        for (int i = 0; i < AmountOfAliensToSpawn; i++){
            AlienShip alien = SpawnerManager.instance.SpawnObject(Alien).GetComponent<AlienShip>();
            alien.Setup(AliensHealth, AliensShootSpeed, AliensSpeed);
            yield return new WaitForSeconds(DelayBetweenEachAlien);
        }
    }

    /// <summary>
    ///     Called when the player gets hit.
    /// </summary>
    public void TakeDamage(){
        if(HasWon) return;
        Health--;
        FuelBar.instance.SetFuel(Health);

        if (Health <= 0){
            GameOver(gameObject, false);
        }
    }

    /// <summary>
    ///     Called when an alien dies.
    /// </summary>
    public void AlienDied(){
        AmountOfAliensDead++;
        if (AmountOfAliensDead >= AmountOfAliensToSpawn){
            // WON!!!!
            if(Planet != "") {
                PlayerPrefs.SetInt($"{Planet}Unlocked", 1);
                PlayerPrefs.Save();
            }
            Won();
        }
    }

    /// <summary>
    ///     Called when the player wins.
    /// </summary>
    private void Won(){
        HasWon = true;
        StartCoroutine(ShowLandingScene());
    }

    /// <summary>
    ///     Fades in a black box to transition to the landing scene.
    /// </summary>
    IEnumerator ShowLandingScene(){
        BlackBox.gameObject.SetActive(true);
        float timer = 0;
        while (timer < 2f){
            timer += Time.unscaledDeltaTime;
            BlackBox.alpha = timer;
            yield return null;
        }
        ScreenManager.instance.HideAllScreens();

        Time.timeScale = 0;
        LandingScene.SetActive(true);
        AstroVCam.gameObject.SetActive(true);
        MainVCam.gameObject.SetActive(false);
        AstroVCam.transform.position = Astronaut.transform.position;
        Camera.main.transform.position = AstroVCam.transform.position;
        Astronaut.GetComponent<Astronaut>().IsMoving = true;

        timer = 2f;
        while (timer > 0){
            timer -= Time.unscaledDeltaTime;
            BlackBox.alpha = timer;
            yield return null;
        }
        BlackBox.gameObject.SetActive(false);
    }

    /// <summary>
    ///     Called when the places the flag.
    /// </summary>
    public IEnumerator PlacedFlag(){
        yield return new WaitForSecondsRealtime(1f);
        AudioManager.Instance.PlayVoiceOver("MissionComplete");
        ScreenManager.instance.ShowWinScreen();
        StartCoroutine(WaitWin());
    }

    /// <summary>
    ///     Waits for the win screen to be shown, then sets the score.
    /// </summary>
    IEnumerator WaitWin(){
        yield return new WaitForSecondsRealtime(0.5f);
        StartCoroutine(SetTextToNumberSmoothly(WinScoreText, Score));
    }
    
    /// <summary>
    ///     Called When the player loses.
    /// </summary>
    public void GameOver(GameObject Caller, bool FromFuel = false){
        if(IsGameOver) return;
        IsGameOver = true;  
        Time.timeScale = 0;
        ScoreSo.Value = Score;
        ScreenManager.instance.ShowGameOverScreen();
        StartCoroutine(WaitGameover());
    }

    /// <summary>
    ///     Waits for the GameOver screen to be shown, then sets the score.
    /// </summary>
    IEnumerator WaitGameover(){
        yield return new WaitForSecondsRealtime(0.5f);
        AudioManager.Instance.PlayVoiceOver("MissionFailed");
        StartCoroutine(SetTextToNumberSmoothly(GameOverScoreText, Score));
    }
    
    /// <summary>
    ///     Sets the text to a number smoothly.
    /// </summary>
    IEnumerator SetTextToNumberSmoothly(TextMeshProUGUI text, int number){
        var start = text.text;
        var end = number.ToString();
        var timer = 0f;
        while(timer < 1){
            timer += Time.unscaledDeltaTime;
            text.SetText(((int)Mathf.Lerp(int.Parse(start), int.Parse(end), timer)).ToString());
            yield return null;
        }
    }

    /// <summary>
    /// Goes to scene with the same scene Name
    /// </summary>
    /// <param name="SceneName">the scene Name</param>
    public void GoToScene(string SceneName){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneName);
    }

    /// <summary>
    /// Replay the current scene.
    /// </summary>
    public void Replay(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Decrease the fuel by the amount.
    /// </summary>
    /// <param name="Caller">The GameObject that called the function</param>
    /// <param name="Amount">The amount of fuel to remove</param>
    public void OnHitPlayer(GameObject Caller, float Amount){
        // Spawn a particle effect
        if(Caller != null) Destroy(Caller);
        
        if (HasShield) {
            HasShield = false;
            return;
        }

        Fuel -= Amount;
    }

    /// <summary>
    ///     Stuns the player.
    /// </summary>
    /// <param name="StunTime"> The time the player is stunned.</param>
    public void StunPlayer(float StunTime) {
        Movement.StunnedTime = StunTime;
    }

    
    /// <summary>
    /// Add Fuel
    /// </summary>
    /// <param name="AddFuelAmount">Amount of fuel to add</param>
    public void AddFuel(float AddFuelAmount){
        Fuel += AddFuelAmount;
        
        if(Fuel > MaxFuel){
            Fuel = MaxFuel;
        }
    }
    
    /// <summary>
    ///     Changes the state of DecreaseFuel.
    /// </summary>
    public void ChangeDecreaseFuel(bool State){
        DecreaseFuel = State;
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
}
