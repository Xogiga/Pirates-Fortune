using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Competence_Master : MonoBehaviour {
	private GameObject infobulle;

	private void Find_Infobulle(int Numero_competence){
		infobulle = GameObject.Find("Hero(Clone)/Hero Stats Canvas/Barre des competences/Competence_"+Numero_competence+"/Infobulle_"+Numero_competence);
	}

	public void Show_infobulle(int Numero_competence){
		Find_Infobulle(Numero_competence);
		infobulle.SetActive (true);
	}

	public void Hide_infobulle(int Numero_competence){
		infobulle.SetActive (false);
	}

}



