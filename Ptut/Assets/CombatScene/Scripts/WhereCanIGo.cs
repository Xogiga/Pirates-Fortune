using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereCanIGo : MonoBehaviour {
	public Sprite classic;
	public Sprite mouseover;
	private GameObject game_manager;
	private GameManager_Master game_master;
	private Collider[] collidersList;
	private Hero_Master hero_master;

	void OnEnable(){
		Set_initial_reference ();
	}

	void Set_initial_reference(){
		collidersList=null;
		game_manager = GameObject.FindGameObjectWithTag ("GameManager");
		game_master = game_manager.GetComponent<GameManager_Master>();
		hero_master = this.GetComponent<Hero_Master> ();
	}

	public void OnMouseEnter(){
		if (game_master.get_playing_perso().name == this.name) {
			if (hero_master.is_moving == false) {
				collidersList = Physics.OverlapSphere (this.transform.position, (hero_master.point_de_deplacement));
				foreach (Collider c in collidersList) {
					Vector3 distance = c.transform.position - this.transform.position;
					if (c.CompareTag ("Map")) {																							 	//Si l'objet touche fait partie de la map
						if (Mathf.Round(Mathf.Abs (distance.x) + Mathf.Abs (distance.y)) <= hero_master.point_de_deplacement) {				// si la distance total en x et en y est inférieure au point de déplacement du joueur + arrondi car les calculs de floats bug
							if (game_master.get_matrice_case(Mathf.RoundToInt(c.transform.position.x), Mathf.RoundToInt(c.transform.position.y))==0) {  	// si l'objet n'est pas sous le joueur ou l'ennemi
								c.GetComponent<SpriteRenderer> ().sprite = mouseover;													
							}
						}
					}
				}
			}
		}
	}

	public void OnMouseExit(){																												//Permet de réinitialiser les sprites si l'utilisateur enlève sa souris de la case
		if (collidersList != null) {
			foreach (Collider c in collidersList) {
				if (c.CompareTag ("Map")) {
					c.GetComponent<SpriteRenderer> ().sprite = classic;
				}
			}
			collidersList = null;
		}

	}

	void Update(){																															//Permet de réinitialiser les sprites si l'utilisateur fait "Fin de tour" sans bouger sa souris
		if (collidersList != null) {
			if (game_master.is_it_your_turn == false) {
				foreach (Collider c in collidersList) {
					if (c.CompareTag ("Map")) {
						c.GetComponent<SpriteRenderer> ().sprite = classic;
					}
				}
				collidersList = null;
			}
		}
	}
}
