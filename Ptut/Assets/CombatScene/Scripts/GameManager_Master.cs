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
	private GameManager_BeginFight script_matrice;
	private int[,] matrice_case;

	private void OnEnable(){
		Set_initial_reference();
		Call_event_begin_fight();
	}


	private void Set_initial_reference(){
		is_fight_begin = false;
		is_it_your_turn = false;
		is_fight_over=false;
		script_matrice = this.GetComponent<GameManager_BeginFight>();
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
			matrice_case = script_matrice.get_matrice_case ();
		}
	}		

	public void Call_event_end_fight(){
		if (event_end_fight != null && is_fight_begin == true)
		{
			is_fight_over = true;
			event_end_fight ();
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
