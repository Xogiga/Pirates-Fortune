using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger_BeginEnnemyTurn : MonoBehaviour {

	private GameObject game_manager;
	private GameManager_Master game_master;
	private GameObject player;
	private Deplacement script_player;
	private GameObject bouton_fin_de_tour;
	private GameObject annonce;


	void Set_needed_references()
	{
		game_manager = GameObject.FindGameObjectWithTag ("GameManager");
		game_master = game_manager.GetComponent<GameManager_Master>();
		player = GameObject.FindWithTag ("Player");
		script_player = player.GetComponent<Deplacement> ();
		bouton_fin_de_tour = GameObject.Find ("CombatHUD(Clone)").transform.Find ("Button").gameObject;			//Trouve un objet inactif à partir de son parent
		annonce = GameObject.Find ("CombatHUD(Clone)").transform.Find ("Announce").gameObject;
	}
		

	public void Begin_ennemy_turn(){
		
		Set_needed_references ();
		if (game_master.is_it_your_turn == true && script_player.is_moving == false) {
			game_master.is_it_your_turn = false;
			bouton_fin_de_tour.SetActive (false);
			GameObject ennemi = GameObject.FindWithTag ("Ennemi");
			Attaque deplacement_ennemi = ennemi.GetComponent<Attaque> ();
			deplacement_ennemi.Comportement ();
		}
	}
		
	public void End_ennemy_turn(){
		Set_needed_references ();
		if (game_master.is_it_your_turn == false) {
			script_player.pointDeDeplacement = script_player.statsDeDeplacement;
			script_player.setUI ();
			game_master.is_it_your_turn = true;
			bouton_fin_de_tour.SetActive (true);
			annonce.SetActive (true);
			annonce.GetComponentInChildren<Text> ().text = "Your Turn !";
			StartCoroutine (Disable_UI (annonce));
		}
	}

	IEnumerator Disable_UI(GameObject annonce){
		yield return new WaitForSeconds (1);
		annonce.SetActive (false);
	}
}
