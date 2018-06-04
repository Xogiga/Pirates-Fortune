using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene{
	public class References : MonoBehaviour {
		public static GameManager_Master GameMaster;
		public static MapSave SaveMapScript;
		public static Dialog_Manager DialogueManager;
		public static GameManager_Create_Map CreationScript;
		public static GameManager_Commands Commands;

		void Awake(){
			GameObject game_manager = GameObject.FindWithTag ("GameManager");
			GameMaster = game_manager.GetComponent<GameManager_Master> ();
			SaveMapScript  = game_manager.GetComponent<MapSave> ();
			CreationScript = game_manager.GetComponent<GameManager_Create_Map> ();
			Commands = game_manager.GetComponent<GameManager_Commands> ();
			DialogueManager = game_manager.GetComponent<Dialog_Manager> ();
		}
	}
}
