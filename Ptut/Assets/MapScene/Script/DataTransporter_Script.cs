using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTransporter_Script : MonoBehaviour {
	public static DataTransporter_Script data;
	private FightParameters FP;

	public void Set_Fight_Parameters(FightParameters fp){
		FP = fp;
		DontDestroyOnLoad (this.gameObject);					//Rend l'objet persistant entre les scènes.
		data = this;
	}

	public FightParameters Get_Fight_Parameters(){
		return FP;
	}
}