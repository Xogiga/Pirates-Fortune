using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero_Master : MonoBehaviour {
	public bool is_moving;
	public Text text_deplacement;
	public Text text_action;
	private float stats_de_deplacement;
	public float point_de_deplacement;
	private int stats_daction;
	private int action_point;
	private Canvas canvas_hero;



	// Use this for initialization
	void Start () {
	}

	void OnEnable()
	{
		SetInitialReferences();
	}

	void SetInitialReferences()
	{
		is_moving = false;
		stats_de_deplacement = 4;
		stats_daction = 5;
		action_point = stats_daction;
		point_de_deplacement = stats_de_deplacement;
		canvas_hero = this.GetComponentInChildren<Canvas>();
		activer_desactiver_canvas ();
		setUI ();
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
		point_de_deplacement = stats_de_deplacement;
		action_point = stats_daction;
		setUI ();
	}

	public void activer_desactiver_canvas(){
		canvas_hero.enabled = !canvas_hero.enabled;

	}
		
	public void setUI(){																		//Met à jour les infos du personnage
		text_deplacement.text = "Movement Point = "+point_de_deplacement;
		text_action.text = "Action Point = " + action_point;
	}

	public int Get_Action_Point(){
		return action_point;
	}

	public void Set_Action_Point(int val){
		action_point = val;
		setUI ();
	}
}
