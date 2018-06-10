using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_Script : MonoBehaviour {
	public static bool Is_Game_Paused = false;
	public GameObject pause_menu;

	void Update(){
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (Is_Game_Paused == true) {
				Resume ();
			} else {
				Pause ();
			}
		}
	}

	public void Resume(){
		pause_menu.SetActive (false);
		Time.timeScale = 1f;
		Is_Game_Paused = false;

	}

	void Pause(){
		pause_menu.SetActive (true);
		Time.timeScale = 0f;
		Is_Game_Paused = true;
	}

	public void Load_Menu(){
		Time.timeScale = 1f;
		StartCoroutine(Load_Menu_Scene("MenuScene"));
	}

	IEnumerator Load_Menu_Scene(string scene_name)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene_name);

		// Wait until the asynchronous scene fully loads
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
	}

	public void Quit_Game(){
		Application.Quit ();
	}
}
