using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Competence_Master : MonoBehaviour {
	private GameObject infobulle;
	private GameManager_Master game_master;

	public void OnEnable(){
		Set_initial_references ();
	}

	//Reférences fixes
	private void Set_initial_references () {																		
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
		Bind_buttons ();
	}

	private void Bind_buttons(){																																		//PROBLEME, comment changer le paramètre ?
		UnityEngine.Events.UnityAction set_attack = () => {																												//Crée un évènement qui contient la fonction Set_next_attack
			game_master.GetComponent<GameManager_Commands> ().Set_Next_Attack (1);
		};

		Button[] buttons_list = this.GetComponentsInChildren<Button> ();																								//Trouve la liste des boutons
		foreach (Button b in buttons_list) {
			
			b.onClick.AddListener (set_attack);																															//Lie chaque bouton à l'évènement
		}
	}

	private void Find_Infobulle(int Numero_competence){
		infobulle = GameObject.Find("Hero_"+game_master.get_indice_playing_perso()+"/Hero Stats Canvas/Barre des competences/Competence_"+Numero_competence+"/Infobulle_"+Numero_competence);
	}

	public void Show_infobulle(int Numero_competence){
		Find_Infobulle(Numero_competence);
		infobulle.SetActive (true);
	}

	public void Hide_infobulle(int Numero_competence){
		infobulle.SetActive (false);
	}

}



