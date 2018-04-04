using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Show_Possible_Movements : MonoBehaviour {
	private GameObject game_manager;
	private GameManager_Master game_master;
	private Hero_Master hero_master;
	private List<SpriteRenderer> sprite_list;
	private GameManager_Pathfinding game_pathfinding;


	void OnEnable(){
		Set_initial_reference ();
	}

	void Set_initial_reference(){
		sprite_list = null;
		game_manager = GameObject.FindWithTag("GameManager");
		game_master = game_manager.GetComponent<GameManager_Master>();
		hero_master = this.GetComponent<Hero_Master> ();
		game_pathfinding = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Pathfinding> ();;
	}

	//Fonction qui affiche les cases atteignable par le héros
	public void OnMouseEnter(){
		if (game_master.get_playing_perso().name == this.name && hero_master.is_moving == false) {											//Si le héros survolé ne se déplace pas et que c'est son tour
			Get_Tile_List();																												//Actualise la liste de case
			foreach (SpriteRenderer s in sprite_list) {																						//Pour chaque case
				s.color =  new Color32(35,236,64,255);																						//Change la couleur
			}
		}
	}

	//Fonction qui récupère la liste de case libre autour du joueur
	private void Get_Tile_List(){
		sprite_list = new List<SpriteRenderer>();
		int movement_points = hero_master.Get_Movement_Point();
		Tile[,] grid = game_master.get_matrice ();

		int HeroX = Mathf.RoundToInt (this.transform.position.x);
		int HeroY =  Mathf.RoundToInt(this.transform.position.y);
		for (int i = HeroX - movement_points; i <= HeroX + movement_points; i++) {															//On récupère les cases autour de lui en fonction de ses points de déplacement
			for (int j = HeroY - movement_points; j <= HeroY + movement_points; j++) {
				if (i> 0 && i <17 && j>0 && j <9) {																							//On vérifie que la case se situe bien dans la grille
					int distance = Mathf.Abs(i - HeroX) + Mathf.Abs(j - HeroY) ;
					if (distance <= movement_points) {																						//On vérifie que la distance à la case est bien inférieure à ses points de mouvement
						if (grid [i, j].state == 0) {																						//On vérifie que la case est innocupé
							game_pathfinding.Find_Path (this.transform.position, grid [i, j].obj.transform.position);						//Détermine le chemin avec le script de PathFinding
							List<Tile> path = game_pathfinding.Get_Path ();																	//Récumère le chemin
							if(path != null && path.Count <= movement_points){																//Vérifie que la longueur du chemin est inférieure au nombre de point de déplacement
								sprite_list.Add (grid [i, j].obj.GetComponent<SpriteRenderer> ());											//Ajoute le sprite de la case à la liste
							}
						}
					}
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
