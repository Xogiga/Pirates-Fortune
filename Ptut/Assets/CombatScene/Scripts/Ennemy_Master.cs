using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemy_Master : MonoBehaviour {

	private GameManager_Master game_master;
	private int ennemy_movement_point;
	private int move_done;
	private int max_range;
	private int health_stat;
	private int current_health;
	private int action_stat;
	private int action_point;
	private CombatHUD_Master combatHUD_master;

	void OnEnable()
	{
		Set_initial_reference();
	}

	private void Set_initial_reference(){
		health_stat = 100;
		current_health = health_stat;
		ennemy_movement_point = 3;
		move_done = 0;
		max_range = 5;
		action_stat = 5;
		action_point = action_stat;
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();
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
		GameObject hero_target = Target_Choice_by_distance ();											//Trouve la cible
		Vector3 end_position = Choice_side (hero_target);												//Choisi sa destination
		game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);			//Change la case de de départ en 0 dans la matrice
		StartCoroutine (Deplacement (end_position));													//Se déplace
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

	//Fonction qui attaque l'ennemi a porté
	IEnumerator Attack(int int_distance,GameObject target){
		combatHUD_master.enable_disable_stats ();														//Affiche les informations de la cible
		while (action_point >= 1) {																		//Tant que l'ennemi a des points d'action, il attaque																		
			if (int_distance == 1 && action_point >= 3) {												//Choisi l'attaque en fonction de la distance et de ses points d'action
				action_point -= 3;																		//Réduit ses points d'action
				target.GetComponent<Hero_Master> ().DeductHealth (50);									//Blesse le héros ciblé
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
		target = Target_Choice_by_distance ();															//Trouve le héros le plus proche

		Vector3 distance = target.transform.position - this.transform.position;							//Détermine la distance qui le sépare de sa cible en case
		int_distance = Mathf.Abs(Mathf.RoundToInt(distance.x)) +
			Mathf.Abs(Mathf.RoundToInt(distance.y));

		if (int_distance <= max_range) {																//Retourne la distance qui les sépare ou 0
			return true;
		} else {
			return false;
		}
	}

	//Fonction qui détermine la cible de l'attaque
	private GameObject Target_Choice_by_distance(){
		GameObject[] heros = GameObject.FindGameObjectsWithTag ("Hero");  								//Trouve la liste des héros
		GameObject hero = null;																			//Cible de l'attaque
		Vector3 total_path;
		int distance_min = 1000;
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
		Vector3 total_path = hero.transform.position - this.transform.position;		//Vecteur de distance entre l'ennemi et le héros
		Vector3 end_position;

		float xtotal_path = Mathf.Round (total_path.x);
		float ytotal_path = Mathf.Round (total_path.y);


		if (xtotal_path != 0) {
			if (xtotal_path > 0) {													// détermine de quel coté du joueur l'ennemi doit aller
				end_position = hero.transform.position - Vector3.right;				//si l'ennemi doit aller à droite on enlève un mouvement à droite
			} else {
				end_position = hero.transform.position - Vector3.left;
			}
		} else {
			if (ytotal_path > 0) {											
				end_position = hero.transform.position - Vector3.up;
			} else {
				end_position = hero.transform.position - Vector3.down;
			}
		}
		return end_position;
	}


	//Fonction qui gère la  fin de déplacement
	private void End_Deplacement(){
		move_done = 0;
		game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 1);			//Change la case de d'arrivé en 1 dans la matrice
		Comportement_Suite();
	}

	//Fonction qui déplace l'ennemi
	IEnumerator Deplacement(Vector3 endposition)
	{
		float waittime = 0.04f; //Temps entre chaque micro-déplacement de MoveToward
		float step = 4*waittime; //Vitesse*Temps = distance de MoveTowards				

		Vector3 parcours =  endposition - this.transform.position;
		float xparcours = Mathf.Round(parcours.x);
		float yparcours = Mathf.Round(parcours.y);

		Vector3 newpos; // Arrivée du déplacement de 1 case

		move_done = 0;

		if (Mathf.Abs (xparcours) == Mathf.Abs (yparcours)) {              					 //Tout les deplacements en diagonale
			while (move_done < ennemy_movement_point && this.transform.position != endposition) {

				if (xparcours > 0) {
					newpos = this.transform.position + Vector3.right;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
					move_done++;
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					} else if (yparcours < 0) {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);	
						}
						move_done++;
					}
				} else if (xparcours < 0) {

					newpos = this.transform.position + Vector3.left;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
					move_done++;
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					} else if (yparcours < 0) {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					}
				}

			}
		}


		if (xparcours == 0 || yparcours == 0) {                     					// Toutes les lignes droites
			while (move_done < ennemy_movement_point && this.transform.position != endposition) { 
				if (xparcours != 0) {
					if (xparcours > 0) {
						newpos = this.transform.position + Vector3.right;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					} else {
						newpos = this.transform.position + Vector3.left;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);	
						}
						move_done++;
					}
				}
				if (yparcours != 0) {
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					} else {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					}
				}
			}
		}

		if (xparcours > 0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          //Tous les mouvements à droite sauf ligne droite et diagonale
			while (move_done < ennemy_movement_point && this.transform.position != endposition) {
				for (int i = 0; i < xparcours && move_done < ennemy_movement_point; i++) {
					newpos = this.transform.position + Vector3.right;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
					move_done++;
				}

				for (int i = 0; i < Mathf.Abs (yparcours) && move_done < ennemy_movement_point; i++) {
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					}
					if (yparcours < 0) {

						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					}
				}
			}
		}


		if (xparcours <0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          //tous les mouvements à gauche sauf ligne droite et diagonale
			while (move_done < ennemy_movement_point && this.transform.position != endposition) {
				for (int i = 0; i < Mathf.Abs(xparcours) && move_done < ennemy_movement_point; i++) {
					newpos = this.transform.position + Vector3.left;
					while (this.transform.position != newpos && move_done < ennemy_movement_point) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
					move_done++;
				}

				for (int i = 0; i < Mathf.Abs(yparcours) && move_done < ennemy_movement_point; i++) {
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					}
					if (yparcours < 0) {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						move_done++;
					}
				}
			}
		}
		End_Deplacement();
	}
}
