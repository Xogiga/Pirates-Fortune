using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Show_Possible_Movements : MonoBehaviour {
	private GameObject game_manager;
	private GameManager_Master game_master;
	private Hero_Master hero_master;
	private GameManager_Pathfinding game_pathfinding;
	private List<SpriteRenderer> sprite_list;


	void OnEnable(){
		Set_initial_reference ();
	}

	void Set_initial_reference(){
		sprite_list = null;
		game_manager = GameObject.FindWithTag("GameManager");
		game_master = game_manager.GetComponent<GameManager_Master>();
		hero_master = this.GetComponent<Hero_Master> ();
		game_pathfinding = game_manager.GetComponent<GameManager_Pathfinding> ();
	}

	//Fonction qui affiche les cases atteignable par le héros
	public void OnMouseEnter(){
		if (game_master.get_playing_perso().name == this.name && hero_master.is_moving == false) {											//Si le héros survolé ne se déplace pas et que c'est son tour
			Get_Tile_List();																												//Actualise la liste de case
			foreach (SpriteRenderer s in sprite_list) {																						//Pour chaque case
				s.color =  new Color32(255,45,36,255);																						//Change la couleur
			}
		}
	}

	//Fonction qui récupère la liste de case libre autour du joueur
	private void Get_Tile_List(){
		sprite_list = new List<SpriteRenderer>();
		int movement_points = hero_master.Get_Movement_Point();

		Collider[] collidersList = Physics.OverlapSphere (this.transform.position, movement_points);										//On récupère les objets autour de lui en fonction de ses points de déplacement

		foreach (Collider c in collidersList) {																								//Pour chacun des objets touchés
			int tileX = Mathf.RoundToInt(c.transform.position.x);
			int tileY =  Mathf.RoundToInt(c.transform.position.y);
			if (c.CompareTag ("Map") && game_master.get_matrice_case(tileX,tileY) == 0) {													//Si l'objet touché est une case de la Map et qu'elle est vide
				game_pathfinding.Find_Path (this.transform.position, c.transform.position);													//Détermine le chemin avec le script de PathFinding
				List<Tile> path = game_pathfinding.Get_Path ();																				//Récupère le chemin
				if (path != null && path.Count <= movement_points) {																		//Si le chemin existe et est accessible avec les points de mouvements disponibles
					sprite_list.Add(c.GetComponent<SpriteRenderer> ());																		//Ajoute le sprite à la liste
				}
			}
		}
	}

	//Fonction qui permet de réinitialiser les sprites si l'utilisateur enlève sa souris de la case
	public void OnMouseExit(){																												
		if (sprite_list != null) {
			foreach (SpriteRenderer s in sprite_list) {
				s.color =  new Color32(255,255,255,255);
			}
			sprite_list = null;
		}
	}

	//Permet de réinitialiser les sprites si l'utilisateur fait "Fin de tour" sans bouger sa souris
	void Update(){																															
		if (game_master.is_it_your_turn == false && sprite_list != null) {
			foreach (SpriteRenderer s in sprite_list) {
					s.color = new Color32(255,255,255,255);
				}
			sprite_list = null;
		}
	}
}
