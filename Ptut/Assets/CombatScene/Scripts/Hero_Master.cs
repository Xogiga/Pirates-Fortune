using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

namespace CombatScene{
public class Hero_Master : MonoBehaviour {
	public bool is_moving;
	private int stats_de_deplacement;
	[Range(0,100)] public int movement_point;
	private int stats_daction;
	[Range(0,100)] public int action_point;
	private int health_stat;
	[Range(0,100)] public int current_health;
	private GameObject indicator;
	public GameObject damage_text;

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
		indicator = this.transform.GetChild (0).gameObject;										//Recupère un gameObject fils
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
		References.CombatHud.Set_Hero_Health(this.gameObject);
		int previous_health = current_health;
		current_health += health_change;
		if (current_health > health_stat) {
			current_health = health_stat;
		}
		References.CombatHud.Change_Hero_Health(previous_health, current_health, health_stat);
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
		References.CombatHud.Change_Hero_Health (previous_health, current_health, health_stat);

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
		References.CombatLog.Add_Text (message, 1);
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
		References.GameMaster.set_matrice_case (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y), 0);	//Vide la case où il se tenait
		References.GameMaster.Remove_From_List (this.gameObject.name);											//Supprime le personnage de la liste

		while (References.CombatHud.Is_Animating()) {															//Attend la fin de l'annimation de la barre de vie
			yield return new WaitForSeconds (0.5f);
		}
		Destroy (this.gameObject);																			
	}

	public int Get_Movement_Point(){
		return movement_point;
	}

	public void Set_Movement_Point(int new_movement_point){
		movement_point = new_movement_point;
		References.CombatHud.Set_Hero_Movement_Point (this.gameObject);											//Affiche la réduction de point de mouvement
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
		References.CombatHud.Set_Hero_Action_Point (this.gameObject);						//Affiche la réduction de point d'action
	}
}
}