using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NumberDisplayUI : MonoBehaviour {



	public GameObject[] numbersPrefab;
	public Transform[] digitsPlaceHolders;
	private GameObject[] digits;

	private enum DisplayPhase {Animation, Static};
	private string currentData;

	void Awake(){
		currentData = "";
		for (int i = 0; i < digitsPlaceHolders.Length; i++) {
			currentData += " ";
		}
		digits = new GameObject[digitsPlaceHolders.Length];

	}


	public void updateData(int newData){
		string dataStr = newData.ToString ();
		string tmpStr = "";
		Debug.Log ("display change from [" + currentData + "] to [" + dataStr + "]");
		int padding = currentData.Length - dataStr.Length;
		int i;
		for (i = 0; i < padding; i++) {
			if (char.IsDigit(currentData,i)) {
				PhaseOut (i);
			}
			tmpStr += " ";
		}
		for(i=padding;i<digitsPlaceHolders.Length;i++){
			if (!char.IsDigit(currentData,i)) {
				PhaseIn (i,numbersPrefab[(int)char.GetNumericValue(dataStr,i-padding)]);
			}
			else if (!currentData.Substring(i,1).Equals(dataStr.Substring(i - padding,1))) {
				Switch (i, numbersPrefab[(int)char.GetNumericValue(dataStr,i-padding)]);
			}
		}
		currentData = tmpStr + dataStr;

	}

	private void PhaseIn(int position, GameObject numberPrefab){
		
		Transform placeholder = digitsPlaceHolders[position];
		Animation anim = placeholder.gameObject.GetComponent<Animation> ();
		anim.Play();
		digits[position] = Instantiate (numberPrefab, placeholder);
		digits[position].SetActive (false);
		StartCoroutine(DelayedActivation(digits[position],0.25f));
		//FIXME phaseIn animation for placeholder. - instantiate after timeout;
		//FIXME sfx
	}

	private void PhaseOut(int position){
		Transform placeholder = digitsPlaceHolders[position];
		Animation anim = placeholder.gameObject.GetComponent<Animation> ();
		anim.Play();

		//FIXME phaseOut animation for placeholder. Set animation length in destroy time.
		//FIXME sfx
		Destroy(digits[position],0.5f);
	}

	private void Switch(int position, GameObject newObject){
		Transform placeholder = digitsPlaceHolders[position];
		Animation anim = placeholder.gameObject.GetComponent<Animation> ();
		anim.Play();

		//FIXME Switch animation for placeholder. Set half animation length in destroy time.
		//FIXME sfx
		Destroy(digits[position],0.25f);
		digits[position] = Instantiate ( newObject, placeholder);
		digits[position].SetActive (false);
		StartCoroutine(DelayedActivation(digits[position],0.25f));


	} 

	private IEnumerator DelayedActivation(GameObject _target, float _delay){
		yield return new WaitForSeconds (_delay);
		_target.SetActive (true);
	}
}
