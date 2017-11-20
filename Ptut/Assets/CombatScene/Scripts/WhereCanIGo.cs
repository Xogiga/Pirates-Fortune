using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhereCanIGo : MonoBehaviour {
	public Sprite classic;
	public Sprite mouseover;
	private GameObject game_manager;
	private GameManager_Master game_master;
	private Collider[] collidersList;

	void OnEnable(){
		Set_initial_reference ();
	}

	void Set_initial_reference(){
		collidersList=null;
		game_manager = GameObject.FindGameObjectWithTag ("GameManager");
		game_master = game_manager.GetComponent<GameManager_Master>();
	}

	public void OnMouseEnter(){
		if (game_master.is_it_your_turn == true) {
			Deplacement scriptPlayer = this.GetComponent<Deplacement> ();
			if (scriptPlayer.is_moving == false) {
				collidersList = Physics.OverlapSphere (this.transform.position, (scriptPlayer.pointDeDeplacement));
				foreach (Collider c in collidersList) {
					Vector3 distance = c.transform.position - this.transform.position;
					if (c.CompareTag ("Map")) {																							 //Si l'objet touche fait partie de la map
						if (Mathf.Round(Mathf.Abs (distance.x) + Mathf.Abs (distance.y)) <= scriptPlayer.pointDeDeplacement) {			// si la distance total en x et en y est inférieure au point de déplacement du joueur + arrondi car les calculs de floats bug
							GameObject ennemi = GameObject.FindWithTag ("Ennemi");
							if (c.transform.position != this.transform.position && c.transform.position != ennemi.transform.position) {  // si l'objet n'est pas sous le joueur ou l'ennemi
								c.GetComponent<SpriteRenderer> ().sprite = mouseover;													
							}
						}
					}
				}
			}
		}
	}

	public void OnMouseExit(){
		if (game_master.is_it_your_turn == true) {
			if (collidersList != null) {
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
