using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero_Deplacement : MonoBehaviour {
	private Hero_Master hero_master;
	private GameManager_Master game_master;

	void OnEnable(){
		Set_initial_references ();
	}

	void Set_initial_references()
	{		
		game_master = GameObject.FindGameObjectWithTag ("GameManager").GetComponent<GameManager_Master> ();
		hero_master = this.GetComponent<Hero_Master> ();
		hero_master.is_moving = false;

	}

	public void try_to_move (Vector3 endposition)
	{
		if (verify_distance (endposition)) {																										//Si le héros a assez de point de déplacement
			game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);			//Change la case de départ en 0 dans la matrice
			game_master.set_matrice_case (Mathf.RoundToInt (endposition.x), Mathf.RoundToInt (endposition.y), 1);									//Change la case de destination en 1 dans la matrice
			StartCoroutine (Move (endposition));																									//Déplace le héros
		}
	}

	//Fonction qui vérifie que le joueur a assez de point de déplacement
	private bool verify_distance(Vector3 endposition){
		Vector3 distance = endposition - this.transform.position; 																					//Récupère la distance entre le joueur et la case
		int xparcours = Mathf.RoundToInt(distance.x);																								//Arrondit xparcours car récupérer le X Y Z directement d'un vecteur bug souvent
		int yparcours = Mathf.RoundToInt(distance.y);

		if ((Mathf.Abs (xparcours) + Mathf.Abs (yparcours)) > hero_master.Get_Movement_Point()) { 													//Vérifie que le joueurr a assez de point de déplacement
			return false;
		} else {
			hero_master.Set_Movement_Point(hero_master.Get_Movement_Point() - Mathf.Abs (xparcours) - Mathf.Abs (yparcours));						//Réduit les points de déplacement du héros
			return true;
		}
	}



	IEnumerator Move(Vector3 endposition)
	{
		hero_master.is_moving = true;   																											// Booléen qui empêche d'engager un nouveau déplacement, de rappeller la fonction, avant que le précédent soit fini

		Vector3 newpos; 																															// Arrivée du déplacement de 1 case
		Vector3 parcours = endposition - this.transform.position; 																					//Récupère la distance entre le joueur et la case
		float xparcours = Mathf.Round(parcours.x);																									//Arrondit xparcours car récupérer le X Y Z directement d'un vecteur bug souvent
		float yparcours = Mathf.Round(parcours.y);

		float waittime = 0.04f; 																													//Temps entre chaque micro-déplacement de MoveToward
		float step = 4*waittime; 																													//Vitesse*Temps = distance de MoveTowards

		if (Mathf.Abs (xparcours) == Mathf.Abs (yparcours)) {              					 														//Tous les deplacements en diagonale
			while (this.transform.position != endposition) {

				if (xparcours > 0) {
					newpos = this.transform.position + Vector3.right;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					} else if (yparcours < 0) {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					}
				} else if (xparcours < 0) {

					newpos = this.transform.position + Vector3.left;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					} else if (yparcours < 0) {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					}
				}

			}
		}


		if (xparcours == 0 || yparcours == 0) {                     					// Toutes les lignes droites
			while (this.transform.position != endposition) { 
				if (xparcours != 0) {
					if (xparcours > 0) {
						newpos = this.transform.position + Vector3.right;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					} else {
						newpos = this.transform.position + Vector3.left;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					}
				}
				if (yparcours != 0) {
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					} else {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					}
				}
			}
		}

		if (xparcours > 0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          									//Tous les mouvements à droite sauf ligne droite et diagonale
			while (this.transform.position != endposition) {
				for (int i = 0; i < xparcours; i++) {
					newpos = this.transform.position + Vector3.right;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
				}

				for (int i = 0; i < Mathf.Abs (yparcours); i++) {
					if (yparcours > 0) {

						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}

					}
					if (yparcours < 0) {

						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}

					}
				}
			}
		}


		if (xparcours <0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          										//Tous les mouvements à gauche sauf ligne droite et diagonale
			while (this.transform.position != endposition) {
				for (int i = 0; i < Mathf.Abs(xparcours); i++) {
					newpos = this.transform.position + Vector3.left;
					while (this.transform.position != newpos) {
						yield return new WaitForSeconds (waittime);
						this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
					}
				}

				for (int i = 0; i < Mathf.Abs(yparcours); i++) {
					if (yparcours > 0) {
						newpos = this.transform.position + Vector3.up;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}

					}
					if (yparcours < 0) {
						newpos = this.transform.position + Vector3.down;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					}
				}
			}
		}
		hero_master.is_moving = false;																										 // Booléen qui autorise un nouveau déplacement
	}

}


