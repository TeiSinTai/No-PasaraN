using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowRotation : MonoBehaviour {

	public float rotationSpeed = 0.5f;

	// Use this for initialization
	void Awake () {
		StartCoroutine ("Rotation");
	}

	private IEnumerator Rotation(){
		while (true) {
			transform.Rotate (0, Mathf.LerpUnclamped(0.0f,1.0f,rotationSpeed/(Time.deltaTime+0.1f)), 0);
			yield return null;
		}
	}
}
