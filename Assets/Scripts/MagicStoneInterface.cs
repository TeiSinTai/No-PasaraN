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



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateTurret(){
		myTurret = Instantiate (turretPrefab, teleportObject.transform.position, teleportObject.transform.rotation);
		myTurret.GetComponent<Turret> ().Init ();
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
