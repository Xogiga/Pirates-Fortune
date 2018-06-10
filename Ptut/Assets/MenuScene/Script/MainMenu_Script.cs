using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace MenuScene{
	public class MainMenu_Script : MonoBehaviour {

		//Fonction qui charge la scène de la map
		public void Play_Games(){
			StartCoroutine (Load_Map_Scene_In_Background ());										//Charge l'évènement à partir de son nom
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

		//Fonction qui charge la scène de la map
		public void Quit_Game(){
			Application.Quit();
		}
	}
}