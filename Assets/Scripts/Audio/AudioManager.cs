using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour {
	public static AudioManager Instance;
	[SerializeField] private AudioMixer Mixer;
	[SerializeField] private AudioMixerGroup SFX;
	[SerializeField] private AudioMixerGroup Music; 
	[SerializeField] private ExpolsionsAudio Expolsions;
	[SerializeField] private ClickAudio Clicks;
	[SerializeField] private LaserAudio Lasers;
	[SerializeField] private QuestionBlockAudio QuestionBlocks;
	[SerializeField] private SelectAudio Selects;
	[SerializeField] private ShootAudio Shoots;
	[SerializeField] private PlateImpactAudio PlateImpacts;
	[SerializeField] private FootStepAudio FootSteps;
	[SerializeField] private PlaceFlagAudio PlaceFlags;
	[SerializeField] private VoiceOversAudio VoiceOvers;
	[SerializeField] private AudioClip TakeOffAudio;
	[SerializeField] private AudioClip ZapAudio;
	[SerializeField] private AudioClip FuelPickupAudio;
	[SerializeField] private AudioClip WarningAudio;
	[SerializeField, Range(0, 1)] private float ShootVolume = 1;

	bool IsVoiceOversOn = true;
	private void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad(gameObject);

			Mixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume", 1)) * 20);
			Mixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1)) * 20);
			IsVoiceOversOn = PlayerPrefs.GetInt("EnableVoiceOvers", 1) == 1;
		} else {
			Destroy(gameObject);
		}
	}

	public void PlaySound(AudioClip clip) {
		GameObject go = new GameObject("Audio: " + clip.name);
		DontDestroyOnLoad(go);
		go.transform.parent = Camera.main.transform;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.outputAudioMixerGroup = SFX;
		source.volume = 1;
		source.Play();

		Destroy(go, clip.length);

	}

	public void PlayExpolsion() {
		int index = Random.Range(0, Expolsions.clips.Count);
		PlaySound(Expolsions.clips[index]);
	}

	public void PlayClick() {
		int index = Random.Range(0, Clicks.clips.Count);
		PlaySound(Clicks.clips[index]);
	}

	public void PlayLaser() {
		int index = Random.Range(0, Lasers.clips.Count);
		PlaySound(Lasers.clips[index]);
	}

	public void PlayQuestionBlock() {
		int index = Random.Range(0, QuestionBlocks.clips.Count);
		PlaySound(QuestionBlocks.clips[index]);
	}

	public void PlaySelect() {
		int index = Random.Range(0, Selects.clips.Count);
		PlaySound(Selects.clips[index]);
	}

	public void PlayShoot() {
		int index = Random.Range(0, Shoots.clips.Count);
		PlaySound(Shoots.clips[index]);
	}

	public void PlayPlateImpact() {
		int index = Random.Range(0, PlateImpacts.clips.Count);
		PlaySound(PlateImpacts.clips[index]);
	}

	public void PlayFootStep() {
		int index = Random.Range(0, FootSteps.clips.Count);
		PlaySound(FootSteps.clips[index]);
	}

	public void PlayPlaceFlag() {
		int index = Random.Range(0, PlaceFlags.clips.Count);
		PlaySound(PlaceFlags.clips[index]);
	}

	public void PlayVoiceOver(string Name) {
		if(!IsVoiceOversOn) return;
		VoiceOver voiceOver = VoiceOvers.VoiceOvers.FirstOrDefault(x => x.name == Name);
		if (voiceOver != null) {
			PlaySound(voiceOver.clip);
		}
	}

	public void PlayTakeOff() {
		PlaySound(TakeOffAudio);
	}

	public void PlayZap() {
		PlaySound(ZapAudio);
	}

	public void PlayFuelPickup() {
		PlaySound(FuelPickupAudio);
	}

	public void PlayWarning() {
		PlaySound(WarningAudio);
	}
}

[System.Serializable]
public class AudioClipInfo {
	public string name;
	public AudioClip clip;
}

[System.Serializable]
public class ExpolsionsAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class ClickAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}


[System.Serializable]
public class LaserAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class QuestionBlockAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class SelectAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class ShootAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class PlateImpactAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class FootStepAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class PlaceFlagAudio {
	public List<AudioClip> clips = new List<AudioClip>();
}

[System.Serializable]
public class VoiceOver {
	public string name;
	public AudioClip clip;
}

[System.Serializable]
public class VoiceOversAudio {
	public List<VoiceOver> VoiceOvers = new List<VoiceOver>();
}