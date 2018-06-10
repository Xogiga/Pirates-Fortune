using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombatScene{
public class GameManager_Commands : MonoBehaviour {
	private GameManager_Master game_master;
	private GameObject hero;
	private Hero_Master hero_master;
	private Hero_Attack_1 script_attack;
	private Hero_Deplacement script_deplacement;
	private bool programmed_attack;
	private int next_attack_code;
	public Texture2D sprite_curseur;
	private CombatHUD_Master combatHUD_master;

	void OnEnable(){
		Set_initial_references ();
	}

	//Reférences fixes
	private void Set_initial_references () {																		
		game_master = this.GetComponent<GameManager_Master>();
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master> ();
		programmed_attack = false;
		next_attack_code = -1;
	}

	//Référence amené à changer en fonction du tour (du personnage)
	public void Set_new_references(){																
		hero = game_master.get_playing_perso ();
		hero_master = hero.GetComponent<Hero_Master>();
		script_attack = hero_master.GetComponent<Hero_Attack_1> ();
		script_deplacement = hero_master.GetComponent<Hero_Deplacement> ();
	}

	void Update () {
		if (game_master.is_it_your_turn == true) {														//Vérifie que c'est le tour du joueur
			if (hero_master.is_moving == false) {														//Vérifie que le héros ne se déplace pas déjà
				if (EventSystem.current.IsPointerOverGameObject ()) {									//Si le pointeur est au dessus d'un élément de l'ATH, sort de la fonction.
					return;
				}
				if (Input.GetKeyDown (KeyCode.A) || Input.GetKeyDown (KeyCode.Z)) {						//Gère les compétences
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);						//Crée un rayon
					RaycastHit hit;																		//Permet de récupérer la hitbox touchée
					if (Physics.Raycast (ray, out hit)) {												//Return True si le Rayon touche une hitbox à la position de la souris
						if (hit.transform.tag == "Ennemy") {
							if (Input.GetKeyDown (KeyCode.A))											//Appel la première compétence
								script_attack.Frappe (hit.transform);
							if (Input.GetKeyDown (KeyCode.Z))
								script_attack.Lancer_de_Couteau (hit.transform);						//Appel la deuxième compétence
						}
					}
				}


				if (Input.GetMouseButtonDown (0)) {														//Gère les déplacements à la souris
					Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);						//Crée un rayon
					RaycastHit hit;																		//Permet de récupérer la hitbox touchée
					if (Physics.Raycast (ray, out hit)) {												//Return True si le Rayon touche une hitbox à la position de la souris
						if (programmed_attack == true) {												//Gère les attaques à la souris
							if (hit.transform.tag == "Ennemy") {
								switch (next_attack_code) {												//Switch qui détermine quelle attaque lancer
								case 1:
									script_attack.Frappe (hit.transform);
									break;
								case 2:
									script_attack.Lancer_de_Couteau (hit.transform);
									break;
								}
							} else {
								combatHUD_master.Announce ("You can't attack this.");
							}
						} else if (hit.transform.tag == "Map") {										//Vérifie que l'objet touché fait partie de la map
							script_deplacement.try_to_move (hit.transform.position);					//Se déplace jusqu'à la case sélectionée
						}
					}
					Reset_cursor ();
				}

				if (Input.GetKeyDown (KeyCode.Space)) {													//Appel la fin de tour
						game_master.passer_le_tour ();
				}
			}
		}
		if (Input.GetKey ("escape")) 																	//Retourne au Menu
		{
				StartCoroutine(game_master.Load_Next_Scene_In_Background("MenuScene"));
		}
	}

	//Fonction qui change le curseur et définit le numéro de la prochaine compétence lancée à la souris
	public void Set_Next_Attack(int skill_number)
	{
		programmed_attack = true;
		next_attack_code = skill_number;
		Cursor.SetCursor (sprite_curseur,new Vector2 (10,15),CursorMode.ForceSoftware);
	}

	public void Reset_cursor(){
		if (programmed_attack == true) {
			programmed_attack = false;													//Suprimme l'action enregistrée
			Cursor.SetCursor (null, Vector2.zero, CursorMode.Auto);						//Remet le curseur classique
		}
	}
}
}
