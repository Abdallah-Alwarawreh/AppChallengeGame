using UnityEngine;


public class ScreenManager : MonoBehaviour{
    public static ScreenManager instance;
    
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private GameObject TakeOffScreen;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject GameOverScreenBG;
    [SerializeField] private GameObject QuestionScreen;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject WinScreenBG;
    
    private void Awake() => instance = this;

    /// <summary>
    ///     Hide all screens.
    /// </summary>
    public void HideAllScreens(){
        GameScreen.SetActive(false);
        TakeOffScreen.SetActive(false);
        GameOverScreen.SetActive(false);
        GameOverScreenBG.SetActive(false);
        QuestionScreen.SetActive(false);
        WinScreen.SetActive(false);
    }
    
    /// <summary>
    ///     Show the game screen.
    /// </summary>
    public void ShowGameScreen(){
        HideAllScreens();
        GameScreen.SetActive(true);
    }
    
    /// <summary>
    ///     Show the take off screen.
    /// </summary>
    public void ShowTakeOffScreen(){
        HideAllScreens();
        TakeOffScreen.SetActive(true);
    }
    
    /// <summary>
    ///     Show the game over screen.
    /// </summary>
    public void ShowGameOverScreen(){
        HideAllScreens();
        GameOverScreen.SetActive(true);
        GameOverScreenBG.SetActive(true);
    }
    
    /// <summary>
    ///     Show the question screen.
    /// </summary>
    public void ShowQuestionScreen(){
        HideAllScreens();
        QuestionScreen.SetActive(true);
    }
    
    /// <summary>
    ///     Show the win screen.
    /// </summary>
    public void ShowWinScreen(){
        HideAllScreens();
        WinScreenBG.SetActive(true);
        WinScreen.SetActive(true);
    }
}
