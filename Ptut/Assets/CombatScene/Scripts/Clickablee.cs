using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickablee : MonoBehaviour {
	public Sprite classic;
	public Sprite mouseover;
	private GameObject game_manager;
	private GameManager_Master game_master;
	private GameObject player;									//Ralentie le jeu ?
	private Hero_Master hero_master;
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
		hero_master = player.GetComponent<Hero_Master> ();
		are_references_set = true;
	}		

		public void OnMouseEnter(){
			if (are_references_set == false) {
				Set_needed_references ();
			}
			if (game_master.is_it_your_turn == true && hero_master.is_moving == false){
				Vector3 distance = this.transform.position - player.transform.position;
				if(Mathf.Round(Mathf.Abs (distance.x) + Mathf.Abs (distance.y)) <= hero_master.point_de_deplacement){						// arrondi car les calculs de floats bug
					this.GetComponent<SpriteRenderer> ().sprite = mouseover;
				}
			}
		}


		public void OnMouseExit(){
			if (game_master.is_it_your_turn == true)
				this.GetComponent<SpriteRenderer> ().sprite = classic;
		}

		void Update(){																														//Permet de réinitialiser les sprites si l'utilisateur fait "Fin de tour" sans bouger sa souris
			if(this.GetComponent<SpriteRenderer> ().sprite == mouseover)
				if(game_master.is_it_your_turn==false)
					this.GetComponent<SpriteRenderer> ().sprite = classic;
		}

	}


