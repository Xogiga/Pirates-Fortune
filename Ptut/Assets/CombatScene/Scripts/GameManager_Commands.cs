using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_Commands : MonoBehaviour {
	private GameManager_Master game_master;
	private GameObject hero;
	private Hero_Master hero_master;
	private Hero_Attack_1 script_attack;
	private Deplacement script_deplacement;
	private bool programmed_attack;
	public int next_attack_code;
	public Texture2D sprite_curseur;

	void OnEnable(){
		Set_initial_references ();
	}

	//Reférences fixes
	void Set_initial_references () {																		
		game_master = this.GetComponent<GameManager_Master>();
		programmed_attack = false;
		next_attack_code = -1;
		Set_new_references ();
	}

	//Référence amené à changer en fonction du tour (du personnage)
	public void Set_new_references(){																
		hero = game_master.get_playing_perso ();
		hero_master = hero.GetComponent<Hero_Master>();
		script_attack = hero_master.GetComponent<Hero_Attack_1> ();
		script_deplacement = hero_master.GetComponent<Deplacement> ();
	}

	void Update () {
		if (game_master.is_it_your_turn == true && hero_master.is_moving == false){					//Vérifie que c'est le tour du joueur

			if (Input.GetKeyDown (KeyCode.A)|| Input.GetKeyDown (KeyCode.Z))						//Gère les compétences
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);						//Crée un rayon
				RaycastHit hit;																		//Permet de récupérer la hitbox touchée
				if(Physics.Raycast(ray, out hit))													//Return True si le Rayon touche une hitbox à la position de la souris
				if (hit.transform.tag == "Ennemi")
				if (Input.GetKeyDown (KeyCode.A))													//Appel la première compétence
					script_attack.Frappe(hit.transform);
				if (Input.GetKeyDown (KeyCode.Z))
					script_attack.Lancer_de_Couteau(hit.transform);									//Appel la deuxième compétence
			}




			if (Input.GetMouseButtonDown (0)) {														//Gère les déplacements à la souris
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);						//Crée un rayon
				RaycastHit hit;																		//Permet de récupérer la hitbox touchée
				if (Physics.Raycast (ray, out hit)) {												//Return True si le Rayon touche une hitbox à la position de la souris
					if (hit.transform.tag == "Map") {												//Vérifie que l'objet touché fait partie de la map
						script_deplacement.try_to_move (hit.transform.position);						//Se déplace jusqu'à la case sélectionée
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
		}
		if (Input.GetKeyDown (KeyCode.Space)) {														//Appel la fin de tour
			game_master.passer_le_tour();
		}

		if (Input.GetKey ("escape")) 																//Ferme l'application
		{
			Application.Quit ();
		}
	}


	public void Set_Next_Attack(int skill_number)
	{
		programmed_attack = true;
		next_attack_code = skill_number;
		Cursor.SetCursor (sprite_curseur,Vector2.zero,CursorMode.ForceSoftware);

	}


}
