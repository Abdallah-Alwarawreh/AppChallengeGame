using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FloatSo : ScriptableObject {
	[SerializeField] private float value;
	public float Value {
		get { return value; }
		set { this.value = value; }
	}
	
}