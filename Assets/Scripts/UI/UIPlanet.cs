using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlanet : MonoBehaviour {

	[SerializeField] private RectTransform planet;
	[SerializeField] private float RotationSpeed;
	
	void Update() {
		// rotate the planet on the y axis
		planet.Rotate(0, RotationSpeed * Time.deltaTime, 0);
	}
}