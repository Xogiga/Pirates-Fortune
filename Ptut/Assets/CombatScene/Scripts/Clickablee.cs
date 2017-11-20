using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickablee : MonoBehaviour {
	public Sprite classic;
	public Sprite mouseover;
	private GameObject game_manager;
	private GameManager_Master game_master;
	private GameObject player;									//Ralentie le jeu ?
	private GameObject ennemi;
	private Deplacement script_player;
	private bool are_references_set;

	void OnEnable(){
		Set_initial_references();
	}

	void Set_initial_references(){
		game_manager = GameObject.FindGameObjectWithTag ("GameManager");
		game_master = game_manager.GetComponent<GameManager_Master>();
		are_references_set = false;
	}

	void Set_needed_references(){
		player = GameObject.FindWithTag ("Player");
		ennemi = GameObject.FindWithTag ("Ennemi");
		script_player = player.GetComponent<Deplacement> ();
		are_references_set = true;
	}

	public void OnMouseDown() {
		if (are_references_set == false) {
			Set_needed_references ();
		}
		if (game_master.is_it_your_turn == true && script_player.is_moving == false) {
			if (this.transform.position != player.transform.position && this.transform.position != ennemi.transform.position) {
				script_player.justmove (this.transform.position);
			}
		}
	}

		public void OnMouseEnter(){
			if (are_references_set == false) {
				Set_needed_references ();
			}
			if (game_master.is_it_your_turn == true && script_player.is_moving == false){
						Vector3 distance = this.transform.position - player.transform.position;
						if(Mathf.Round(Mathf.Abs (distance.x) + Mathf.Abs (distance.y)) <= script_player.pointDeDeplacement){						// arrondi car les calculs de floats bug
					if (this.transform.position != player.transform.position && this.transform.position != ennemi.transform.position) {
						this.GetComponent<SpriteRenderer> ().sprite = mouseover;
						}
					}
				}
		}


		public void OnMouseExit(){
			if (game_master.is_it_your_turn == true) {
				this.GetComponent<SpriteRenderer> ().sprite = classic;
			}
		}
	}


