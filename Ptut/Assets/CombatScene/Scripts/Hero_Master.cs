using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class Hero_Master : MonoBehaviour {
	public bool is_moving;
	private int stats_de_deplacement;
	[Range(0,100)] public int movement_point;
	private int stats_daction;
	[Range(0,100)] public int action_point;
	private int health_stat;
	[Range(0,100)] public int current_health;
	private GameObject indicator;
	private CombatHUD_Master combatHUD_master;
	private GameManager_Master game_master;
	public GameObject damage_text;
	private CombatLog_Manager combatlog_master;

	void OnEnable()
	{
		SetInitialReferences();
	}

	void SetInitialReferences()
	{
		health_stat = 100;
		current_health = health_stat;
		is_moving = false;
		stats_de_deplacement = 4;
		stats_daction = 5;
		action_point = stats_daction;
		movement_point = stats_de_deplacement;
		game_master = GameObject.FindWithTag ("GameManager").GetComponent<GameManager_Master> ();
		indicator = this.transform.GetChild (0).gameObject;										//Recupère un gameObject fils
		GameObject combatHUD = GameObject.Find ("CombatHUD");
		combatHUD_master = combatHUD.GetComponent<CombatHUD_Master>();
		combatlog_master = combatHUD.transform.GetChild(6).GetComponent<CombatLog_Manager>();
	}

	//Fonction qui active/désactive la flèche au dessus du personnage
	public void Point_Character(){														
		indicator.SetActive (!indicator.activeInHierarchy);
	}

	//Fonction qui remet les points au max (fin de tour)
	public void Reset_Point()
	{
		movement_point = stats_de_deplacement;
		action_point = stats_daction;
	}

	//Fonction qui augmente la vie
	public void IncreaseHealth(int health_change, string Caller_Name)
	{
		combatHUD_master.Set_Hero_Health(this.gameObject);
		int previous_health = current_health;
		current_health += health_change;
		if (current_health > health_stat) {
			current_health = health_stat;
		}
		combatHUD_master.Change_Hero_Health(previous_health, current_health, health_stat);
		Show_Floating_Text (health_change);
		Send_Log_Message (health_change, Caller_Name);
	}

	//Fonction qui réduit la vie
	public void DeductHealth(int health_change, string Caller_Name){
		int previous_health = current_health;
		current_health -= health_change;
		if (current_health <= 0) {
			current_health = 0;
			StartCoroutine(Death ());
		} 
		combatHUD_master.Change_Hero_Health (previous_health, current_health, health_stat);

		Show_Floating_Text (-health_change);
		Send_Log_Message (-health_change, Caller_Name);
	}

	//Fonction qui fait un message pour le combat log
	public void Send_Log_Message(int health_change, string Caller_name){
		string message;
		if (health_change < 0) {
			if (current_health != 0) {
				message = Caller_name + " deal " + -health_change + " damages to " + this.name + " (" + current_health + " HP left).";
			} else {
				message = Caller_name + " deal " + -health_change + " damages to " + this.name + ". "+this.name+" is dead.";
			}
		} else  {
			message = this.name + " recieve a heal of " + health_change + " HP from "+ Caller_name+" ("+current_health+" HP left).";
		}
		combatlog_master.Add_Text (message, 1);
	}

	//Fonction qui fait apparaître un text flotant au dessus du personnage
	private void Show_Floating_Text(int value){
		GameObject go = Instantiate (damage_text, transform.position, Quaternion.identity, this.transform);		//Crée un text flotant fils du GameObject
		go.transform.localPosition += new Vector3(0,4.5f,0);													//Place ce GO au desssus du joueur
		go.GetComponent<MeshRenderer> ().sortingLayerName = "Animation";										//Définit son sortingLayer comme celui des animations pour qu'il apparaisse devant le reste
		if (value > 0) {																						//Change la couleur selon le montant
			go.GetComponent<TextMesh> ().color = Color.green;													
		} else {
			go.GetComponent<TextMesh> ().color = Color.red;
		}
		go.GetComponent<TextMesh> ().text = value.ToString ();													//Définit le texte à afficher
		Destroy (go, 1);																						//Le détruit après 1 sec
	}

	//Fonction qui gère la mort du personnage
	IEnumerator Death(){
		game_master.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);	//Vide la case où il se tenait
		while (combatHUD_master.Is_Animating()) {																//Attend la fin de l'annimation de la barre de vie
			yield return new WaitForSeconds (0.5f);
		}
		game_master.Remove_From_List (this.gameObject.name);													//Supprime le personnage de la liste
		Destroy (this.gameObject);																			
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
