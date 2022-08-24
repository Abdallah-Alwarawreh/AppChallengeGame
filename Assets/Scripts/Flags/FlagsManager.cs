using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class FlagsManager : MonoBehaviour {
	
	[SerializeField] private string MainMenuSceneName = "Main Menu";
	[SerializeField] private GameObject FlagPrefab; 
	[SerializeField] private Transform FlagsHolder;
	[SerializeField] private List<Button> Buttons = new List<Button>();
	public void SetFlag(string flagCode){
		PlayerPrefs.SetString("FlagCode", flagCode);
		PlayerPrefs.SetInt("HasChosenFlag", 1);
		PlayerPrefs.Save();
		SceneManager.LoadScene(MainMenuSceneName);
	}

	private void Start() {
		foreach(Button btn in Buttons){
			btn.onClick.AddListener(() => {
				AudioManager.Instance.PlayClick();
				SetFlag(btn.gameObject.name);
			});
		}
	}
	
	#if UNITY_EDITOR
    public void SpawnFlags(){
		Buttons.Clear();
        TextAsset flags = (TextAsset)Resources.Load("flags");
        string[] lines = flags.text.Split('\n');
        foreach(string line in lines){
            string[] data = line.Split('|');
            string flagCode = data[1].Replace(".png", "");
			if(flagCode != "xk") flagCode = flagCode.Substring(0, flagCode.Length - 1);
			
			Texture2D flagtexture = (Texture2D)Resources.Load("Flags/" + flagCode);
			Sprite flagSprite = Sprite.Create(flagtexture, new Rect(0, 0, flagtexture.width, flagtexture.height), new Vector2(0.5f, 0.5f));
			GameObject flag = Instantiate(FlagPrefab, FlagsHolder);
			flag.name = flagCode;
			Image flagImage = flag.transform.Find("Flag").GetComponent<Image>();
			flagImage.sprite = flagSprite;

			Button btn = flag.GetComponent<Button>();
			Buttons.Add(btn);
        }
    }
	#endif


	
}

#if UNITY_EDITOR
[CustomEditor(typeof(FlagsManager))]
public class FlagsManagerEditor : Editor {
	public override void OnInspectorGUI(){
		base.OnInspectorGUI();
		FlagsManager manager = (FlagsManager)target;
		if(GUILayout.Button("SpawnFlags")){
			manager.SpawnFlags();
		}
	}
}
#endif
