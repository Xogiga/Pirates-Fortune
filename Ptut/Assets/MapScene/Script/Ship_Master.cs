using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	public class Ship_Master : MonoBehaviour {
		private bool is_moving;

		void OnEnable(){
			Set_initial_references ();
		}

		void Set_initial_references()
		{		
			is_moving = false;
		}

		//Fonction qui vérifie que le bateau n'est pas déjà en mouvement, avant de le déplacer
		public void Can_I_Move(GameObject destination){
			if (is_moving == false) {
				is_moving = true;
				StartCoroutine (Move(destination));
			}
		}

		//Fonction qui déplace le bateau
		IEnumerator Move(GameObject destination)
		{
			float waittime = 0.04f; 																													//Temps entre chaque micro-déplacement de MoveToward
			float step = 4*waittime; 																													//Vitesse*Temps = distance de MoveTowards
			Vector3 end_position = destination.transform.position;

			Side_flip (end_position);																													//Pivote le  bateau en fonction de sa direction

			while (this.transform.position != end_position) {																							//Tant que le bateau n'est pas arriver à destination
				yield return new WaitForSeconds (waittime);																							
				this.transform.position = Vector3.MoveTowards (this.transform.position, end_position, step);											//Avance vers sa destination
			}

			GameManager_Master.GameMaster.Set_Reachable_point (destination);																								//Autorise le déplacement vers les points suivants
			is_moving = false;																															//Booléen qui autorise un nouveau déplacement
			GameManager_Master.GameMaster.Load_Event_Scene(destination);
		}


		//Fonction qui tourne le bateau en fonction de sa direction
		private void Side_flip(Vector3 end_position){
			SpriteRenderer sprite = this.GetComponent<SpriteRenderer>();
			if (this.transform.position.x < end_position.x) {																							//Si sa direction est à droite
				sprite.flipX  = false;																													//Regarde à drotie
			} else if (this.transform.position.x > end_position.x) {																					//Sinon l'inverse
				sprite.flipX  = true;	
			}
		}
	}
}