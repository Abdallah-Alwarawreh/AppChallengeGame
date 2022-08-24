using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class LeaderBoardManager : MonoBehaviour {
	[SerializeField] private string MainMenuSceneName;
	[SerializeField] private GameObject PleaseWait;
	[SerializeField] private GameObject BackButton;
	[SerializeField] private GameObject WarningSign;
	[SerializeField] private string LeaderBoardLink;
	[SerializeField] private GameObject LeaderBoardFieldPrefab;
	[SerializeField] private Transform LeaderBoardFieldsHolder;
	[SerializeField] private Players players;
	[SerializeField] private Dictionary<string, Sprite> FlagSprites;

	void Start() {
		FlagSprites = new Dictionary<string, Sprite>();
		StartCoroutine(GetLeaderBoardData());
	}

	IEnumerator GetLeaderBoardData(){
		WWWForm form = new WWWForm();
        UnityWebRequest www = UnityWebRequest.Post(LeaderBoardLink, form);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError) {
			PleaseWait.SetActive(false);
            WarningSign.SetActive(true);

        } else {
			PleaseWait.SetActive(false);
            string text = www.downloadHandler.text;
            if(text != "error"){
				players = JsonUtility.FromJson<Players>("{\"players\":" + text + "}");
				BackButton.SetActive(true);
				for(int i = 0; i < players.players.Length; i++){
					GameObject LeaderBoardField = Instantiate(LeaderBoardFieldPrefab, LeaderBoardFieldsHolder);
					Sprite FlagSprite;
					if(!FlagSprites.ContainsKey(players.players[i].Flag)){
						Texture2D Flagtexture = (Texture2D)Resources.Load("Flags/" + players.players[i].Flag);
						FlagSprite = Sprite.Create(Flagtexture, new Rect(0, 0, Flagtexture.width, Flagtexture.height), new Vector2(0.5f, 0.5f));
						FlagSprites.Add(players.players[i].Flag, FlagSprite);
					}else{
						FlagSprite = FlagSprites[players.players[i].Flag];
					}
					
					LeaderBoardField.GetComponent<LeaderBoardField>().Setup(i + 1, players.players[i].Username, players.players[i].Score, FlagSprite);
		
				}

            }else{
				WarningSign.SetActive(true);
            }
                
        }
	}

	public void Back(){
		SceneManager.LoadScene(MainMenuSceneName);
	}
}

[System.Serializable]
public class Players{
	public Player[] players;
}

[System.Serializable]
public class Player{
	public string Username;
	public int Score;
	public string Flag;
}