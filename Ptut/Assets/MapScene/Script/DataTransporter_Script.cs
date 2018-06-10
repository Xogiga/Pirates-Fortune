using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransporter_Script : MonoBehaviour {
	public static DataTransporter_Script data;
	private FightParameters FP;
	private bool victory;

	//Fonction qui permet à l'objet de ne pas se détruire durant les changement de scène 
	//et qui donne sa référence en static
	private void Make_Invincible(){
		data = this;
		DontDestroyOnLoad (this.gameObject);					//Rend l'objet persistant entre les scènes.
	}

	public void Set_Fight_Parameters(FightParameters fp){
		Make_Invincible ();
		FP = fp;
	}

	public FightParameters Get_Fight_Parameters(){
		return FP;
	}

	public void Set_Victory(bool v){
		Make_Invincible();
		victory = v;
	}

	public bool Get_Victory(){
		return victory;
	}
}