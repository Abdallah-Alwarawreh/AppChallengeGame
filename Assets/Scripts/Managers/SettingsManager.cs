using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;
public class SettingsManager : MonoBehaviour {
	[SerializeField] private AudioMixer Mixer;
	[SerializeField] private TMP_Dropdown QualityDropdown;
	[SerializeField] private Slider SFXSlider;
	[SerializeField] private Slider MusicSlider;
	[SerializeField] private Toggle EnableVoiceOversToggle;
	[SerializeField] private Image Flag;

	private void Start() {
		QualityDropdown.value = PlayerPrefs.GetInt("Quality", (int)Quality.High);
		
		SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1);
		MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
		EnableVoiceOversToggle.isOn = PlayerPrefs.GetInt("EnableVoiceOvers", 1) == 1;
        
		string FlagCode = PlayerPrefs.GetString("FlagCode", "jo");
		Texture2D flagtexture = (Texture2D)Resources.Load("Flags/" + FlagCode);
		Sprite flagSprite = Sprite.Create(flagtexture, new Rect(0, 0, flagtexture.width, flagtexture.height), new Vector2(0.5f, 0.5f));
		Flag.sprite = flagSprite;
	}

	public void SetQuality(){
		QualitySettings.SetQualityLevel(QualityDropdown.value);
		PlayerPrefs.SetInt("Quality", QualityDropdown.value);
		PlayerPrefs.Save();
	}

	public void SetSFXVolume(){
		Mixer.SetFloat("SFXVolume", Mathf.Log10(SFXSlider.value) * 20);
		PlayerPrefs.SetFloat("SFXVolume", SFXSlider.value);
		PlayerPrefs.Save();
	}

	public void SetMusicVolume(){
		Mixer.SetFloat("MusicVolume", Mathf.Log10(MusicSlider.value) * 20);
		PlayerPrefs.SetFloat("MusicVolume", MusicSlider.value);
		PlayerPrefs.Save();
	}

	public void SetVoiceOvers(){
		PlayerPrefs.SetInt("EnableVoiceOvers", EnableVoiceOversToggle.isOn ? 1 : 0);
		PlayerPrefs.Save();
	}

	public void GoToScene(string sceneName){
		SceneManager.LoadScene(sceneName);
	}
}

public enum Quality {
	Low = 0,
	Medium = 1,
	High = 2
}