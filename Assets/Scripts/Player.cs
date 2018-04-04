using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	private static Player instance;

	private void Start(){
		instance = this;
	}

	public static Vector3 GetPosition(){
		return instance.transform.position;
	}

	public static void SetPosition(Vector3 position){
		//instance.transform.position = new Vector3 (position.x,instance.transform.position.y,position.z);
		instance.transform.position = position;
		}
}
