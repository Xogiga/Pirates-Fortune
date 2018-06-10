using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatScene{
public class Tile_Script : MonoBehaviour {
	private GameObject player;									
	private Hero_Master hero_master;
	private List<Tile> path;
	private int actual_turn;


	public void Set_new_references(){																											//On reprend les références des cases et des personnages à chaque tour allié
		path = null;
		player = References.GameMaster.get_playing_perso ();
		hero_master = player.GetComponent<Hero_Master> ();
	}		


	//Fonction qui change le sprite de la case survolée si elle est accessible
	public void OnMouseEnter(){
		if (EventSystem.current.IsPointerOverGameObject ())																						//Si le pointeur est au dessus d'un élément de l'ATH, sort de la fonction.
			return;
		Set_new_references ();
		if (References.GameMaster.is_it_your_turn == true 																						//Si c'est au tour du joueur de jouer
			&& hero_master.is_moving == false 																									//Si le héros n'est pas déjà entrain de bouger
			&& References.GameMaster.get_matrice_case(Mathf.RoundToInt(this.transform.position.x), Mathf.RoundToInt(this.transform.position.y)) == 0){	//Si la case de la matrice est égale à 0
			References.Pathfinding.Find_Path (player.transform.position, this.transform.position);												//Détermine le chemin entre le héros et la case
			path = References.Pathfinding.Get_Path ();																							//Récupère le chemin
				if (path != null && path.Count <=  hero_master.Get_Movement_Point()) {															//Si le chemin existe et est accessible avec les points de mouvements disponibles
					foreach (Tile t in path) {																									//Change les sprite
					t.obj.GetComponent<SpriteRenderer> ().color = new Color32(35,236,64,255);
						}
				}
		}
		actual_turn = References.GameMaster.get_turn ();																						//Récupère le numéro du tour
	}

	//Fonction qui permet de réinitialiser les sprites si l'utilisateur enlève sa souris de la case
	public void OnMouseExit(){
		if (References.GameMaster.is_it_your_turn == true && path != null) {
			foreach (Tile t in path) {																											//Change tous les sprites du chemin
				t.obj.GetComponent<SpriteRenderer> ().color = new Color32(255,255,255,255);
			}
			path = null;
		}
	}

	//Fonction qui permet de réinitialiser les sprites si l'utilisateur fait "Fin de tour" sans bouger sa souris
	void Update(){																														
		if (References.GameMaster.get_turn () != actual_turn && path != null) {
			foreach (Tile t in path) {																											//Change tous les sprites du chemin
				t.obj.GetComponent<SpriteRenderer> ().color = new Color32(255,255,255,255);
			}
			path = null;
		}

	}
}
}