using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	public float timeToLive = 15.0f;
	private WaitForSeconds secondsToLive;
	private Transform playerPosition;
	private Transform coinPosition;
	public float timeToMove = 0.7f;

	// Use this for initialization
	void Start () {
		secondsToLive = new WaitForSeconds (timeToLive);
		StartCoroutine ("Perish");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Gather(){
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		StopCoroutine ("Perish");
		StartCoroutine ("GoToDaddy");
	}

	private IEnumerator Perish(){
		yield return secondsToLive;
		//FIXME add sfx coin perish
		Destroy (gameObject);
	}

	private IEnumerator GoToDaddy(){
		while (true) {
			Vector3 translation = new Vector3 (0.1f, 0.1f, 0.1f);
			transform.Translate (translation, playerPosition);
			yield return null;
		}
	}

}
