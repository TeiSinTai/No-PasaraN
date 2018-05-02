using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinanceManager : MonoBehaviour {
	public BalanceChangeEvent DisplayBalance;
	private int balance=0;
	private static FinanceManager instance;

	[System.Serializable]
	public class BalanceChangeEvent : UnityEvent<int> {};

	// Use this for initialization
	void Awake () {
		instance = this;
	}
		
	public static int GetBalance(){
		return instance.balance;
	}

	public static void AddCoin(int coinValue){
		//FIXME sfx coin add
		instance.balance += coinValue;
		if (instance.DisplayBalance != null) {
			instance.DisplayBalance.Invoke (instance.balance);
		}
		Debug.Log ("Current balance: " + instance.balance.ToString () + ", added " + coinValue.ToString ());
	}
		

	public static bool TryWithdrawal(int value){
		if (instance.balance >= value) {
			instance.balance -= value;
			if (instance.DisplayBalance != null) {
				instance.DisplayBalance.Invoke (instance.balance);
			}
			//FIXME sfx withdrawal
			//Debug.Log ("Current balance: " + instance.balance.ToString () + ", removed " + value.ToString ());
			return true;
		} else {
			//Debug.Log ("Current balance: " + instance.balance.ToString () + ", tried to remove " + value.ToString ());
			return false;
		}
	}



}
