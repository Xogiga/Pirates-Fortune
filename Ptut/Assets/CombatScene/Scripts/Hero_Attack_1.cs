﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatScene{
public class Hero_Attack_1 : MonoBehaviour {
	private Hero_Master hero_master;

	// Use this for initialization
	void Start () {
		Set_references ();
	}

	void Set_references(){
		hero_master = this.GetComponent<Hero_Master> ();
	}		

	public int Range(Transform ennemy)
	{
		int distance = Mathf.Abs(Mathf.RoundToInt (ennemy.position.x - this.transform.position.x))
			+ Mathf.Abs(Mathf.RoundToInt (ennemy.position.y - this.transform.position.y));
		return distance;
	}

	private bool Is_Action_Possible(int cost){
		if (cost <= hero_master.Get_Action_Point ()) {
			hero_master.Set_Action_Point (hero_master.Get_Action_Point () - cost);
			return true;
		}
		else {
			References.CombatHud.Announce ("Not enough Action Point !");	
			return false;
			}
	}

	public void Frappe(Transform ennemy)
	{
		int range = Range (ennemy);
		if (range <= 1) {
			if (Is_Action_Possible (3)) {
				Ennemy_Master ennemy_master = ennemy.GetComponent<Ennemy_Master> ();
				ennemy_master.DeductHealth (30, name);
			}
		} else {
			References.CombatHud.Announce ("You're too far !");																								//Ajouter  un son
		}
	}

	public void Lancer_de_Couteau(Transform ennemy)
	{
		int range = Range (ennemy);
		if (range >= 2 && range <= 4) 
		{
			if (Is_Action_Possible (1)) {
				Ennemy_Master ennemy_master = ennemy.GetComponent<Ennemy_Master> ();
				ennemy_master.DeductHealth (6, name);
			}
		} else {
			References.CombatHud.Announce ("You're not in range !");																						//Ajouter un son
		}
	}

	//Fonction qui renvoie la portée minimale et maximale des attaques des compétences
	public void Get_Range_Skill(int skill_number, out int range_min, out int range_max){
		if (skill_number == 1) {
			range_min = 1;
			range_max = 1;
		} else {
			range_min = 2;
			range_max = 4;
		}
	}
}
}