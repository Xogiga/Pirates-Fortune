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
	private int[,] matrice_case;
	private GameObject[] liste_perso;
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
		indice_playing_perso = 0;
	}

	public void Call_event_begin_fight(){
		if(event_begin_fight == null){
			print("l'évènement est vide, problème de priorité");
		}
		if (event_begin_fight != null && is_fight_begin == false)
		{
			is_fight_begin = true;
			is_it_your_turn = true;
			event_begin_fight ();
			matrice_case = script_creation_map.get_matrice_case ();
			liste_perso = script_creation_map.get_liste_perso ();
			print_liste ();

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
		indice_playing_perso ++;																						//Passe au suivant

		if (liste_perso [indice_playing_perso] == null) {																//Si la case suivante est null revient à 0
			indice_playing_perso = 0;
		}

	}


	//Retourne le personnage qui joue son tour
	public GameObject get_playing_perso(){
		return liste_perso [indice_playing_perso];
	}

	public void print_liste(){
		int i = 0;
		for(i=0; i<=10;i++)
		{
			print (liste_perso[i]);
		}
	}

	public void set_matrice_case(int x,int y,int val){																	//Modifie la valeur d'une case de la matrice passé en paramètre
		matrice_case[x,y]=val;
	}

	void Update () {
		if (Input.GetKey ("escape")) 
		{
			Application.Quit ();
		}

	}		


}
