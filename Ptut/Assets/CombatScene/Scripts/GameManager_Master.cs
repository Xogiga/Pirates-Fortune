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
	private Tile[,] matrice_case;
	private GameObject[] liste_perso;
	private GameObject[] list_case;
	private int indice_playing_perso;
	private CombatHUD_Master combatHUD_master ;

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
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();	
	}


	//Event qui se déclenche unitquement en début de combat
	public void Call_event_begin_fight(){
		if(event_begin_fight == null){
			print("l'évènement est vide, problème de priorité");
		}
		if (event_begin_fight != null && is_fight_begin == false)
		{
			is_fight_begin = true;																						//Déclare le combat commencé
			event_begin_fight ();																						//Lance les fonctions de l'event begin_fight :
			matrice_case = script_creation_map.get_matrice_case ();														//Récupère la matrice
			liste_perso = script_creation_map.get_liste_perso ();														//Récupère la liste des personnages dans l'ordre des tours
			this.GetComponent<GameManager_Commands>().enabled = true;													//Active le script des commandes
			begin_hero_turn();
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
			end_hero_turn();
		}

		if (get_playing_perso ().name == "Ennemi_" + indice_playing_perso) {											//Si le personnage précédent est un ennemi
			end_ennemy_turn();
		}
			
		if (liste_perso [indice_playing_perso + 1] == null) {															//Si la case suivante est null revient à 0
			indice_playing_perso = 0;
		} else {
			indice_playing_perso++;																						//Passe au personnage suivant
		}


		if (get_playing_perso ().name == "Hero_" + indice_playing_perso) {												//Si le personnage suivant est un héros
			begin_hero_turn();
		}

		if (get_playing_perso ().name == "Ennemi_" + indice_playing_perso) {											//Si le personnage suivant est un ennemi
			begin_ennemy_turn();
		}
	}

	//Gère la fin de tour allié
	private void end_hero_turn(){
		combatHUD_master.enable_disable_button_and_stats ();															//On désactive les infos du héros et le bouton fin de tour
		get_playing_perso ().GetComponent<Hero_Master>().Reset_Point();													//On lui redonne ses points
	}

	//Gère la fin de tour ennemi
	private void end_ennemy_turn(){
		get_playing_perso ().GetComponent<Ennemy_Master>().Reset_Point();
	}

	//Gère le début de tour allié
	private void begin_hero_turn(){
		combatHUD_master.Announce ("Your Turn !");																		//Annonce le tour allié
		combatHUD_master.Set_Hero_Points (get_playing_perso());															//On affiche ses stats
		combatHUD_master.enable_disable_button_and_stats ();															//On désactive les infos du héros et le bouton fin de tour
		script_commande.Set_new_references();																			//On donne les références du nouveaux personnage aux commandes

		get_playing_perso ().GetComponent<Hero_Master> ().Its_me_mario_FlipFlap ();
		is_it_your_turn = true;																							//On donne la main au joueur

	}

	//Gère le début de tour ennemi
	private void begin_ennemy_turn(){
		is_it_your_turn = false;																						//On ne donne plus la main au joueur
		combatHUD_master.Announce ("Ennemy Turn !");																	//Annonce le tour ennemi
		get_playing_perso ().GetComponent<Ennemy_Master> ().Comportement ();											//Appel son comportement
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
		matrice_case[x,y].state = val;
	}

	public int get_matrice_case(int x,int y){																			//Renvoie la valeur d'une case de la matrice passé en paramètre
		return matrice_case[x,y].state;
	}

			


}
