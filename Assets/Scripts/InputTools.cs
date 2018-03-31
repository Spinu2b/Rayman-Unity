using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTools : MonoBehaviour {

	private float axisInputThreshold = 0.01f;

	public float GetAxisRaw(string axisName) {
		return Input.GetAxisRaw (axisName);
	}

	public bool InputTyped() {
		return Mathf.Abs (Input.GetAxisRaw ("Horizontal")) > axisInputThreshold || Mathf.Abs (Input.GetAxisRaw ("Vertical")) > axisInputThreshold; 
	}
}
