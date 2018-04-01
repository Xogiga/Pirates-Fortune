using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_Master : MonoBehaviour {

	private GameManager_Master game_master;
	private int ennemy_movement_point;
	private int max_range;
	private int health_stat;
	private int current_health;
	private int action_stat;
	private int action_point;
	private CombatHUD_Master combatHUD_master;
	private GameManager_Pathfinding game_pathfinding;


	void OnEnable()
	{
		Set_initial_reference();
	}

	private void Set_initial_reference(){
		health_stat = 100;
		current_health = health_stat;
		ennemy_movement_point = 4;
		max_range = 5;
		action_stat = 5;
		action_point = action_stat;
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();
		game_pathfinding = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Pathfinding> ();
	}

	//Active les stats de l'ennemi
	void OnMouseEnter()
	{
		combatHUD_master.Set_Ennemy_Health (this.gameObject);
		combatHUD_master.enable_disable_ennemy_stats ();
	}

	//Désctive les stats de l'ennemi
	void OnMouseExit()
	{
		combatHUD_master.enable_disable_ennemy_stats ();
	}

	public int Get_Health_Stats(){
		return health_stat;
	}

	public int Get_Current_Health(){
		return current_health;
	}

	public void IncreaseHealth(int health_change)
	{
		int previous_health = current_health;
		current_health += health_change;
		if (current_health > health_stat) {
			current_health = health_stat;
		}
		combatHUD_master.Change_Ennemy_Health (previous_health, current_health, health_stat);
	}

	public void DeductHealth(int health_change){
		int previous_health = current_health;
		current_health -= health_change;
		if (current_health < 0) {
			current_health = 0;
		}
		combatHUD_master.Change_Ennemy_Health (previous_health, current_health, health_stat);
	}


	//Fonction qui remet les points au max (fin de tour)
	public void Reset_Point()
	{
		action_point = action_stat;
	}

	//Fonction qui fait agir l'IA
	public void Comportement(){
		int distance;
		GameObject hero_target = Target_Choice_by_distance (out distance);									//Trouve la cible

		if (distance > 1) {																					//Si la cible n'est pas déjà au corps à corps
			Vector3 end_position = Choice_side (hero_target);												//Choisi sa destination
			if (end_position != Vector3.zero) {																//Si la destination est accessible
				try_to_move (end_position);																	//Se déplace
			} else {
				Comportement_Suite();
			}
		} else {
			Comportement_Suite ();
		}
	}

	//Fonction qui fait agir l'IA après le déplacement
	private void Comportement_Suite(){
		int int_distance;
		GameObject target;
		if (Can_I_Attack (out int_distance, out target)) {												//Vérifie qu'il y a un ennemi a porté
			combatHUD_master.Set_Hero_Health(target.gameObject);										//Affiche la barre de vie de la cible
			StartCoroutine(Attack (int_distance, target));												//Attaque la cible
		} else {
			game_master.passer_le_tour (); 
		}
	}

	//Fonction qui fait déplacer l'ennemi
	public void try_to_move (Vector3 endposition)
	{
		game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);			//Change la case de départ en 0 dans la matrice
		game_pathfinding.Find_Path (this.transform.position, endposition);																		//Détermine le chemin avec le script de PathFinding
		List<Tile> path = game_pathfinding.Get_Path ();																							//Récupère le chemin
		StartCoroutine (Move2 (path));																											//Déplace le héros
	}

	IEnumerator Move2(List<Tile> path)
	{																																			// Booléen qui empêche d'engager un nouveau déplacement, de rappeller la fonction, avant que le précédent soit fini
		float waittime = 0.04f; 																												//Temps entre chaque micro-déplacement de MoveToward
		float step = 4*waittime; 																												//Vitesse*Temps = distance de MoveTowards
		for (int i = 0; i < ennemy_movement_point && i<path.Count; i++) {																		//Parcours la liste de case tant qu'il n'est pas arrivé ou tant qu'il a des points de déplacement
			Vector3 next_position = new Vector3(path[i].x,path[i].y,0f);																		//Récupère la position de la case suivante

			while (this.transform.position != next_position) {																					//Tant que le héros n'est pas passer à la case suivante
				yield return new WaitForSeconds (waittime);																							
				this.transform.position = Vector3.MoveTowards (this.transform.position, next_position, step);									//Avance vers la case 
			}
		}																										 								// Booléen qui autorise un nouveau déplacement
		End_Deplacement();
	}

	//Fonction qui gère la  fin de déplacement
	private void End_Deplacement(){
		game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 1);			//Change la case de d'arrivé en 1 dans la matrice
		Comportement_Suite();
	}


	//Fonction qui détermine la cible de l'attaque
	private GameObject Target_Choice_by_distance(out int distance_min){
		GameObject[] heros = GameObject.FindGameObjectsWithTag ("Hero");  								//Trouve la liste des héros
		GameObject hero = null;																			//Cible de l'attaque
		Vector3 total_path;
		distance_min = 1000;
		int distance_comparé;

		foreach (GameObject h in heros) {																//Compare les distances entre l'ennemi et les héros
			total_path = h.transform.position - this.transform.position;
			distance_comparé = Mathf.Abs(Mathf.RoundToInt(total_path.x)) + Mathf.Abs(Mathf.RoundToInt(total_path.y));
			if (distance_comparé < distance_min) {
				distance_min = distance_comparé;
				hero = h;
			}
		}

		return hero;
	}

	//Détermine de quel côté l'ennemi va se placer
	private Vector3 Choice_side(GameObject hero){
		Vector3 end_position = hero.transform.position;																//Choisi le héros cible comme destination
		game_master.set_matrice_case (Mathf.RoundToInt (end_position.x), Mathf.RoundToInt (end_position.y), 0);		//Change TEMPORAIREMENT la case de l'ennemi en 0
		game_pathfinding.Find_Path (this.transform.position, end_position);											//Détermine le chemin avec le script de PathFinding
		List<Tile> path = game_pathfinding.Get_Path ();																//Récupère le chemin
		game_master.set_matrice_case (Mathf.RoundToInt (end_position.x), Mathf.RoundToInt (end_position.y), 1);		//Remet la case de l'ennemi en 1
		if (path != null) {																							//S'il existe un chemin
			end_position = new Vector3 (path [path.Count - 2].x, path [path.Count - 2].y, 0f);						//Récupère l'avant dernière case du chemin
			return end_position;
		} else {
			return Vector3.zero;
		}
	}


	//Fonction qui attaque l'ennemi a porté
	IEnumerator Attack(int int_distance,GameObject target){
		combatHUD_master.enable_disable_stats ();														//Affiche les informations de la cible
		while (action_point >= 1) {																		//Tant que l'ennemi a des points d'action, il attaque																		
			if (int_distance == 1 && action_point >= 3) {												//Choisi l'attaque en fonction de la distance et de ses points d'action
				action_point -= 3;																		//Réduit ses points d'action
				target.GetComponent<Hero_Master> ().DeductHealth (30);									//Blesse le héros ciblé
			} else if (int_distance <= 5 && action_point >= 1) {
				action_point -= 1;
				target.GetComponent<Hero_Master> ().DeductHealth (10);
			}
			yield return new WaitForSeconds (0.5f);														//Attend la fin des animations
		}
		combatHUD_master.enable_disable_stats ();														//Cache les informations de la cible
		game_master.passer_le_tour (); 
	}


	//Fonction qui détermine si l'ennemi à la range pour attaquer
	private bool Can_I_Attack(out int int_distance, out GameObject target){						
		target = Target_Choice_by_distance (out int_distance);											//Trouve le héros le plus proche et la distance qui les sépare

		if (int_distance <= max_range) {																//Retourne la distance qui les sépare ou 0
			return true;
		} else {
			return false;
		}
	}
}