using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveDisplay : MonoBehaviour {
	private int numberOfWaves;
	public float goingUpTime=1.0f;
	public GameObject bigWave;
	public GameObject[] waves;
	public Transform topPosition;
	public Transform forwardPosition;
	public float waveMovingTime=1.0f;
	private WaitForSeconds timeBeforeWave;
	private WaitForSeconds timeBeforeForward;
	private Animator waveAnimator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void WaveInit(int _numberOfWaves){
		numberOfWaves = _numberOfWaves;
		//FIXME in the future create gameObjects for waes dynamically;
		StartCoroutine("GoingUp");
	}

	private IEnumerator GoingUp(){
		float startTime = Time.time;
		float endTime = Time.time + goingUpTime;
		Vector3 oldPosition = transform.position;
		while (Time.time < endTime) {
			//transform.position.Set (oldPosition.x, Mathf.Lerp (oldPosition.y, topPosition.position.y, (Time.time-startTime) / goingUpTime),oldPosition.z);
			transform.position = VectorLerp (oldPosition,topPosition.position ,  (Time.time-startTime) / goingUpTime);
			yield return null;
		}
	}

	public void QueueWave(float _timeBeforeWave, int _waveNumber){
		timeBeforeWave = new WaitForSeconds (_timeBeforeWave - waveMovingTime);
		waveAnimator = waves [_waveNumber].GetComponent<Animator> ();
		timeBeforeForward = new WaitForSeconds (_timeBeforeWave);
		StartCoroutine ("WaveStart");
		StartCoroutine ("GoingForward");
	}

	private IEnumerator WaveStart(){
		yield return timeBeforeWave;
		waveAnimator.SetTrigger("Wave Down");
		bigWave.GetComponent<Animator>().SetTrigger("WaveUp");
	}

	private IEnumerator GoingForward(){
		yield return timeBeforeForward;
		float startTime = Time.time;
		float endTime = Time.time + goingUpTime; //FIXME - do we need special timer for that?
		Vector3 oldPosition = transform.position;
		while (Time.time < endTime) {
			transform.position = VectorLerp (oldPosition,oldPosition-topPosition.position+forwardPosition.position ,  (Time.time-startTime) / goingUpTime);
			yield return null;
		}
	}

	private Vector3 VectorLerp(Vector3 a, Vector3 b, float t){
		Vector3 res;
		res.x = Mathf.Lerp (a.x, b.x, t);
		res.y = Mathf.Lerp (a.y, b.y, t);
		res.z = Mathf.Lerp (a.z, b.z, t);
		return res;
	}

	public void resetWave(){
		bigWave.GetComponent<Animator> ().SetTrigger ("WaveDown");
	}

}
