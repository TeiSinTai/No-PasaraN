using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager: MonoBehaviour{
	public Transform creeperDestination;

	public CreeperDiesEvent DisplayCreepers;
	public WaveQueuedEvent WaveQueued;
	private int deadCreeperCount=0;


	[System.Serializable]
	public class CreeperDiesEvent : UnityEvent<int> {};

	[System.Serializable]
	public class WaveQueuedEvent : UnityEvent<float,int> {}; //queued wave number, number of wave

	public int startMoney;
	public Wave[] Waves;
	private WaitForSeconds timeBeforeWave;
	private WaitForSeconds timeBetweenCreepers;
	private static WaveManager instance;
	private int waveNumber;
	private int currentCreeperCount=0;
	public enum WaveManagerPhase {Rest, WaveQueue, SpawnCreepers, StopGame};
	public WaveManagerPhase currentPhase;

	public static WaveManagerPhase getCurrentPhase(){
		return instance.currentPhase;
	}

	public static int wavesNumber(){
		return instance.Waves.Length;
	}

	void Awake(){
		instance = this;
		currentPhase = WaveManagerPhase.StopGame;
	}

	void Start(){
		StartWaves (); //FIXME game start at user input, not on load

		// DEBUG!!!! Create ALL turrets in script. FIXME v0.2
		//GameObject[] turrets;
		//GameObject[] stones;
		//stones = GameObject.FindGameObjectsWithTag("TeleportPoint");
		//foreach (GameObject stone in stones){
		//	stone.GetComponent<MagicStoneInterface> ().CreateTurret();
		//}

		//int turretCount = 0;
		//turrets = GameObject.FindGameObjectsWithTag("Turret");
		//foreach (GameObject turret in turrets){
		//	turret.GetComponent<Turret> ().turretNumber = turretCount++;
		//}

	}

	public static void CreeperDies(){
		instance.currentCreeperCount--;
		instance.deadCreeperCount++;
		if (instance.DisplayCreepers != null) {
			instance.DisplayCreepers.Invoke (instance.deadCreeperCount);
		}
		Debug.Log ("Creeper count: " + instance.currentCreeperCount.ToString ());
		if (instance.currentCreeperCount == 0 && instance.currentPhase == WaveManagerPhase.Rest) {
			if (instance.waveNumber+1 < instance.Waves.Length) {
				instance.waveNumber++;
				Debug.Log ("All creepers died, starting next wave");
				instance.StartCoroutine ("WaveQueueRoutine");
			} else {
				Victory ();
				//Victory?... FIXME
			}
		}
	}

	public void StartWaves(){
		waveNumber = 0;
		currentPhase = WaveManagerPhase.Rest;
		Debug.Log ("Start waves");
		FinanceManager.AddCoin (startMoney);
		StartCoroutine("WaveQueueRoutine");
	}

	public void StopSpawn(){
		StopCoroutine("WaveQueueRoutine");
		StopCoroutine ("WaveSpawnCreepersRoutine");
		currentPhase = WaveManagerPhase.Rest;
	}

	public static void Victory(){
		//Gather all coins left
		GameObject[] coins;
		coins = GameObject.FindGameObjectsWithTag("Coin");
		foreach (GameObject coin in coins)
		{
			coin.GetComponent<Coin> ().FastGather ();
		}
		//FIXME add small delay for balance accounting, playing coin gather sfx, anything else.
		//fill it with fireworks, for example.

		//Output balance
		Debug.Log ("Victory. Balance: " + FinanceManager.GetBalance());
		instance.StopAllCoroutines ();
		instance.currentPhase = WaveManagerPhase.StopGame;
	}

	public static void Defeat(){
		GameObject[] coins;
		coins = GameObject.FindGameObjectsWithTag("Coin");
		foreach (GameObject coin in coins)
		{
			coin.GetComponent<Coin> ().FastPerish ();
		}
		instance.StopAllCoroutines ();
		instance.currentPhase = WaveManagerPhase.StopGame;
		Debug.Log ("Defeat");
	}

	private IEnumerator WaveQueueRoutine(){
		//FIXME Wave Event 
		if(WaveQueued!=null){
			WaveQueued.Invoke (Waves [waveNumber].beforeWaveTime, waveNumber);
		}
		currentPhase = WaveManagerPhase.WaveQueue;
		timeBeforeWave = new WaitForSeconds (Waves [waveNumber].beforeWaveTime);
		yield return timeBeforeWave;
		currentCreeperCount = Waves [waveNumber].creeperCount;
		//FIXME sfx wave start
		StartCoroutine ("WaveSpawnCreepersRoutine");
	}

	private IEnumerator WaveSpawnCreepersRoutine(){
		currentPhase = WaveManagerPhase.SpawnCreepers;
		int creepersToSpawn = Waves [waveNumber].creeperCount;
		int creepersCount = 0;
		Debug.Log ("Start wave: " + waveNumber.ToString ());

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
				Waves[waveNumber].banishCost,
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
