using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Attaque : MonoBehaviour {
	private int  pointDeDeplacementEnnemi = 3;
	private int moveDone = 0;
	private GameObject game_manager;
	private GameManger_BeginEnnemyTurn ennemy_turn_script;


	void OnEnable(){
		game_manager = GameObject.FindGameObjectWithTag ("GameManager");
		ennemy_turn_script = game_manager.GetComponent<GameManger_BeginEnnemyTurn>();
	}

	public void Comportement () {
		StartCoroutine (Deplacement ());
	}		

	private Vector3 possibleMovement(){
		GameObject player = GameObject.FindWithTag ("Player");  							// trouve la position du Joueur;
		Vector3 totalPath = player.transform.position - this.transform.position;
		Vector3 endPosition = Vector3.zero;
		float xtotalPath = Mathf.Round (totalPath.x);
		float ytotalPath = Mathf.Round (totalPath.y);
			if (xtotalPath != 0) {
				if (xtotalPath > 0) {											// détermine de quel coté du joueur l'ennemi doit aller
				endPosition = player.transform.position - Vector3.right;			//si l'ennemi doit aller à droite on enlève un mouvement à droite
				} else {
					endPosition = player.transform.position - Vector3.left;
				}
			} else {
				if (ytotalPath > 0) {											
					endPosition = player.transform.position - Vector3.up;
				} else {
					endPosition = player.transform.position - Vector3.down;
				}
			}
			return endPosition;
	}

		IEnumerator Deplacement()
	{
		float waittime = 0.04f; //Temps entre chaque micro-déplacement de MoveToward
		float step = 4*waittime; //Vitesse*Temps = distance de MoveTowards				

		Vector3 endposition = possibleMovement();

		Vector3 parcours =  endposition - this.transform.position;
		float xparcours = Mathf.Round(parcours.x);
		float yparcours = Mathf.Round(parcours.y);

		Vector3 newpos; // Arrivée du déplacement de 1 case

		moveDone = 0;

			if (Mathf.Abs (xparcours) == Mathf.Abs (yparcours)) {              					 //Tout les deplacements en diagonale
			while (moveDone < pointDeDeplacementEnnemi && this.transform.position != endposition) {

					if (xparcours > 0) {
						newpos = this.transform.position + Vector3.right;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
						moveDone++;
						if (yparcours > 0) {
							newpos = this.transform.position + Vector3.up;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
							moveDone++;
						} else if (yparcours < 0) {
							newpos = this.transform.position + Vector3.down;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);	
							}
						moveDone++;
						}
					} else if (xparcours < 0) {

						newpos = this.transform.position + Vector3.left;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					moveDone++;
						if (yparcours > 0) {
							newpos = this.transform.position + Vector3.up;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						} else if (yparcours < 0) {
							newpos = this.transform.position + Vector3.down;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						}
					}

				}
			}


			if (xparcours == 0 || yparcours == 0) {                     					// Toutes les lignes droites
			while (moveDone < pointDeDeplacementEnnemi && this.transform.position != endposition) { 
					if (xparcours != 0) {
						if (xparcours > 0) {
							newpos = this.transform.position + Vector3.right;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						} else {
							newpos = this.transform.position + Vector3.left;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);	
							}
						moveDone++;
						}
					}
					if (yparcours != 0) {
						if (yparcours > 0) {
							newpos = this.transform.position + Vector3.up;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						} else {
							newpos = this.transform.position + Vector3.down;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						}
					}
				}
			}

			if (xparcours > 0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          //Tous les mouvements à droite sauf ligne droite et diagonale
			while (moveDone < pointDeDeplacementEnnemi && this.transform.position != endposition) {
				for (int i = 0; i < xparcours && moveDone < pointDeDeplacementEnnemi; i++) {
						newpos = this.transform.position + Vector3.right;
						while (this.transform.position != newpos) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					moveDone++;
					}

				for (int i = 0; i < Mathf.Abs (yparcours) && moveDone < pointDeDeplacementEnnemi; i++) {
						if (yparcours > 0) {
							newpos = this.transform.position + Vector3.up;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						}
						if (yparcours < 0) {

							newpos = this.transform.position + Vector3.down;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						}
					}
				}
			}


			if (xparcours <0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          //tous les mouvements à gauche sauf ligne droite et diagonale
			while (moveDone < pointDeDeplacementEnnemi && this.transform.position != endposition) {
				for (int i = 0; i < Mathf.Abs(xparcours) && moveDone < pointDeDeplacementEnnemi; i++) {
						newpos = this.transform.position + Vector3.left;
					while (this.transform.position != newpos && moveDone < pointDeDeplacementEnnemi) {
							yield return new WaitForSeconds (waittime);
							this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
						}
					moveDone++;
					}

				for (int i = 0; i < Mathf.Abs(yparcours) && moveDone < pointDeDeplacementEnnemi; i++) {
						if (yparcours > 0) {
							newpos = this.transform.position + Vector3.up;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						}
						if (yparcours < 0) {
							newpos = this.transform.position + Vector3.down;
							while (this.transform.position != newpos) {
								yield return new WaitForSeconds (waittime);
								this.transform.position = Vector3.MoveTowards (this.transform.position, newpos, step);
							}
						moveDone++;
						}
					}
				}
			}
		moveDone = 0;
		ennemy_turn_script.End_ennemy_turn ();
		}
	}

