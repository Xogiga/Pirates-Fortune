using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger_BeginEnnemyTurn : MonoBehaviour {

	private GameManager_Master game_master;
	private Hero_Master hero_master;
	private GameObject bouton_fin_de_tour;
	private Announce_Script annonce;
	private bool are_references_set;

	void OnStart(){
		are_references_set = false;	
	}

	void Set_needed_references()
	{
		game_master = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Master>();
		hero_master = GameObject.FindWithTag ("Player").GetComponent<Hero_Master> ();
		bouton_fin_de_tour = GameObject.Find ("CombatHUD(Clone)").transform.Find ("Button").gameObject;			//Trouve un objet inactif à partir de son parent
		annonce = GameObject.Find ("CombatHUD(Clone)").transform.Find ("Announce").gameObject.GetComponentInChildren<Announce_Script>();
		are_references_set = true;
	}
		

	public void Begin_ennemy_turn(){
		if (are_references_set == false)
			Set_needed_references ();
		if (game_master.is_it_your_turn == true && hero_master.is_moving==false) {
			
			game_master.is_it_your_turn = false;
			bouton_fin_de_tour.SetActive (false);

			annonce.Announce ("Ennemy Turn !");

			GameObject ennemi = GameObject.FindWithTag ("Ennemi");
			Attaque deplacement_ennemi = ennemi.GetComponent<Attaque> ();
			deplacement_ennemi.Comportement ();
		}
	}
		
	public void End_ennemy_turn(){
		Set_needed_references ();
		if (game_master.is_it_your_turn == false) {
			hero_master.Reset_Point();
			game_master.is_it_your_turn = true;
			bouton_fin_de_tour.SetActive (true);
			annonce.Announce ("Your Turn !");
		}
	}


}
