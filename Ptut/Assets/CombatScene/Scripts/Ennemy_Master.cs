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
		game_master = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Master> ();
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();
		game_pathfinding = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Pathfinding> ();
	}

	//Active les stats de l'ennemi
	void OnMouseEnter()
	{
		combatHUD_master.Set_Ennemy_Health (this.gameObject);
		combatHUD_master.enable_disable_ennemy_stats ();
	}

	//Désactive les stats de l'ennemi
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
		if (current_health <= 0) {																//Si la vie du personnage atteint 0 ou inférieur
			current_health = 0;
			combatHUD_master.Change_Ennemy_Health (previous_health, current_health, health_stat);//Change la vie du personnage sur l'ATH
			game_master.Remove_From_List (this.gameObject.name);								//Supprime le personnage de la liste
			game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);	//Vide la case où il se tenait
			combatHUD_master.enable_disable_ennemy_stats ();									//Fait disparaître les stats du personnage
			Destroy (this.gameObject);															//Détruit le gameObject
		} else {
			combatHUD_master.Change_Ennemy_Health (previous_health, current_health, health_stat);//Sinon change du personnage sur l'ATH
		}
	}


	//Fonction qui remet les points au max (fin de tour)
	public void Reset_Point()
	{
		action_point = action_stat;
	}

	public void Its_me_mario_FlipFlap(){														//Pas envie de faire une flèche au dessus de la tête
		this.GetComponent<SpriteRenderer>().flipY = !this.GetComponent<SpriteRenderer>().flipY;
		StartCoroutine (FlipFlap ());
	}

	IEnumerator FlipFlap(){																		//Du trolling certes, mais du trolling travaillé
		yield return new WaitForSeconds (0.5f);
		this.GetComponent<SpriteRenderer>().flipY = !this.GetComponent<SpriteRenderer>().flipY;
	}

	//Fonction qui fait agir l'IA
	public void Comportement(){
		GameObject[] heros;
		GameObject hero_target;
		Vector3 end_position = Vector3.zero;
		bool action_done = false;
		int distance;
		int nb_try = 0;

		heros = Sort_Target_By_Distance ();																	//Récupère la liste des héros dans l'ordre de leur éloignement

		for (int i = 0; i < heros.Length && action_done == false; i++) {									//Tant qu'il y a des héros dans la liste et qu'aucune action n'a été effectuée
			hero_target = heros [nb_try];																	//Choisit sa cible
			distance = Get_Distance_From_Target (hero_target.transform.position);							//Détermine la distance qui le sépare de la case à côté de la cible
			if (distance > 1 && distance < 10000) {															//Si elle est éloignée
				end_position = Choice_side (hero_target);													//Trouve sa prochaine position
				if (end_position != Vector3.zero) {															//Si sa prochaine destination est accessible
					try_to_move (end_position);																//Se déplace
					action_done = true;
				}
			} else if (distance == 1) {																		//Si la cible est déjà au corps à corps
				Comportement_Suite ();
				action_done = true;
			}
			nb_try++;
		}
		if (action_done == false) {																			//Si aucun rapprochement n'est possible
			Comportement_Suite ();																			//Passe à la suite
		}
	}

	//Fonction qui fait agir l'IA après le déplacement
	private void Comportement_Suite(){
		int distance;
		GameObject target;
		if (Can_I_Attack (out distance, out target)) {														//Vérifie qu'il y a un ennemi a porté
			combatHUD_master.Set_Hero_Health(target.gameObject);											//Affiche la barre de vie de la cible
			StartCoroutine(Attack (distance, target));														//Attaque la cible
		} else {																							//Sinon, passe le tour
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
			Side_flip (next_position);																											//Fonction qui le fait pivoter dans sa direction

			while (this.transform.position != next_position) {																					//Tant que le héros n'est pas passer à la case suivante
				yield return new WaitForSeconds (waittime);																							
				this.transform.position = Vector3.MoveTowards (this.transform.position, next_position, step);									//Avance vers la case 
			}
		}																										 								// Booléen qui autorise un nouveau déplacement
		End_Deplacement();
	}

	//Fonction qui tourne le personnage en fonction de sa direction
	private void Side_flip(Vector3 next_position){
		if (this.transform.position.x < next_position.x) {																							//Si sa direction est à droite
			this.gameObject.GetComponent<SpriteRenderer> ().flipX = false;																			//Regarde à drotie
		} else if (this.transform.position.x > next_position.x) {																					//Sinon l'inverse
			this.gameObject.GetComponent<SpriteRenderer> ().flipX = true;
		}
	}

	//Fonction qui gère la  fin de déplacement
	private void End_Deplacement(){
		game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 1);			//Change la case de d'arrivé en 1 dans la matrice
		Comportement_Suite();
	}


	//Fonction qui détermine les cible dans l'ordre de distance
	public GameObject[] Sort_Target_By_Distance(){
		GameObject[] heros = GameObject.FindGameObjectsWithTag ("Hero");  													//Trouve la liste des héros
		GameObject hero_temp = null;																						//Cible de l'attaque
		int distance;
		int distance_ref;

		for (int i=0; i<heros.Length-1;i++){
			distance_ref = Get_Distance_From_Target (heros [i].transform.position);											//Choisit une distance de référence
			for(int j = i+1; j<heros.Length; j++) {																			//Parcours les autres héros																	
				distance = Get_Distance_From_Target (heros [j].transform.position);											//Définit le nombre de mouvement nécessaire pour s'y rendre
				if (distance < distance_ref) {																				//Si ce nombre de mouvement est inférieur au nombre de référence
					distance_ref = distance;																				//Definit la nouvelle référence
					hero_temp = heros [i];																					//Echange les places dans le tableau
					heros [i] = heros [j];
					heros [j] = hero_temp;
				}
			}
		}
		return heros;
	}

	//Fonction qui détermine le chemin à prendre pour se rendre sur la case de l'ennemi
	private List<Tile> Get_Path_From_Target(Vector3 target_position){
		game_master.set_matrice_case (Mathf.RoundToInt (target_position.x), Mathf.RoundToInt (target_position.y), 0);		//Change TEMPORAIREMENT la case de l'ennemi en 0
		game_pathfinding.Find_Path (this.transform.position, target_position);												//Détermine le chemin avec le script de PathFinding
		List<Tile> path = game_pathfinding.Get_Path ();																		//Récupère le chemin
		game_master.set_matrice_case (Mathf.RoundToInt (target_position.x), Mathf.RoundToInt (target_position.y), 1);		//Remet la case de l'ennemi en 1
		return path;
	}

	//Fonction qui retourne la distance qui sépare l'ennemi d'une cible
	private int Get_Distance_From_Target(Vector3 target_position){
		int distance;
		List<Tile> path = Get_Path_From_Target(target_position);															//Récupère le chemin
		if (path != null) {																									//Si le chemin n'est pas null
			distance = path.Count;																							//Retourne la distance entre lui et la cible
		} else {																											//Sinon, retourne 1000
			distance = 10000;
		}
		return distance;
	}

	//Détermine de quel côté l'ennemi va se placer par rapport à sa cible
	private Vector3 Choice_side(GameObject hero){
		Vector3 end_position = hero.transform.position;																		//Choisi le héros cible comme destination
		List<Tile> path = Get_Path_From_Target(end_position);																//Récupère le chemin pour se rendre sur la case de la cible
		end_position = new Vector3 (path [path.Count - 2].x, path [path.Count - 2].y, 0f);									//Récupère l'avant dernière case du chemin
		return end_position;
	}


	//Fonction qui attaque l'ennemi a porté
	IEnumerator Attack(int int_distance,GameObject target){
		bool attack_possibility = true;
		combatHUD_master.enable_disable_stats ();															//Affiche les informations de la cible
		while (action_point >= 1 && attack_possibility == true) {											//Tant que l'ennemi a des points d'action, il attaque																		
			if (int_distance == 1 && action_point >= 3) {													//Choisi l'attaque en fonction de la distance et de ses points d'action
				action_point -= 3;																			//Réduit ses points d'action
				target.GetComponent<Hero_Master> ().DeductHealth (30);										//Blesse le héros ciblé
			} else if (int_distance >= 2 && int_distance <= 5 && action_point >= 1) {
				action_point -= 1;
				target.GetComponent<Hero_Master> ().DeductHealth (6);
			} else {																						//Si il n'a la range pour aucune attaque
				attack_possibility = false;
			}
			yield return new WaitForSeconds (0.5f);															//Attend la fin des animations
		}
		combatHUD_master.enable_disable_stats ();															//Cache les informations de la cible
		game_master.passer_le_tour (); 
	}


	//Fonction qui détermine si l'ennemi à la range pour attaquer
	private bool Can_I_Attack(out int distance, out GameObject target){						
		GameObject[] targets = Sort_Target_By_Distance();													//Récupère la liste des héros dans l'ordre d'éloignement			
		target = targets[0];																				//Trouve le héros le plus proche
		distance = Get_Distance_From_Target(target.transform.position);										//Trouve la distance qui le sépare de sa cible
		if (distance <= max_range) {																		//Si la cible est à portée
			return true;																					//Retourne la distance qui les sépare et la cible
		} else {																							//Sinon, ne retourne rien
			return false;
		}
	}

	//Vérifie si le combat est terminé
	public void Update(){
		if (game_master.is_fight_over == true) {
			StopAllCoroutines();
		}
	}
}