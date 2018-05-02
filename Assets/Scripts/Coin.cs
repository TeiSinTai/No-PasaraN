using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {
	public int coinValue = 1;
	public float timeToLive = 15.0f;
	private WaitForSeconds secondsToLive;
	private Vector3 playerPosition;
	private Vector3 coinPosition;
	public float timeToMove = 0.7f;
	private enum CoinStates {Perish, Gather};
	private CoinStates coinState = CoinStates.Perish;
	public float coinGatherDistanceSqr = 20.0f;

	// Use this for initialization
	void Start () {
		secondsToLive = new WaitForSeconds (timeToLive);
		if (!Gather ()) {
			StartCoroutine ("Perish");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Gather(){
		if (coinState == CoinStates.Perish) {
			playerPosition = GameObject.FindGameObjectWithTag ("MoneyBag").transform.position;
			Vector3 diff = playerPosition - transform.position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < coinGatherDistanceSqr) {
				StopCoroutine ("Perish");
				coinState = CoinStates.Gather;
				StartCoroutine ("GoToDaddy");
				return true;
			} else {
				return false;
			}
		} else {
			return true;
		}
	}

	public void FastGather(){
		StopAllCoroutines ();
		FinanceManager.AddCoin (coinValue);
		Destroy (gameObject);
	}

	public void FastPerish(){
		StopAllCoroutines ();
		Destroy (gameObject);
	}


	private IEnumerator Perish(){
		yield return secondsToLive;
		//FIXME add sfx coin perish
		Destroy (gameObject);
	}

	private Vector3 VectorLerp(Vector3 a, Vector3 b, float t){
		Vector3 res;
		res.x = Mathf.Lerp (a.x, b.x, t);
		res.y = Mathf.Lerp (a.y, b.y, t);
		res.z = Mathf.Lerp (a.z, b.z, t);
		return res;
	}

	private IEnumerator GoToDaddy(){
		float time = Time.time; 
		coinPosition = transform.position;
		while (Time.time - time < timeToMove) {
			transform.position=  VectorLerp (coinPosition, playerPosition, (Time.time - time) / timeToMove);
			yield return null;
		}
		FinanceManager.AddCoin (coinValue);
		Destroy (gameObject);
	}

}
