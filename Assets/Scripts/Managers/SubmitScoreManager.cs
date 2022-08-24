using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class SubmitScoreManager : MonoBehaviour {
    [SerializeField] private string LeaderBoardSceneName;
    [SerializeField] private string SubmitScoreURL;
    [SerializeField] private int MinUsernameLength;
    [SerializeField] private int MaxUsernameLength;
    [SerializeField] private FloatSo ScoreSo;
    [SerializeField] private TMP_InputField UsernameField;
    [SerializeField] private Button SubmitButton;
    [SerializeField] private TMP_Text ErrorText;
    [SerializeField] private TMP_Text ScoreText;

    private string UniqueID;
    private void Start() {
        ScoreText.text = $"Score:\n{ScoreSo.Value}";
        UsernameField.text = PlayerPrefs.GetString("Username");

        UniqueID = SystemInfo.deviceUniqueIdentifier;
    }

    public void OnInputFieldChanged() {
        if (UsernameField.text.Length > MinUsernameLength) {
            SubmitButton.interactable = true;
        } else {
            SubmitButton.interactable = false;
        }

        if (UsernameField.text.Length > MaxUsernameLength) {
            UsernameField.text = UsernameField.text.Substring(0, MaxUsernameLength);
        }

        UsernameField.text = Regex.Replace(UsernameField.text, @"[^a-zA-Z0-9 ]", "");
        UsernameField.text = Regex.Replace(UsernameField.text, @"[ ]{2,}", " ");
        UsernameField.text = UsernameField.text.Trim();

    }

    public void SubmitScore(){
        SubmitButton.interactable = false;
        ErrorText.text = "";
        string Username = UsernameField.text;
        
        if(Username.Length < MinUsernameLength){
            ErrorText.text = "Username is too short";
            SubmitButton.interactable = true;
            return;
        }else if(Username.Length > MaxUsernameLength){
            ErrorText.text = "Username is too long";
            SubmitButton.interactable = true;
            return;
        }

        PlayerPrefs.SetString("Username", Username);
        PlayerPrefs.Save();

        StartCoroutine(SubmitScoreToServer());
    }


    IEnumerator SubmitScoreToServer() {
        WWWForm form = new WWWForm();
        form.AddField("score", ScoreSo.Value.ToString());
        form.AddField("username", UsernameField.text);
        form.AddField("uniqueid", UniqueID);
        form.AddField("flag", PlayerPrefs.GetString("FlagCode", "jo"));
        UnityWebRequest www = UnityWebRequest.Post(SubmitScoreURL, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
            ErrorText.text = "Error submitting score";
        } else {
            string text = www.downloadHandler.text;
            Debug.Log(text);
            if(text == "success"){
                SceneManager.LoadScene(LeaderBoardSceneName);
            }else{
                ErrorText.text = "Error submitting score";
            }
                
        }
    }

}
