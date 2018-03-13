﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Announce_Script : MonoBehaviour {
	
	public void Announce(string message){
		this.gameObject.SetActive (true);
		this.GetComponentInChildren<Text> ().text = message;
		StartCoroutine (Disable_UI ());
	}

	IEnumerator Disable_UI(){																	//Fait disparaitre l'annonce
		yield return new WaitForSeconds (1);
		this.gameObject.SetActive (false);
	}
}
