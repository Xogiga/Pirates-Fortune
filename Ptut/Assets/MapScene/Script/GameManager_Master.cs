using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		private List<GameObject> reachable_points;

		void OnEnable(){
			Set_Initial_References ();
		}

		private void Set_Initial_References(){
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
			string event_name = point.GetComponent<interest_marker_script> ().event_name;
			StartCoroutine(Load_Next_Scene_In_Background(event_name));
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
	
