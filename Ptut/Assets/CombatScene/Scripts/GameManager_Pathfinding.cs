using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Pathfinding : MonoBehaviour {
	private int width;
	private int height;
	private Tile[,] matrice_case;
	private GameManager_BeginFight script_creation_map;
	private List<Tile> path;

	private void OnEnable(){
		Set_initial_reference ();
	}

	private void Set_initial_reference(){
		script_creation_map = this.GetComponent<GameManager_BeginFight>();
		matrice_case = script_creation_map.get_matrice_case ();
		width = script_creation_map.Get_Width ();
		height = script_creation_map.Get_Height ();
	}

	//Fonction qui trouve la case dans la matrice à partir de sa position
	Tile Get_Tile_From_Vector3 (Vector3 position){
		return matrice_case[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)];
	}

	//Fonction qui cherche la prochaine case avec la meilleur valeur possible
	public void Find_Path(Vector3 startPos, Vector3 endPos){
		Tile start_tile = Get_Tile_From_Vector3 (startPos);														//Case de départ
		Tile target_tile = Get_Tile_From_Vector3 (endPos);														//Case d'arrivée

		List<Tile> open_set = new List<Tile> ();																//Liste des cases à traiter
		HashSet<Tile> closed_set = new HashSet<Tile> ();														//Liste des cases déjà traitée
		open_set.Add (start_tile);																				//Ajoute la case de départ

		while (open_set.Count > 0) {																			//Boucle tant que la liste de case à traiter n'est pas vide
			Tile current_tile = open_set[0];
			for (int i = 1; i < open_set.Count; i++) {
				if (open_set [i].fCost < current_tile.fCost ||													//Parcours les cases à inspecter jusqu'à trouver celle qui à le meilleur fCost (Valeur qui cumule la distance entre la case de départ et d'arrivée)
					open_set[i].fCost == current_tile.fCost && open_set[i].hCost < current_tile.hCost) {		//Ou celle qui a un fCost équivalent mais un hCost meilleur (distance entre la case et l'arrivée)											
					current_tile = open_set [i];
				}
			}
			open_set.Remove (current_tile);																		//Supprime la case avec le meilleur coût de la liste de case à inspecter
			closed_set.Add (current_tile);																		//Ajoute cette case à la liste des cases déjà traitée

			if (current_tile.x == target_tile.x && current_tile.y == target_tile.y) {							//Si la case que l'on traite est la case d'arrivée
				path = Retrace_Path(start_tile,target_tile);													//Enregistre le chemin dans la valeur path
				return;																							//Sort de la fonction
			}

			foreach (Tile neighbour in Get_Neighbours(current_tile)) {											//Boucle qui parcours les cases voisines de la case étudiée
				if (neighbour.state == 1 || closed_set.Contains (neighbour))									//Ne tient pas compte des cases "obstacles" ou déjà traitées
					continue;

				int movement_cost_to_neighbour = current_tile.gCost + Get_Distance (current_tile, neighbour);	//Calcule la distance entre la case voisine et la case de départ en passant par la case étudiée
				if (movement_cost_to_neighbour < neighbour.gCost || !open_set.Contains (neighbour)) {			//Si cette distance est inférieure à l'ancienne valeur gCost ou si la case n'est pas dans la liste des cases à étudier
					neighbour.gCost = movement_cost_to_neighbour;												//On actualise les valeurs gCost et hCost de la case voisine
					neighbour.hCost = Get_Distance (neighbour, target_tile);
					neighbour.parent = current_tile;															//Et on définit la case actuelle comme parente de la case voisine

					if (!open_set.Contains (neighbour)) {														//Si la case voisine ne fait pas partie des case à étudier
						open_set.Add (neighbour);																//Ajoute la case voisine à la liste des cases à étudier
					}
				}
			}
		}
	}


	//Fonction qui détermine les cases voisines de celle étudiée
	List<Tile> Get_Neighbours(Tile current_tile){
		List<Tile> Neighbours = new List<Tile> ();																//Liste des cases voisines

		for (int x = -1; x <= 1; x++) {																			//Parcours les cases cases autours
			for (int y = -1; y <= 1; y++) {
				if (x + y != 1 && x + y != -1)																	//Elimine les cases diagonales et centrales													
					continue;

				int checkX = current_tile.x + x;
				int checkY = current_tile.y + y;

				if (checkX > 0 && checkX < width && checkY > 0 && checkY < height) {							//Vérifie que la case voisine est dans la grille
					Neighbours.Add (matrice_case [checkX, checkY]);												//Si oui, l'ajoute à la liste de voisines
				}
			}
		}
		return Neighbours;
	}

	//Fonction qui détermine la distance entre deux cases
	int Get_Distance(Tile tileA, Tile tileB){
		int distanceX = Mathf.Abs (tileA.x - tileB.x);
		int distanceY = Mathf.Abs (tileA.y - tileB.y);

		return distanceX + distanceY;
	}

	//Fonction qui parcours le chemin inverse des parents
	List<Tile> Retrace_Path(Tile start_tile,Tile end_tile){
		List<Tile> path = new List<Tile> ();
		Tile current_tile = end_tile;

		while (current_tile != start_tile) {																	//Tant qu'on n'a pas parcouru toutes les cases parentes 
			path.Add (current_tile);																			//Ajoute la case au chemin
			current_tile = current_tile.parent;																	//Et passe à son parent
		}

		path.Reverse ();																						//Inverse l'ordre de la liste

		return path;
	}

	//Fonction qui retourne le chemin de case à parcourir
	public List<Tile> Get_Path(){
		return path;
	}
}
