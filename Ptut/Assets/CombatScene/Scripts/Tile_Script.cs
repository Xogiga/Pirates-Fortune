using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Script : MonoBehaviour {
	public Sprite classic;
	public Sprite mouseover;
	private GameManager_Master game_master;
	private GameObject player;									
	private Hero_Master hero_master;
	private bool are_references_set;

	void OnEnable(){
		Set_initial_references();
	}

	void Set_initial_references(){
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master>();
	}

	public void Set_new_references(){																											//On reprend les références des cases et des personnages à chaque tour allié
		player = game_master.get_playing_perso ();
		hero_master = player.GetComponent<Hero_Master> ();
	}		


	public void OnMouseEnter(){
		Set_new_references ();
		if (game_master.is_it_your_turn == true 																							//Si c'est au tour du joueur de jouer
			&& hero_master.is_moving == false 																									//Si le héros n'est pas déjà entrain de bouger
			&& game_master.get_matrice_case(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y)) == 0){	//Si la case de la matrice est égale à 0
			Vector3 distance = this.transform.position - player.transform.position;
			if(Mathf.Round(Mathf.Abs (distance.x) + Mathf.Abs (distance.y)) <= hero_master.Get_Movement_Point()){							// arrondi car les calculs de floats bug
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
