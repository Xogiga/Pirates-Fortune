using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deplacement : MonoBehaviour {
	public Text stats;
	public bool is_moving;
	public float statsDeDeplacement;
	public float pointDeDeplacement;

	void OnEnable(){
		Set_initial_references ();
		setUI ();
	}

	void Set_initial_references()
	{
		pointDeDeplacement = statsDeDeplacement;
		is_moving = false;
	}

	public void justmove (Vector3 endposition)
	{
			Vector3 endpos = endposition;
			StartCoroutine (Move (endpos));
	}

	public void setUI(){
		stats.text = "Point de Deplacement = "+pointDeDeplacement.ToString();
	}

	IEnumerator Move(Vector3 endposition)
	{
		is_moving = true;   // Booléen qui empêche d'engager un nouveau déplacement, de rappeller la fonction, avant que le précédent soit fini

		Vector3 newpos; // Arrivée du déplacement de 1 case
		Vector3 parcours = endposition - this.transform.position; //Récupère la distance entre le joueur et la case
		float xparcours = Mathf.Round(parcours.x);					//Arrondit xparcours car récupérer le X Y Z directement d'un vecteur bug souvent
		float yparcours = Mathf.Round(parcours.y);
		
		float waittime = 0.04f; //Temps entre chaque micro-déplacement de MoveToward
		float step = 4*waittime; //Vitesse*Temps = distance de MoveTowards

		if ((Mathf.Abs (xparcours) + Mathf.Abs (yparcours)) > pointDeDeplacement) {  //Vérifie que le jouer a assez de point de déplacement
			goto Fin;
		} else {
			pointDeDeplacement -= (Mathf.Abs (xparcours) + Mathf.Abs (yparcours));
			setUI ();
		}

		if (Mathf.Abs (xparcours) == Mathf.Abs (yparcours)) {              					 //Tout les deplacements en diagonale
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

		if (xparcours > 0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          //Tous les mouvements à droite sauf ligne droite et diagonale
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


		if (xparcours <0 && yparcours != 0 && Mathf.Abs (xparcours) != Mathf.Abs (yparcours)) {          //tous les mouvements à gauche sauf ligne droite et diagonale
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
		Fin:
		is_moving = false;				 // Booléen qui autorise un nouveau déplacement
}

}


