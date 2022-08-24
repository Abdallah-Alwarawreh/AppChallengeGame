using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMenu : MonoBehaviour {
	[SerializeField] private ScrollRect SR;
	[SerializeField] private RectTransform canvas;
	[SerializeField] private float SwipeThreshold;
	[SerializeField] private Button TravelButton;
	[SerializeField] private PlanetsRenderers Planets;
	[SerializeField] private Locks PlanetLocks;
	[SerializeField] private Color LockedColor;
	[SerializeField] private List<Image> Dots = new List<Image>();
	[SerializeField] private Color DotNoSelectColor;
	[SerializeField] private List<string> PlanetSceneNames = new List<string>();
	private float[] ChildrenPos;
	
	bool JupiterUnlocked = false;
	bool SaturnUnlocked = false;
	bool UranusUnlocked = false;
	bool NeptuneUnlocked = false;
	private void Start() {
		UnlockPlanets();

		SR.horizontalNormalizedPosition = 0;

		ChildrenPos = new float[SR.content.childCount];
		for (int i = 0; i < SR.content.childCount; i++) {
			ChildrenPos[i] = SR.content.GetChild(i).localPosition.x;
			RectTransform t = SR.content.GetChild(i).GetComponent<RectTransform>();
			t.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, canvas.rect.width);
		}
	}

	private void ScrollTo(int index) {
		SR.horizontalNormalizedPosition = ((float)index) / (SR.content.childCount - 1);
	}

	int index = 0;
	bool SwipedRight = false;
	bool SwipedLeft = false;
	private void Update() {
		if(Input.touchCount > 0) {
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Moved) {
				if(Mathf.Abs(touch.deltaPosition.x) > Mathf.Abs(touch.deltaPosition.y)) {
					if(touch.deltaPosition.x > SwipeThreshold) {
						SwipedLeft = true;
						SwipedRight = false;
					} else if(touch.deltaPosition.x < -SwipeThreshold) {
						
						SwipedRight = true;
						SwipedLeft = false;
					}else{
						SwipedRight = false;
						SwipedLeft = false;
					}
				}
			}else if (touch.phase == TouchPhase.Ended) {
				OnFinishSwipe();
			}
		}
	}

	void OnFinishSwipe(){
		if(SwipedRight) {
			index = (index + 1) % ChildrenPos.Length;
			ScrollTo(index);
			Swiped();
		}else if(SwipedLeft) {
			index = (index - 1 + ChildrenPos.Length) % ChildrenPos.Length;
			ScrollTo(index);
			Swiped();
		}
		SwipedRight = false;
		SwipedLeft = false;
	}

	void Swiped(){
		if(index == 0) {
			TravelButton.interactable = true;
			HideAllPlanetsExpect(Planets.Mars);
		}else if(index == 1) {
			TravelButton.interactable = JupiterUnlocked;
			HideAllPlanetsExpect(Planets.Jupiter);
		}else if(index == 2) {
			TravelButton.interactable = SaturnUnlocked;
			HideAllPlanetsExpect(Planets.Saturn);
		}else if(index == 3) {
			TravelButton.interactable = UranusUnlocked;
			HideAllPlanetsExpect(Planets.Uranus);
		}else if(index == 4) {
			TravelButton.interactable = NeptuneUnlocked;
			HideAllPlanetsExpect(Planets.Neptune);
		}
		ChangeDot();
	}

	private void HideAllPlanetsExpect(MeshRenderer planet) {
		Planets.Mars.enabled = false;
		Planets.Jupiter.enabled = false;
		Planets.Saturn.enabled = false;
		Planets.SaturnRing.enabled = false;
		Planets.Uranus.enabled = false;
		Planets.UranusRing.enabled = false;
		Planets.Neptune.enabled = false;
		if (planet == Planets.Saturn){
			planet.enabled = true;
			Planets.SaturnRing.enabled = true;
		}else if (planet == Planets.Uranus){
			planet.enabled = true;
			Planets.UranusRing.enabled = true;
		}else{
			planet.enabled = true;
		}
	} 

	private void UnlockPlanets(){
		JupiterUnlocked = PlayerPrefs.GetInt("JupiterUnlocked", 0) == 1;
		SaturnUnlocked = PlayerPrefs.GetInt("SaturnUnlocked", 0) == 1;
		UranusUnlocked = PlayerPrefs.GetInt("UranusUnlocked", 0) == 1;
		NeptuneUnlocked = PlayerPrefs.GetInt("NeptuneUnlocked", 0) == 1;

		if(!JupiterUnlocked) {
			Material LockedMaterial = new Material(Planets.Jupiter.material);
			LockedMaterial.color = LockedColor;
			Planets.Jupiter.material = LockedMaterial;
			PlanetLocks.Jupiter.enabled = true;
		}

		if(!SaturnUnlocked) {
			Material LockedMaterial = new Material(Planets.Saturn.material);
			Material LockedMaterialRing = new Material(Planets.SaturnRing.material);
			LockedMaterial.color = LockedColor;
			LockedMaterialRing.color = LockedColor;

			Planets.Saturn.material = LockedMaterial;
			Planets.SaturnRing.material = LockedMaterialRing;
			PlanetLocks.Saturn.enabled = true;
		}

		if(!UranusUnlocked) {
			Material LockedMaterial = new Material(Planets.Uranus.material);
			Material LockedMaterialRing = new Material(Planets.UranusRing.material);
			LockedMaterial.color = LockedColor;
			LockedMaterialRing.color = LockedColor;

			Planets.Uranus.material = LockedMaterial;
			Planets.UranusRing.material = LockedMaterialRing;
			PlanetLocks.Uranus.enabled = true;
		}

		if(!NeptuneUnlocked) {
			Material LockedMaterial = new Material(Planets.Neptune.material);
			LockedMaterial.color = LockedColor;
			Planets.Neptune.material = LockedMaterial;
			PlanetLocks.Neptune.enabled = true;
		}
	}

	public void Travel(){
		SceneManager.LoadScene(PlanetSceneNames[index]);
	}

	public void RightArrow(){
		index = (index + 1) % ChildrenPos.Length;
		ScrollTo(index);
		Swiped();
	}

	public void LeftArrow(){
		index = (index - 1 + ChildrenPos.Length) % ChildrenPos.Length;
		ScrollTo(index);
		Swiped();
	}

	public void GoToScene(string SceneName){
		SceneManager.LoadScene(SceneName);
	}
	
	void ChangeDot(){
		foreach(Image dot in Dots) {
			dot.color = DotNoSelectColor;
		}

		Dots[index].color = Color.white;
	}
}


[System.Serializable]
public class PlanetsRenderers{
	public MeshRenderer Mars;
	public MeshRenderer Jupiter;
	public MeshRenderer Saturn;
	public MeshRenderer SaturnRing;
	public MeshRenderer Uranus;
	public MeshRenderer UranusRing;
	public MeshRenderer Neptune;
}

[System.Serializable]
public class Locks{
	public Image Jupiter;
	public Image Saturn;
	public Image Uranus;
	public Image Neptune;
}