using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class MagicStoneInterface : MonoBehaviour {
	public GameObject teleportObject;
	public Image teleportImage;
	public GameObject turretPrefab;
	public GameObject turretCreator;
	private GameObject myTurret;
	private static MagicStoneInterface currentMagicStone = null;
	private static int turretCount = 0;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void CreateTurret(){
		MagicStoneInterface instance = currentMagicStone;
		instance.myTurret = Instantiate (instance.turretPrefab, instance.teleportObject.transform.position,new Quaternion(0,0,0,0));
		instance.myTurret.GetComponent<Turret> ().Init ();
		instance.myTurret.GetComponent<Turret> ().turretNumber = turretCount;
		turretCount++;
	}

	public static GameObject GetTurret(){
		MagicStoneInterface instance = currentMagicStone;
		if(instance.myTurret!=null){
			return instance.myTurret;
		}else{
			return null;
		}
	}


	public void MovePlayer(){
		if (currentMagicStone != null) {
			currentMagicStone.gameObject.SetActive(true);
		}
		currentMagicStone = this;
		Player.SetPosition (transform.position);
		currentMagicStone.gameObject.SetActive(false);
	}


}
