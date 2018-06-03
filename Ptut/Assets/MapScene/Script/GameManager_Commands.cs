using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapScene {
	public class GameManager_Commands : MonoBehaviour {
		private Ship_Master player_script;
		public static GameManager_Commands Commands;

		void Awake(){
			Commands = this;
		}

		void OnEnable(){
			Set_initial_references ();
		}

		//Reférences fixes
		private void Set_initial_references () {																		
			player_script = GameObject.FindWithTag ("Hero").GetComponent<Ship_Master> ();
		}

		// Update is called once per frame
		void Update () {
			
			if (Input.GetMouseButtonDown (0)) {														//Gère les déplacements à la souris
				if (EventSystem.current.IsPointerOverGameObject ()) {								//Si le pointeur est au dessus d'un élément de l'ATH, sort de la fonction.
					return;
				}
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);						//Crée un rayon
				RaycastHit hit;																		//Permet de récupérer la hitbox touchée
				if (Physics.Raycast (ray, out hit)) {												//Return True si le Rayon touche une hitbox à la position de la souris
					if (hit.transform.tag == "Point" 												//Vérifie que l'objet touché fait partie de la map
						&& hit.transform.GetComponent<interest_marker_script>().reachable == true) 	//Et que c'est un point atteignable
					{												
						player_script.Can_I_Move (hit.transform.gameObject);						//Se déplace jusqu'au point sélectionée
					}
				}
			}
			if (Input.GetKey ("escape")) 															//Ferme l'application
			{
				Application.Quit ();
			}
		}
	}
}
