using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatHUD_Master : MonoBehaviour {
	private GameObject announce;
	private GameObject button_end_turn;
	private GameObject health_bar;
	private Image health_image;
	private Text health_text;
	private GameObject movement_point_text;
	private GameObject action_point_text;
	private GameObject hero_stats_canvas;


	// Use this for initialization
	void OnEnable () {
		SetInitialReferences ();
	}

	void SetInitialReferences()
	{
		announce = GameObject.Find (this.name + "/Announce");
		announce.SetActive (false);

		button_end_turn = GameObject.Find (this.name + "/Button");
		hero_stats_canvas = GameObject.Find (this.name + "/Hero Stats Canvas");

		health_bar = GameObject.Find (this.name+"/Hero Stats Canvas/InfoJoueur/Cadre de barre de vie/Barre de vie");
		health_image = health_bar.GetComponentInChildren<Image> ();
		health_text = health_bar.GetComponentInChildren<Text> ();
		movement_point_text = GameObject.Find (this.name + "/Hero Stats Canvas/InfoJoueur/Deplacement");
		action_point_text = GameObject.Find (this.name + "/Hero Stats Canvas/InfoJoueur/Action Point");

		enable_disable_button_and_stats ();
	}

	//Fonction qui met à jour les infos du personnage
	public void Set_Hero_Points(GameObject hero){		
		Set_Hero_Action_Point (hero);
		Set_Hero_Movement_Point (hero);
		Set_Hero_Health (hero);
	}

	//Fonction qui active/desactive le bouton fin de tour
	public void enable_disable_button_and_stats(){
		button_end_turn.SetActive (!button_end_turn.activeInHierarchy);
		hero_stats_canvas.SetActive (!hero_stats_canvas.activeInHierarchy);
	}

	//Fonction qui change les points d'actions
	public void Set_Hero_Action_Point(GameObject hero){
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();
		action_point_text.GetComponentInChildren<Text>().text = "Action Point = "+ hero_master.Get_Action_Point();
	}

	//Fonction qui change les points de mouvement
	public void Set_Hero_Movement_Point(GameObject hero){
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();
		movement_point_text.GetComponentInChildren<Text> ().text = "Movement Point = " + hero_master.Get_Movement_Point ();
	}

	//Fonction qui affiche les PV du héros
	public void Set_Hero_Health(GameObject hero){
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();

		health_text.text = hero_master.Get_Current_Health ().ToString();
		health_image.fillAmount = hero_master.Get_Current_Health()/hero_master.Get_Health_Stats();
	}

	//Fonction qui change la barre de vie
	public void Change_Hero_Health(int hero_health, int new_hero_health, int max_health){
		if (new_hero_health > hero_health) {
			IncreaseBar (new_hero_health,max_health);
		} else {
			DecreaseBar (new_hero_health,max_health);
		}
		health_text.text = hero_health.ToString ();
	}

	//Fonction qui augmente la barre de vie
	IEnumerator IncreaseBar(int new_hero_health, int max_health){
		while (health_image.fillAmount <= new_hero_health / max_health) {
			yield return new WaitForSeconds (Time.deltaTime);
			health_image.fillAmount += 0.01f;
		}
		health_image.fillAmount = new_hero_health / max_health;
	}

	//Fonction qui diminue la barre de vie
	IEnumerator DecreaseBar(int new_hero_health, int max_health){
		while (health_image.fillAmount >= new_hero_health / max_health) {
			yield return new WaitForSeconds (Time.deltaTime);
			health_image.fillAmount -= 0.01f;
		}
		health_image.fillAmount = new_hero_health / max_health;  									
	}

	//Fonction qui change le message de l'annonce
	public void Announce(string message){
		announce.gameObject.SetActive (true);
		announce.GetComponentInChildren<Text> ().text = message;
		StartCoroutine (Disable_Announce());
	}

	//Fait disparaitre l'annonce
	IEnumerator Disable_Announce(){
		yield return new WaitForSeconds (1);
		announce.gameObject.SetActive (false);
	}
}
