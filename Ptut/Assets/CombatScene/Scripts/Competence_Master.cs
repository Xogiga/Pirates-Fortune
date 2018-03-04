using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Competence_Master : MonoBehaviour {
	private GameObject infobulle;																				//Ne marche pas à cause du collider ?

	void OnEnable(){
		infobulle = GameObject.Find("Hero(Clone)/Hero Stats Canvas/Barre des competences/Strike/Infobulle");
		infobulle.SetActive (false);
	}

	void OnMouseEnter (){
		
		infobulle.SetActive (true);
	}

	void OnMouseExit()
	{
		infobulle.SetActive (false);
	}
}
