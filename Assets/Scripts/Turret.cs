using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {
	//Turret game-cycle:
	// Patrol: until closest enemy within range, check closest every X seconds FIXME
	// Shoot: closest enemy until dead or out of range -> Retarget
	// Retarget: closest enemy, if in range -> Shoot, else -> Patrol- 

	private enum TurretPhase {Patrol, Shoot};
	public static int turretCount = 0;
	private TurretPhase currentTask;
	private GameObject closestCreeper;
	public float patrolSpeed = 1.0f;
	private float targetUpdate = 0.0f;
	public float retargetTime = 0.5f;
	public float turretRange = 2.0f;
	public float turretShootDelay = 0.1f;
	public int turretDamage = 1;
	private WaitForSeconds turretShotTimer;
	public int turretNumber;
	public GameObject cannon;


	// Use this for initialization
	void Start () {
		turretCount++;
		Init ();
	}

	public void Init(){
		currentTask = TurretPhase.Patrol;
		turretShotTimer = new WaitForSeconds (turretShootDelay);
		Patrol ();
	}

	
	// Update is called once per frame
	void Update () {
		targetUpdate += Time.deltaTime;
	}

	public GameObject FindClosestCreeper()
	{
		GameObject[] creepers;
		creepers = GameObject.FindGameObjectsWithTag("Creeper");
		GameObject closest = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject creeper in creepers)
		{
			Vector3 diff = creeper.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < distance)
			{
				closest = creeper;
				distance = curDistance;
			}
		}
		//Debug.Log ("Closest creeper: " + distance.ToString ());
		return closest;
	}

	private void Patrol(){
		cannon.transform.rotation.eulerAngles.Set(0.0f,90.0f,0.0f);
		if (currentTask == TurretPhase.Patrol) {
			StartCoroutine ("Patrolling");
		} else if (currentTask == TurretPhase.Shoot) {
			StopCoroutine ("Shooting");
			StopCoroutine ("TargetFollow");
			currentTask = TurretPhase.Patrol;
			StartCoroutine ("Patrolling");
		}
	}

	private void Shoot(){
		if (currentTask == TurretPhase.Patrol) {
			StopCoroutine ("Patrolling");
			currentTask = TurretPhase.Shoot;
			StartCoroutine ("Shooting");
			StartCoroutine ("TargetFollow");
		}

	}

	private IEnumerator Patrolling(){
		while (true) {
			if (targetUpdate > retargetTime) {
				closestCreeper = FindClosestCreeper ();
				targetUpdate = 0.0f;
				//Debug.Log ("Turret " + turretNumber.ToString () + " search for target!");
			}
			if (closestCreeper != null) {
				Vector3 diff = closestCreeper.transform.position - transform.position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < turretRange) {
					Shoot ();
				}
			}
			transform.Rotate (0, Mathf.LerpUnclamped(0.0f,1.0f,patrolSpeed/(Time.deltaTime+0.1f)), 0);
			yield return null;
		}
	}

	private IEnumerator Shooting(){
		Vector3 diff;
		float curDistance;
		while (true) {
			if (closestCreeper != null) { //check if it is still alive
				diff = closestCreeper.transform.position - transform.position;
				curDistance = diff.sqrMagnitude;
				if (curDistance < turretRange) { // check if is is still within range
					//FIXME v.0.4 shot sfx AUDIO
					//Drawing the shot
					cannon.GetComponent<Cannon> ().MakeShot (closestCreeper, 0.5f);
					if (closestCreeper.GetComponent<Creeper> ().TakeDamage (turretDamage, turretNumber)) { 
						//make it take damage and ask, if it dies =)
						// Retarget, because target confirmed dead
						closestCreeper = FindClosestCreeper ();
					}
				} else {
					//Retarget, because target escaped our Divine Wrath
					closestCreeper = FindClosestCreeper ();
					if (closestCreeper == null) {
						Patrol ();
					} else {
						diff = closestCreeper.transform.position - transform.position;
						curDistance = diff.sqrMagnitude;
						if (curDistance > turretRange) { //New target out of range. Patrolling
							Patrol ();
						}
					}
				}
			} else {
				//Retarget, because target died when we were not looking. Damn.
				closestCreeper = FindClosestCreeper ();
				if (closestCreeper == null) {
					Patrol ();
				} else {
					diff = closestCreeper.transform.position - transform.position;
					curDistance = diff.sqrMagnitude;
					if (curDistance > turretRange) { //New target out of range. Patrolling
						Patrol ();
					}
				}
			}
			yield return turretShotTimer;
		}
	}

	private IEnumerator TargetFollow(){
		//FIXME v0.2b плавный поворот с ускорениями, без рывков. 
		while (true) {
			if(closestCreeper!=null){
				Vector3 turretPointAt = new Vector3 (closestCreeper.transform.position.x, transform.position.y, closestCreeper.transform.position.z);
				transform.LookAt (turretPointAt);
				cannon.transform.LookAt (closestCreeper.transform.position);
			}
			yield return null;
		}
	}
}
