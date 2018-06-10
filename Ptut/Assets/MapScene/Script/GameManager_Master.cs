using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		private List<GameObject> reachable_points;
		private GameObject current_point;
		public GameObject data_transporter;


		void OnEnable(){
			Begin ();
		}

			
		//Fonction qui déclenche la création de la map
		private void Begin(){
			References.CreationScript.Create_Map ();
			References.Commands.enabled = true;
		}

		//Fonction qui assure la suite des évènements à l'arrivée du joueur sur un point
		public void Interact_With_Interest_Point(GameObject destination){
			interest_marker_script point_script = destination.GetComponent<interest_marker_script> ();
			Set_Current_Point (destination);

			if (point_script.Event.Involve_Fight == false) {												//Si l'évènement n'inclut pas un combat
				if (point_script.done == false) {															//S'il n'est pas fait
					References.DialogueManager.Start_Dialogue (destination);								//Démarre le dialogue
					point_script.done = true;																//Le considère comme "fait" après le dialogue
				}

			} else { 																						//Si l'évènement inclus un combat
				if (DataTransporter_Script.data != null && DataTransporter_Script.data.Get_Victory ()) {	//S'il le combat vient d'être remporté
					if (point_script.marine == false) {														//Si ce n'est pas la marine
						point_script.done = true;															//Détermine l'évènement comme fait
						References.CreationScript.Add_Visited_Points (current_point);						//Grise le dernier point visité
					}	
					//Et pour tous les combats
					Destroy (DataTransporter_Script.data.gameObject);										//Détruit l'objet qui contient les données du précédent combat
					Send_To_Save_File ();																	//Sauvegarde les informations de la map dans un fichier

				} else {																					//Si le combat ne vient pas d'être remporté
					if (point_script.done == false) {														//S'il n'est pas déjà fait
						References.DialogueManager.Start_Dialogue (destination);							//Démarre le dialogue
					}
				}
			}
		}

		//Définit le point actuel et ceux atteignables
		private void Set_Current_Point(GameObject destination){
			current_point = destination;
			Set_Reachable_point();																		//Détermines les prochaines iles accessibles
		}

		//Fonction qui détermine les points accessibles
		public void Set_Reachable_point(){
			interest_marker_script point_script = current_point.GetComponent<interest_marker_script> ();//Récupère le script qui contient les données du point

			if (point_script.Event.EventName == "End") {												//Gère la fin
				Manage_End ();
			}

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

		//Fonction qui s'occupe de la fin
		private void Manage_End(){
			string path = Application.persistentDataPath + "/Saves" + "/MapSave.dat";
			if(File.Exists (path)){
				File.Delete (path);
			}
			StartCoroutine (Load_Next_Fight_In_Background ("MenuScene"));
		}

		//Fonction qui gère l'évènement après l'affichage du dialogue
		public void Execute_Event(){
			interest_marker_script point_script = current_point.GetComponent<interest_marker_script> ();//Récupère le script qui contient les données du point
			if (point_script.marine == false) {															//Si l'évènement n'est pas celui de la Marine
				References.CreationScript.Add_Visited_Points(current_point);							//Grise le dernier point visité
			}

			Send_To_Save_File();																		//Sauvegarde les informations de la map dans un fichier, avant de changer de scène
			if (point_script.Event.Involve_Fight) {														//Si le point contient une scène à charger
				if (point_script.Event.fp != null) {													//S'il a des paramètres de combat différents de ceux par défaut
					GameObject dt = Instantiate (data_transporter);										//Crée un objet qui transporte les paramètres du fight
					dt.GetComponent<DataTransporter_Script> ().Set_Fight_Parameters (point_script.Event.fp);//Transmet les paramètres de fight de l'event au transporteur
				}
				StartCoroutine (Load_Next_Fight_In_Background ("CombatScene"));							//Charge l'évènement à partir de son nom
			} 
		}

		private void Send_To_Save_File(){
			References.SaveMapScript.playerPos = current_point;											//Enregistre la position du joueur
			References.SaveMapScript.Save ();															//Sauvegarde;
		}


		IEnumerator Load_Next_Fight_In_Background(string scene_name)
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name);

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}
			
	}
}
