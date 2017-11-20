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

	private void OnEnable(){
		Set_initial_reference();
		Call_event_begin_fight();
	}

	private void Set_initial_reference(){
		is_fight_begin = false;
		is_it_your_turn = false;
		is_fight_over=false;
	}

	public void Call_event_begin_fight(){
		if (event_begin_fight != null && is_fight_begin == false)
		{
			is_fight_begin = true;
			is_it_your_turn = true;
			event_begin_fight ();
		}
	}		

	public void Call_event_end_fight(){
		if (event_end_fight != null && is_fight_begin == true)
		{
			is_fight_over = true;
			event_end_fight ();
		}
	}

	void Update () {
		if (Input.GetKey ("escape")) 
		{
			Application.Quit ();
		}

	}
}
