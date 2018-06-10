using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

namespace MenuScene{
	public class MainMenu_Script : MonoBehaviour {

		//Fonction qui charge la scène de la map
			public void Continue_Game(){
			StartCoroutine (Load_Map_Scene_In_Background ());										//Charge la scène de la carte
		}

		IEnumerator Load_Map_Scene_In_Background()
		{
			AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapScene");

			// Wait until the asynchronous scene fully loads
			while (!asyncLoad.isDone)
			{
				yield return null;
			}
		}

		//Fonction qui supprime la dernière sauvegarde
		public void New_Game(){
			string path = Application.persistentDataPath + "/Saves" + "/MapSave.dat";
			if(File.Exists (path)){
				File.Delete (path);
			}
			StartCoroutine (Load_Map_Scene_In_Background ());										//Charge la scène de la carte
		}

		//Fonction qui charge la scène de la map
		public void Quit_Game(){
			Application.Quit();
		}
	}
}