using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		private List<GameObject> reachable_points;
		private GameObject current_point;

		void OnEnable(){
			Begin ();
		}

		public void Set_current_point(GameObject point){
			current_point = point;
		}

		//Fonction qui déclenche la création de la map
		private void Begin(){
			if (References.CreationScript != null && References.Commands != null) {
				References.CreationScript.Create_Map ();
				References.Commands.enabled = true;
			}
		}

		//Fonction qui assure la suite des évènements à l'arrivée du joueur sur un point
		public void Interact_With_Interest_Point(GameObject destination){
			current_point = destination;
			interest_marker_script point_script = destination.GetComponent<interest_marker_script> ();
			Set_Reachable_point();																		//Détermines les prochaines iles accessibles
			if (point_script.done == false) {															//Si l'évènement n'a pas déjà était fait
				References.DialogueManager.Start_Dialogue (destination);								//Démarre le dialogue
			}
			References.SaveMapScript.playerPos = current_point;											//Enregistre la position du joueur
		}

		//Fonction qui détermine les points accessibles
		public void Set_Reachable_point(){
			interest_marker_script point_script = current_point.GetComponent<interest_marker_script> ();//Récupère le script qui contient les données du point

			if (reachable_points != null) {																//Si la liste existe déjà
				foreach (GameObject N in reachable_points) {											//Rend tous ces points inaccessible
					N.GetComponent<interest_marker_script>().reachable = false;
				}
			}

			reachable_points = point_script.local_list_point;											//Récupère la liste de voisin

			foreach (GameObject N in reachable_points) {												//Rend les voisins accessible
				N.GetComponent<interest_marker_script>().reachable = true;
			}
		}

		//Fonction qui gère l'évènement après l'affichage du dialogue
		public void Execute_Event(){
			interest_marker_script point_script = current_point.GetComponent<interest_marker_script> ();	//Récupère le script qui contient les données du point
			if (point_script.done == false) {																//Si le point n'a pas déjà été parcouru
				point_script.done = true;																	//Détermine l'évènement comme fait
				References.CreationScript.Add_Visited_Points(current_point);								//Grise le dernier point visité
				References.SaveMapScript.Save ();															//Sauvegarde les informations de la map dans un fichier, avant de changer de scène
				if(point_script.Event.Involve_Fight){														//Si le point contient une scène à charger
					StartCoroutine (Load_Next_Fight_In_Background ());										//Charge l'évènement à partir de son nom
				}
			}
		}


		IEnumerator Load_Next_Fight_In_Background()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CombatScene");

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}
	}
}
