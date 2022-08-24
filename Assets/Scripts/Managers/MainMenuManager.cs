using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
	public string SelectionMenuSceneName;
	public string EndlessModeSceneName;
	public string LeaderBoardSceneName;
	public string CreditsSceneName;
	public string SettingsSceneName;
	
	public void PlayGame(){
		SceneManager.LoadScene(SelectionMenuSceneName);
	}

	public void EndlessMode(){
		SceneManager.LoadScene(EndlessModeSceneName);
	}

	public void LeaderBoard(){
		SceneManager.LoadScene(LeaderBoardSceneName);
	}

	public void Credits(){
		SceneManager.LoadScene(CreditsSceneName);
	}

	public void Settings(){
		SceneManager.LoadScene(SettingsSceneName);
	}
	
	public void QuitGame(){
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}	
}