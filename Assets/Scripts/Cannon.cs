using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour {
	public LineRenderer projectileLineRenderer;
	private WaitForSeconds ShotTraceWaitSeconds;

	public void MakeShot(GameObject target,float ShotTraceWaitTime){
		projectileLineRenderer.SetPosition (0, transform.position);
		projectileLineRenderer.SetPosition (1, target.transform.position);
		projectileLineRenderer.enabled = true;
		ShotTraceWaitSeconds = new WaitForSeconds (ShotTraceWaitTime);
		StartCoroutine ("ClearShotTrace");

	}

	// Use this for initialization
	void Start () {
		projectileLineRenderer.positionCount = 2;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator ClearShotTrace(){
		yield return ShotTraceWaitSeconds;
		projectileLineRenderer.enabled = false;
	}

}
