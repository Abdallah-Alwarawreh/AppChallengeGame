using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuestionManager : MonoBehaviour{
    public static QuestionManager instance;
    [SerializeField] private Question[] Questions;
    [SerializeField] private Reward[] Rewards;
    [SerializeField] private TMP_Text QuestionText;
    [SerializeField] private TMP_Text RewardText;
    [SerializeField] private Button[] AnswerButtons;
    [SerializeField] private TMP_Text[] AnswerText;
    [SerializeField] private Color CorrectColor;
    [SerializeField] private Color WrongColor;
    [SerializeField] private Button CancelButton;
    private Question CurrentQuestion;
    private Reward CurrentReward;
    private float OldTimeScale;
    [HideInInspector] public bool IsQuestionActive;
    private void Awake() => instance = this;
    
    /// <summary>
    ///     Gets a random Question.
    /// </summary>
    public Question GetRandomQuestion(){
        int index = UnityEngine.Random.Range(0, Questions.Length);
        return Questions[index];
    }
    
    /// <summary>
    ///     Gets a random reward.
    /// </summary>
    public Reward GetRandomReward(){
        int index = UnityEngine.Random.Range(0, Rewards.Length);
        return Rewards[index];
    }
    
    /// <summary>
    ///     Checks the answer
    /// </summary>
    /// <param name="answer">The answer</param>
    public void CheckAnswer(int answer){
        foreach (var btn in AnswerButtons){
            btn.interactable = false;
        }
        
        if(answer == CurrentQuestion.CorrectAnswer){
            AnswerButtons[answer].image.color = CorrectColor;

            switch (CurrentReward.RewardType){
                case RewardTypes.Money:
                    // TODO: Add money
                    break;
                case RewardTypes.Fuel:
                    GameManager.instance.AddFuel(CurrentReward.RewardValue);
                    AudioManager.Instance.PlayVoiceOver("Correct");
                    break;
            }
        } else{
            AnswerButtons[answer].image.color = WrongColor;
            AnswerButtons[CurrentQuestion.CorrectAnswer].image.color = CorrectColor;
            AudioManager.Instance.PlayVoiceOver("Wrong");
        }
        CancelButton.interactable = false;
        StartCoroutine(WaitToReset());
    }

    /// <summary>
    ///     Resets back to the game scene.
    /// </summary>
    IEnumerator WaitToReset(){
        yield return new WaitForSecondsRealtime(2);
        
        Time.timeScale = OldTimeScale;
        ScreenManager.instance.ShowGameScreen();
        SpawnerManager.instance.StopSpawning = false;
        GameManager.instance.ChangeDecreaseFuel(true);
        IsQuestionActive = false;
    }
    
    /// <summary>
    ///     Shows a random question with a random reward and shuffled answers.
    /// </summary>
    public void ShowQuestion(){
        if(IsQuestionActive || GameManager.instance.IsFightingAliens) return;
        ResetUI();
        AudioManager.Instance.PlayQuestionBlock();
        IsQuestionActive = true;
        CurrentQuestion = GetRandomQuestion();
        CurrentReward = GetRandomReward();
        QuestionText.text = CurrentQuestion.QuestionText;
        RewardText.text = "Reward: " + CurrentReward.RewardText;
        
        // Shuffle answers
        string CorrectAnswer = CurrentQuestion.Answers[CurrentQuestion.CorrectAnswer];
        for (int i = 0; i < CurrentQuestion.Answers.Length; i++){
            int randomIndex = UnityEngine.Random.Range(0, CurrentQuestion.Answers.Length);
            (CurrentQuestion.Answers[i], CurrentQuestion.Answers[randomIndex]) = (CurrentQuestion.Answers[randomIndex], CurrentQuestion.Answers[i]);
        }
        CurrentQuestion.CorrectAnswer = CurrentQuestion.Answers.ToList().IndexOf(CorrectAnswer);
        
        for (int i = 0; i < AnswerButtons.Length; i++){
            AnswerText[i].text = CurrentQuestion.Answers[i];
            AnswerButtons[i].interactable = true;
        }
        CancelButton.interactable = true;
        
        OldTimeScale = Time.timeScale;
        Time.timeScale = 0;
        ScreenManager.instance.ShowQuestionScreen();

    }
    
    /// <summary>
    ///     Called when the player clicks on the X.
    /// </summary>
    public void CancelQuestion(){
        
        Time.timeScale = OldTimeScale;
        ScreenManager.instance.ShowGameScreen();
        SpawnerManager.instance.StopSpawning = false;
        GameManager.instance.ChangeDecreaseFuel(true);
        IsQuestionActive = false;
    }

    public void ResetUI(){
        foreach (var btn in AnswerButtons){
            btn.image.color = Color.white;
            btn.interactable = true;
        }
        CancelButton.interactable = false;
    }
}

[Serializable]
public class Question{
    public string QuestionText;
    public string[] Answers;
    public int CorrectAnswer;
}

[Serializable]
public class Reward{
    public string RewardText;
    public float RewardValue;
    public RewardTypes RewardType;
    
}

public enum RewardTypes{
    Money,
    Fuel
}