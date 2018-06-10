using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CombatScene{
public class Hero_Deplacement : MonoBehaviour {
	private Hero_Master hero_master;
	private GameManager_Master game_master;
	private GameManager_Pathfinding game_pathfinding;
	private CombatHUD_Master combatHUD_master;

	void OnEnable(){
		Set_initial_references ();
	}

	void Set_initial_references()
	{		
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
		game_pathfinding = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Pathfinding> ();
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();	
		hero_master = this.GetComponent<Hero_Master> ();
		hero_master.is_moving = false;

	}

	public void try_to_move (Vector3 endposition)
	{
		if (verify_distance (endposition)) {																										//Si le héros a assez de point de déplacement
			game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);			//Change la case de départ en 0 dans la matrice
			List<Tile> path = game_pathfinding.Get_Path ();																							//Récupère le chemin
			StartCoroutine (Move2 (path));																											//Déplace le héros
			game_master.set_matrice_case (Mathf.RoundToInt (endposition.x), Mathf.RoundToInt (endposition.y), 1);									//Change la case d'arrivée en 1 dans la matrice
		}
	}

	//Fonction qui vérifie que le joueur a assez de point de déplacement
	private bool verify_distance(Vector3 endposition){
		game_pathfinding.Find_Path (this.transform.position, endposition);																			//Détermine le chemin avec le script de PathFinding
		List<Tile> path = game_pathfinding.Get_Path ();

		if (path == null) {
			combatHUD_master.Announce ("You can't go there");
			return false;
		} else if (path.Count > hero_master.Get_Movement_Point ()) {
			combatHUD_master.Announce ("Not enought Movement Points");
			return false;
		} else {																																	//Si le chemin existe et que le héros possède assez de point de déplacement
			hero_master.Set_Movement_Point (hero_master.Get_Movement_Point () - path.Count);														//Réduit les points de déplacement du héros
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
			Side_flip (next_position);																												//Fonction qui le fait pivoter dans sa direction

			while (this.transform.position != next_position) {																						//Tant que le héros n'est pas passer à la case suivante
				yield return new WaitForSeconds (waittime);																							
				this.transform.position = Vector3.MoveTowards (this.transform.position, next_position, step);										//Avance vers la case 
			}
		}
		hero_master.is_moving = false;																												// Booléen qui autorise un nouveau déplacement
	}

	//Fonction qui tourne le personnage en fonction de sa direction
	private void Side_flip(Vector3 next_position){
		SpriteRenderer sprite = this.transform.GetChild(1).GetComponent<SpriteRenderer>();
		if (this.transform.position.x < next_position.x) {																							//Si sa direction est à droite
			sprite.flipX  = false;																													//Regarde à drotie
		} else if (this.transform.position.x > next_position.x) {																					//Sinon l'inverse
			sprite.flipX  = true;	
		}
	}
}
}