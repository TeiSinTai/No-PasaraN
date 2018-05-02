using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private static Player instance;
	public Transform moneyBag;
	public float coinGatherDistanceSqr = 20.0f; 
	private void Start(){
		instance = this;
		FindClosestTeleport ().GetComponent<MagicStoneInterface> ().MovePlayer ();

	}

	public static Vector3 GetPosition(){
		return instance.transform.position;
	}

	public static void SetPosition(Vector3 position){
		//instance.transform.position = new Vector3 (position.x,instance.transform.position.y,position.z);
		instance.transform.position = position;
		GameObject[] coins;
		coins = GameObject.FindGameObjectsWithTag("Coin");
		foreach (GameObject coin in coins)
		{
			coin.GetComponent<Coin> ().Gather ();
		}
		//Debug.Log ("Closest creeper
	}

	public GameObject FindClosestTeleport()
	{
		GameObject[] stones;
		stones = GameObject.FindGameObjectsWithTag("TeleportPoint");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject stone in stones)
		{
			Vector3 diff = stone.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = stone;
				distance = curDistance;
			}
		}
		//Debug.Log ("Closest stone: " + distance.ToString ());
		return closest;
	}

}
