using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero_Master : MonoBehaviour {
	private GameManager_Master Game_Master;
	private Hero_Attack_1 script_attack;
	private Deplacement script_deplacement;
	private GameManger_BeginEnnemyTurn ennemy_turn_script;
	public bool is_moving;
	public Text text_deplacement;
	public Text text_action;
	private float stats_de_deplacement;
	public float point_de_deplacement;
	private int stats_daction;
	private int action_point;
	private bool programmed_attack;
	private int next_attack_code;
	public Texture2D sprite_curseur;

	// Use this for initialization
	void Start () {
	}

	void OnEnable()
	{
		SetInitialReferences();
	}

	public void Reset_Point()
	{
		point_de_deplacement = stats_de_deplacement;
		action_point = stats_daction;
		setUI ();
	}

	void SetInitialReferences()
	{
		is_moving = false;
		programmed_attack = false;
		next_attack_code = 0;
		stats_de_deplacement = 4;
		stats_daction = 5;
		action_point = stats_daction;
		point_de_deplacement = stats_de_deplacement;
		Game_Master = GameObject.Find("GameManager (1)").GetComponent<GameManager_Master>();		//Permet l'accès aux infos du Game Master
		script_attack = this.GetComponent<Hero_Attack_1> ();
		script_deplacement = this.GetComponent<Deplacement> ();
		ennemy_turn_script = GameObject.Find("GameManager (1)").GetComponent<GameManger_BeginEnnemyTurn>();
		setUI ();
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

	public void Set_Next_Attack(int skill_number)
	{
		programmed_attack = true;
		next_attack_code = skill_number;
		Cursor.SetCursor (sprite_curseur,Vector2.zero,CursorMode.ForceSoftware);

	}

	// Update is called once per frame
	void Update () {
		if (Game_Master.is_it_your_turn == true && is_moving == false){							//Vérifie que c'est le tour du joueur
			
			if (Input.GetKeyDown (KeyCode.A)|| Input.GetKeyDown (KeyCode.Z))					//Gère les compétences
				{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);					//Crée un rayon
				RaycastHit hit;																	//Permet de récupérer la hitbox touchée
				if(Physics.Raycast(ray, out hit))												//Return True si le Rayon touche une hitbox à la position de la souris
					if (hit.transform.tag == "Ennemi")
						if (Input.GetKeyDown (KeyCode.A))										//Appel la première compétence
							script_attack.Frappe(hit.transform);
						if (Input.GetKeyDown (KeyCode.Z))
							script_attack.Lancer_de_Couteau(hit.transform);						//Appel la deuxième compétence
				}




			if (Input.GetMouseButtonDown (0)) {													//Gère les déplacements à la souris
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);					//Crée un rayon
				RaycastHit hit;																	//Permet de récupérer la hitbox touchée
				if (Physics.Raycast (ray, out hit)) {											//Return True si le Rayon touche une hitbox à la position de la souris
					if (hit.transform.tag == "Map") {												//Vérifie que l'objet touché fait partie de la map
						script_deplacement.justmove (hit.transform.position);					//Se déplace jusqu'à la case sélectionée
					}

					if (programmed_attack == true) {												//Gère les attaques à la souris
						if (hit.transform.tag == "Ennemi")
							switch (next_attack_code) {												//Switch qui détermine quelle attaque lancer
							case 1:
								script_attack.Frappe (hit.transform);
								break;
							case 2:
								script_attack.Lancer_de_Couteau (hit.transform);
								break;
							}
						programmed_attack = false;													//Suprimme l'action enregistrée
						Cursor.SetCursor (null,Vector2.zero,CursorMode.Auto);
					}
				}
			}





			if (Input.GetKeyDown (KeyCode.Space)) {												//Gère la fin de tour
				ennemy_turn_script.Begin_ennemy_turn();
			}
		}
	}
}
