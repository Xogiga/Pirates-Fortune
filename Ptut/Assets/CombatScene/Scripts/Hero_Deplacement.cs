using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero_Deplacement : MonoBehaviour {
	private Hero_Master hero_master;
	private GameManager_Master game_master;
	private GameManager_Pathfinding game_pathfinding;

	void OnEnable(){
		Set_initial_references ();
	}

	void Set_initial_references()
	{		
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
		game_pathfinding = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Pathfinding> ();
		hero_master = this.GetComponent<Hero_Master> ();
		hero_master.is_moving = false;

	}

	public void try_to_move (Vector3 endposition)
	{
		if (verify_distance (endposition)) {																										//Si le héros a assez de point de déplacement
			game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);			//Change la case de départ en 0 dans la matrice
			game_pathfinding.Find_Path (this.transform.position, endposition);																		//Détermine le chemin avec le script de PathFinding
			List<Tile> path = game_pathfinding.Get_Path ();																							//Récupère le chemin
			StartCoroutine (Move2 (path));																											//Déplace le héros
			game_master.set_matrice_case (Mathf.RoundToInt (endposition.x), Mathf.RoundToInt (endposition.y), 1);									//Change la case de destination en 1 dans la matrice
		}
	}

	//Fonction qui vérifie que le joueur a assez de point de déplacement
	private bool verify_distance(Vector3 endposition){
		Vector3 distance = endposition - this.transform.position; 																					//Récupère la distance entre le joueur et la case
		int xparcours = Mathf.RoundToInt(distance.x);																								//Arrondit xparcours car récupérer le X Y Z directement d'un vecteur bug souvent
		int yparcours = Mathf.RoundToInt(distance.y);

		if ((Mathf.Abs (xparcours) + Mathf.Abs (yparcours)) > hero_master.Get_Movement_Point()) { 													//Vérifie que le joueur a assez de point de déplacement
			return false;
		} else {
			hero_master.Set_Movement_Point(hero_master.Get_Movement_Point() - Mathf.Abs (xparcours) - Mathf.Abs (yparcours));						//Réduit les points de déplacement du héros
			return true;
		}
	}


	IEnumerator Move2(List<Tile> path)
	{
		hero_master.is_moving = true;   																											// Booléen qui empêche d'engager un nouveau déplacement, de rappeller la fonction, avant que le précédent soit fini
		float waittime = 0.04f; 																													//Temps entre chaque micro-déplacement de MoveToward
		float step = 4*waittime; 																													//Vitesse*Temps = distance de MoveTowards
		for (int i = 0; i < path.Count ; i++) {																										//Parcours la liste de case tant qu'il n'est pas arrivé
			Vector3 next_position = new Vector3(path[i].x,path[i].y,0f);																			//Récupère la position de la case suivante

			while (this.transform.position != next_position) {																						//Tant que le héros n'est pas passer à la case suivante
				yield return new WaitForSeconds (waittime);																							
				this.transform.position = Vector3.MoveTowards (this.transform.position, next_position, step);										//Avance vers la case 
			}
		}
		hero_master.is_moving = false;																										 // Booléen qui autorise un nouveau déplacement
	}
}


