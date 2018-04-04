using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave {
	[Header ("Wave timings")]
	public float beforeWaveTime = 2.0f;
	public float creeperSpawnTime = 1.0f;

	[Header ("Creepers")]
	public int creeperCount = 10;
	public int creeperHealth=85;
	public float creeperSpeed=3.5f;
	public float creeperAngularSpeed = 120.0f;
	public float creeperAccel = 8.0f;
	public float coinPValue = 0.5f;
	public GameObject creeperPrefab;
	public GameObject coinPrefab;


}
