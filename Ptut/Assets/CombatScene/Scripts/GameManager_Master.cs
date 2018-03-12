using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Master : MonoBehaviour {
	public delegate void GameManager_EventHandler();  //Gestionnaire d'évènement
	public event GameManager_EventHandler event_begin_fight;
	public event GameManager_EventHandler event_end_fight;

	public bool is_fight_begin;
	public bool is_it_your_turn;
	public bool is_fight_over;
	private GameManager_BeginFight script_creation_map;
	private GameManager_Commands script_commande;
	private int[,] matrice_case;
	private GameObject[] liste_perso;
	private GameObject[] list_case;
	private int indice_playing_perso;


	private void OnEnable(){
		Set_initial_reference();
		Call_event_begin_fight();
	}


	private void Set_initial_reference(){
		is_fight_begin = false;
		is_it_your_turn = false;
		is_fight_over=false;
		script_creation_map = this.GetComponent<GameManager_BeginFight>();
		script_commande = this.GetComponent<GameManager_Commands> ();
		indice_playing_perso = 0;
	}

	//Event qui se déclenche unitquement en début de combat
	public void Call_event_begin_fight(){
		if(event_begin_fight == null){
			print("l'évènement est vide, problème de priorité");
		}
		if (event_begin_fight != null && is_fight_begin == false)
		{
			is_fight_begin = true;																						//Déclare le combat commencé
			is_it_your_turn = true;																						//Déclare que c'est au joueur de jouer (et non l'IA)
			event_begin_fight ();																						//Lance les fonctions de l'event begin_fight :
			matrice_case = script_creation_map.get_matrice_case ();														//Récupère la matrice
			liste_perso = script_creation_map.get_liste_perso ();														//Récupère la liste des personnages dans l'ordre des tours
			liste_perso [indice_playing_perso].GetComponent<Hero_Master> ().activer_desactiver_canvas ();				//Active le canvas du premier héro qui joue
			get_playing_perso ().GetComponent<Hero_Master>().Its_me_mario_FlipFlap();									//FlipFlap
		}
	}		

	public void Call_event_end_fight(){
		if (event_end_fight != null && is_fight_begin == true)
		{
			is_fight_over = true;
			event_end_fight ();
		}
	}


	public void passer_le_tour(){
		if (get_playing_perso ().name == "Hero_" + indice_playing_perso) {												//Si le personnage précédent est un héros
			get_playing_perso ().GetComponent<Hero_Master>().activer_desactiver_canvas ();								//On désactive son Canvas
			get_playing_perso ().GetComponent<Hero_Master>().Reset_Point();												//On lui redonne ses points
			is_it_your_turn = false;																					//On ne donne plus la main au joueur
		}
			
		if (liste_perso [indice_playing_perso + 1] == null) {															//Si la case suivante est null revient à 0
			indice_playing_perso = 0;
		} else {
			indice_playing_perso++;																						//Passe au suivant
		}


		if (get_playing_perso ().name == "Hero_" + indice_playing_perso) {												//Si le personnage suivant est un hero
			is_it_your_turn = true;																						//On donne la main au joueur
			get_playing_perso ().GetComponent<Hero_Master>().activer_desactiver_canvas ();								//On affiche son Canvas
			script_commande.Set_new_references();																		//On donne les références du nouveaux personnage aux commandes
			list_case = GameObject.FindGameObjectsWithTag ("Map");														//On donne les références du nouveaux personnage aux cases
			foreach (GameObject m in list_case) {
				m.GetComponent<Clickablee> ().Set_new_references ();
			}
			get_playing_perso ().GetComponent<Hero_Master> ().Its_me_mario_FlipFlap ();
		}
	}


	//Retourne le personnage qui joue son tour
	public GameObject get_playing_perso(){
		return liste_perso [indice_playing_perso];
	}

	//Retourne le personnage qui joue son tour
	public int get_indice_playing_perso(){
		return indice_playing_perso;
	}

	public void set_matrice_case(int x,int y,int val){																	//Modifie la valeur d'une case de la matrice passé en paramètre
		matrice_case[x,y]=val;
	}

	public int get_matrice_case(int x,int y){																	//Modifie la valeur d'une case de la matrice passé en paramètre
		return matrice_case[x,y];
	}

			


}
