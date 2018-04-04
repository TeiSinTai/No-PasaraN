using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Creeper : MonoBehaviour {
	private NavMeshAgent navMesh;
	private int health = 3;
	public float targetDistance = 0.1f;
	private int[] turretDamage;
	public GameObject coinPrefab;
	public float coinPValue=0.5f;
	public int turretMaxCount = 50;

	// Use this for initialization
	void Start () {
	}

	private void LogTurretDamage(){
		string debugString = "";
		int totalDamage = 0;
		string[] turretDamageString = new string[turretDamage.Length];
		for(int i =0; i< turretDamage.Length;i++) {
			turretDamageString[i] =  turretDamage[i].ToString ();
			totalDamage += turretDamage [i];
		}
		debugString = string.Join (",", turretDamageString);
		//Debug.Log (totalDamage.ToString() + " := " + debugString);
	}

	public void Init(int _health, float _speed, float _accel, float _angSpeed, GameObject _coinPrefab, float _coinPValue, Transform _creeperDestination){
		health = _health;
		navMesh = GetComponent<NavMeshAgent> ();
		navMesh.speed = _speed;
		navMesh.acceleration = _accel;
		navMesh.angularSpeed = _angSpeed;
		navMesh.destination = _creeperDestination.position;
		coinPrefab = _coinPrefab;
		coinPValue = _coinPValue;
		turretDamage = new int[turretMaxCount];
	}

	// Update is called once per frame
	void Update () {
		float dist = navMesh.remainingDistance;
		if (dist != Mathf.Infinity && navMesh.pathStatus == NavMeshPathStatus.PathComplete & dist < targetDistance) {
			// Finish
			LogTurretDamage();
			WaveManager.CreeperDies ();
			Destroy(gameObject);
		}
	}

	public bool TakeDamage(int damageToTake, int turretNumber){
		if (turretDamage [turretNumber] == null) {
			turretDamage [turretNumber] = damageToTake;
		} else {
			turretDamage [turretNumber] += damageToTake;
		}
		if (health > 0) {
			health -= damageToTake;
		} else {
			//DO NOT KICK DEAD CREEPERS. 
			return false;
		}
		if (health <= 0) {
			//Die
			Destroy (gameObject);
			LogTurretDamage();
			if (Random.value > coinPValue) {
				Instantiate (coinPrefab, transform.position, transform.rotation);
			}
			WaveManager.CreeperDies ();
			return true;
		} else {
			//Still Alive
			return false;
		}
	}
}
