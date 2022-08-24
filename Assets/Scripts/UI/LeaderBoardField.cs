using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderBoardField : MonoBehaviour {
	[SerializeField] private TMP_Text Rank;
	[SerializeField] private TMP_Text Name;
	[SerializeField] private TMP_Text Score;
	[SerializeField] private Image Flag;

	[SerializeField] private Color Gold;
	[SerializeField] private Color Silver;
	[SerializeField] private Color Broze;
	public void Setup(int rank, string name, int score, Sprite FlagSprite){
		Rank.text = rank.ToString();
		Name.text = name;
		Score.text = score.ToString();
		
		Flag.sprite = FlagSprite;

		if(rank == 1){
			Rank.color = Gold;
		}else if(rank == 2){
			Rank.color = Silver;
		}else if(rank == 3){
			Rank.color = Broze;
		}
		
	}
}