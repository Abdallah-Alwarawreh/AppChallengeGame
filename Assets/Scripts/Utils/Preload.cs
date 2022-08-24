using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preload : MonoBehaviour {
	[SerializeField] private string MainMenuSceneName;
	[SerializeField] private string FlagChooseSceneName;
	private void Awake() {
		SetupQuality();

		bool HasChosenFlag = PlayerPrefs.GetInt("HasChosenFlag", 0) == 1;
		if (HasChosenFlag) {
			SceneManager.LoadScene(MainMenuSceneName);
		} else {
			SceneManager.LoadScene(FlagChooseSceneName);
		}
	}

	void SetupQuality(){
		int QualityLevel = PlayerPrefs.GetInt("Quality", (int)Quality.High);
		QualitySettings.SetQualityLevel(QualityLevel);
	}
}