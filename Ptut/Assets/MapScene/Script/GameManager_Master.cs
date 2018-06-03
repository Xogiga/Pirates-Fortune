using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		private List<GameObject> reachable_points;
		public static GameManager_Master GameMaster;

		void OnEnable(){
			Set_Initial_References ();
			Begin ();
		}

		private void Set_Initial_References(){
			GameMaster = this;
		}

		//Fonction qui déclenche la création de la map
		private void Begin(){
			if (GameManager_Create_Map.creation_script != null && GameManager_Commands.Commands != null) {
				GameManager_Create_Map.creation_script.Create_Map ();
				GameManager_Commands.Commands.enabled = true;
				Set_Reachable_point (MapSave.CurrentMap.startPoint);										//Détermine les points accessibles
			}
		}


		//Fonction qui détermine les points accessibles
		public void Set_Reachable_point(GameObject point){
			if (reachable_points != null) {
				foreach (GameObject N in reachable_points) {
					N.GetComponent<interest_marker_script> ().reachable = false;
				}
			}

			reachable_points = point.GetComponent<interest_marker_script> ().local_list_point;

			foreach (GameObject N in reachable_points) {
				N.GetComponent<interest_marker_script> ().reachable = true;
			}
		}

		//Fonction qui charge la scene de l'evenement
		public void Load_Event_Scene(GameObject point){
			interest_marker_script point_script = point.GetComponent<interest_marker_script> ();			//Récupère le scrit qui contient les données du point
			if (point_script.done == false) {																//Si le point n'a pas déjà été parcouru
				MapSave.CurrentMap.playerPos = GameObject.FindWithTag ("Hero").transform.position;			//Enregistre la position du joueur
				point.GetComponent<interest_marker_script> ().done = true;									//Détermine l'évènement comme fait
				MapSave.CurrentMap.Save ();																	//Sauvegarde les informations de la map dans un fichier, avant de changer de scène
				StartCoroutine (Load_Next_Scene_In_Background (point_script.event_name));					//Charge l'évènement à partir de son nom
			} else {
				print ("Already done");
			}
		}


		IEnumerator Load_Next_Scene_In_Background(string name)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}
	}
}
	
