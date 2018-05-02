using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBuilder : MonoBehaviour {
	public GameObject buildingActivity; //Particle systems and other things.
	public float secondsToBuild = 1.5f;
	private WaitForSeconds timeToBuild;

	// Use this for initialization
	void Start () {
		timeToBuild = new WaitForSeconds (secondsToBuild);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void BuildTurret(){
		Debug.Log ("Build Turret");
		if (WaveManager.getCurrentPhase() != WaveManager.WaveManagerPhase.StopGame) {
			//Check if already built, try to upgrade?
			if (MagicStoneInterface.GetTurret () != null) {
				//Turret already exist. We need to check it for upgrade possibilities.
				if (2 > 3) { //FIXME check for upgradeability
					//	Upgrade
				} else {
					//FIXME sfx error building/upgrading turret. NO WAI
				}
			} else {
				if (FinanceManager.TryWithdrawal (1)) { //FIXME we need cost variable
					//gameObject.SetActive (false);
					if (buildingActivity != null) {
						buildingActivity.SetActive (true);
					}
					StartCoroutine ("BuildTurretCoroutine");	
				} else {
					//FIXME sfx turret build error no money
				}
			}
		}
	}

	private IEnumerator BuildTurretCoroutine(){
		yield return timeToBuild;
		MagicStoneInterface.CreateTurret ();
		if (buildingActivity != null) {
			buildingActivity.SetActive (false);
		}
		//gameObject.SetActive (true);
		gameObject.GetComponent<Animator>().ResetTrigger("Stop");
	}
}
