using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager: MonoBehaviour{
	public Transform creeperDestination;
	public Wave[] Waves;
	private WaitForSeconds timeBeforeWave;
	private WaitForSeconds timeBetweenCreepers;
	private static WaveManager instance;
	private int waveNumber;
	private int currentCreeperCount=0;
	private enum WaveManagerPhase {Rest, WaveQueue, SpawnCreepers};
	private WaveManagerPhase currentPhase;

	void Start(){
		instance = this;
		currentPhase = WaveManagerPhase.Rest;
		StartWaves (); //FIXME game start at user input, not on load

		// DEBUG!!!! Create ALL turrets in script. FIXME v0.2
		GameObject[] turrets;
		GameObject[] stones;
		stones = GameObject.FindGameObjectsWithTag("TeleportPoint");
		foreach (GameObject stone in stones){
			stone.GetComponent<MagicStoneInterface> ().CreateTurret();
		}

		int turretCount = 0;
		turrets = GameObject.FindGameObjectsWithTag("Turret");
		foreach (GameObject turret in turrets){
			turret.GetComponent<Turret> ().turretNumber = turretCount++;
		}

	}

	public static void CreeperDies(){
		instance.currentCreeperCount--;
		Debug.Log ("Creeper count: " + instance.currentCreeperCount.ToString ());
		if (instance.currentCreeperCount == 0 && instance.currentPhase == WaveManagerPhase.Rest) {
			if (instance.waveNumber+1 < instance.Waves.Length) {
				instance.waveNumber++;
				Debug.Log ("All creepers died, starting next wave");
				instance.StartCoroutine ("WaveQueueRoutine");
			} else {
				Debug.Log("VICTORY!!!!");
				//Victory?... FIXME
			}
		}
	}

	public void StartWaves(){
		waveNumber = 0;
		Debug.Log ("Start waves");
		StartCoroutine("WaveQueueRoutine");
	}

	public void StopSpawn(){
		StopCoroutine("WaveQueueRoutine");
		StopCoroutine ("WaveSpawnCreepersRoutine");
		currentPhase = WaveManagerPhase.Rest;
	}

	private IEnumerator WaveQueueRoutine(){
		currentPhase = WaveManagerPhase.WaveQueue;
		timeBeforeWave = new WaitForSeconds (Waves [waveNumber].beforeWaveTime);
		yield return timeBeforeWave;
		currentCreeperCount = Waves [waveNumber].creeperCount;
		StartCoroutine ("WaveSpawnCreepersRoutine");
	}

	private IEnumerator WaveSpawnCreepersRoutine(){
		currentPhase = WaveManagerPhase.SpawnCreepers;
		int creepersToSpawn = Waves [waveNumber].creeperCount;
		int creepersCount = 0;
		Debug.Log ("Start wave: " + waveNumber.ToString ());
		GameObject creeperPrefab = Waves [waveNumber].creeperPrefab;
		timeBetweenCreepers = new WaitForSeconds (Waves [waveNumber].creeperSpawnTime);
		while (creepersCount < creepersToSpawn) {
			GameObject creeper = Instantiate (Waves[waveNumber].creeperPrefab, transform.position, transform.rotation);
			creeper.GetComponent<Creeper> ().Init (
				Waves[waveNumber].creeperHealth,
				Waves[waveNumber].creeperSpeed, 
				Waves[waveNumber].creeperAccel,
				Waves[waveNumber].creeperAngularSpeed,
				Waves[waveNumber].coinPrefab,
				Waves[waveNumber].coinPValue,
				creeperDestination
			);
			creepersCount++;
			Debug.Log ("Creeper spawned: " + creepersCount.ToString ());
			yield return timeBetweenCreepers;
		}
		currentPhase = WaveManagerPhase.Rest;
		Debug.Log ("Wave " + waveNumber.ToString () + " spawn end");
	}
}
