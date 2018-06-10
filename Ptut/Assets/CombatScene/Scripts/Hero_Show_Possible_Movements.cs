using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatScene{
public class Hero_Show_Possible_Movements : MonoBehaviour {
	private Hero_Master hero_master;
	private List<SpriteRenderer> sprite_list;
	private int actual_turn;


	void OnEnable(){
		Set_initial_reference ();
	}

	void Set_initial_reference(){
		sprite_list = null;
		hero_master = this.GetComponent<Hero_Master> ();
	}

	//Fonction qui affiche les cases atteignable par le héros
	public void OnMouseEnter(){
		if (References.GameMaster.get_playing_perso().name == this.name && hero_master.is_moving == false) {											//Si le héros survolé ne se déplace pas et que c'est son tour
			Get_Tile_List();																												//Actualise la liste de case
			foreach (SpriteRenderer s in sprite_list) {																						//Pour chaque case
				s.color =  new Color32(35,236,64,255);																						//Change la couleur
			}
			actual_turn = References.GameMaster.get_turn ();																			//Récupère le numéro du tour
		}
	}

	//Fonction qui récupère la liste de case libre autour du joueur
	private void Get_Tile_List(){
		sprite_list = new List<SpriteRenderer>();
		int movement_points = hero_master.Get_Movement_Point();
		Tile[,] grid = References.GameMaster.get_matrice ();

		int HeroX = Mathf.RoundToInt (this.transform.position.x);
		int HeroY =  Mathf.RoundToInt(this.transform.position.y);
		for (int i = HeroX - movement_points; i <= HeroX + movement_points; i++) {															//On récupère les cases autour de lui en fonction de ses points de déplacement
			for (int j = HeroY - movement_points; j <= HeroY + movement_points; j++) {
				if (i> 0 && i <17 && j>0 && j <9) {																							//On vérifie que la case se situe bien dans la grille
					int distance = Mathf.Abs(i - HeroX) + Mathf.Abs(j - HeroY) ;
					if (distance <= movement_points) {																						//On vérifie que la distance à la case est bien inférieure à ses points de mouvement
						if (grid [i, j].state == 0) {																						//On vérifie que la case est innocupé
							References.Pathfinding.Find_Path (this.transform.position, grid [i, j].obj.transform.position);						//Détermine le chemin avec le script de PathFinding
							List<Tile> path = References.Pathfinding.Get_Path ();																	//Récumère le chemin
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
		if (References.GameMaster.get_turn () != actual_turn && sprite_list != null) {
			foreach (SpriteRenderer s in sprite_list) {
					s.color = new Color32(255,255,255,255);
				}
			sprite_list = null;
		}
	}
}
}