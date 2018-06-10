using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatScene{
	public class References : MonoBehaviour {
		public static GameManager_Master GameMaster;
		public static GameManager_Commands Commands;
		public static CombatHUD_Master CombatHud;
		public static GameManager_BeginFight CreationScript;
		public static GameManager_Pathfinding Pathfinding;
		public static CombatLog_Manager CombatLog;

		void Awake(){
			GameObject game_manager = GameObject.FindWithTag ("GameManager");
			GameMaster = game_manager.GetComponent<GameManager_Master> ();
			Commands = game_manager.GetComponent<GameManager_Commands> ();
			CreationScript = game_manager.GetComponent<GameManager_BeginFight>();
			Pathfinding = game_manager.GetComponent<GameManager_Pathfinding> ();
			GameObject CH = GameObject.Find ("CombatHUD");
			CombatHud = CH.GetComponent<CombatHUD_Master>();
			CombatLog = CH.transform.GetChild (6).GetComponent<CombatLog_Manager> ();
		}
	}
}
