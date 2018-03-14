using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero_Master : MonoBehaviour {
	public bool is_moving;
	private int stats_de_deplacement;
	private int movement_point;
	private int stats_daction;
	private int action_point;
	private int health_stat = 100;
	private int current_health;
	private CombatHUD_Master combatHUD_master;

	void OnEnable()
	{
		SetInitialReferences();
	}

	void SetInitialReferences()
	{
		current_health = health_stat;
		is_moving = false;
		stats_de_deplacement = 4;
		stats_daction = 5;
		action_point = stats_daction;
		movement_point = stats_de_deplacement;
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();
	}

	public void Its_me_mario_FlipFlap(){														//Pas envie de faire une flèche au dessus de la tête
		this.GetComponent<SpriteRenderer>().flipY = !this.GetComponent<SpriteRenderer>().flipY;
		StartCoroutine (FlipFlap ());
	}

	IEnumerator FlipFlap(){																		//Du trolling certes, mais du trolling travaillé
		yield return new WaitForSeconds (0.5f);
		this.GetComponent<SpriteRenderer>().flipY = !this.GetComponent<SpriteRenderer>().flipY;
	}

	public void Reset_Point()
	{
		movement_point = stats_de_deplacement;
		action_point = stats_daction;
	}

	void IncreaseHealth(int health_change)
	{
		current_health += health_change;
		if (current_health > health_stat) {
			current_health = health_stat;
		}
		combatHUD_master.Set_Hero_Health(this.gameObject);
	}

	void DeductHealth(int health_change){
		current_health -= health_change;
		if (current_health < 0) {
			current_health = 0;
		}
		combatHUD_master.Set_Hero_Health(this.gameObject);
	}

	public int Get_Movement_Point(){
		return movement_point;
	}

	public void Set_Movement_Point(int new_movement_point){
		movement_point = new_movement_point;
		combatHUD_master.Set_Hero_Movement_Point (this.gameObject);										//Affiche la réduction de point de mouvement
	}

	public int Get_Action_Point(){
		return action_point;
	}

	public int Get_Health_Stats(){
		return health_stat;
	}

	public int Get_Current_Health(){
		return current_health;
	}

	public void Set_Action_Point(int val){
		action_point = val;
		combatHUD_master.Set_Hero_Action_Point (this.gameObject);						//Affiche la réduction de point d'action
	}
}
