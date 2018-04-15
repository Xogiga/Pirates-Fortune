using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Master : MonoBehaviour {
	public delegate void GameManager_EventHandler();  //Gestionnaire d'évènement
	public event GameManager_EventHandler event_begin_fight;

	public bool is_fight_begin;
	public bool is_it_your_turn;
	public bool is_fight_over;
	private GameManager_BeginFight script_creation_map;
	private GameManager_Commands script_commande;
	private Tile[,] matrice_case;
	private GameObject[] liste_perso;
	private GameObject[] list_case;
	private int indice_playing_perso;
	private CombatHUD_Master combatHUD_master;
	private int turn;

	private void OnEnable(){
		Set_initial_reference();
		Call_event_begin_fight();
	}


	private void Set_initial_reference(){
		is_fight_begin = false;
		is_it_your_turn = false;
		is_fight_over=false;
		script_creation_map = this.GetComponent<GameManager_BeginFight>();
		script_commande = this.GetComponent<GameManager_Commands> ();
		indice_playing_perso = 0;
		turn = 0;
		combatHUD_master = GameObject.Find ("CombatHUD").GetComponent<CombatHUD_Master>();	
	}


	//Event qui se déclenche unitquement en début de combat
	public void Call_event_begin_fight(){
		if(event_begin_fight == null){
			print("l'évènement est vide, problème de priorité");
		}
		if (event_begin_fight != null && is_fight_begin == false)
		{
			is_fight_begin = true;																						//Déclare le combat commencé
			event_begin_fight ();																						//Lance les fonctions de l'event begin_fight :
			matrice_case = script_creation_map.get_matrice_case ();														//Récupère la matrice
			liste_perso = script_creation_map.get_liste_perso ();														//Récupère la liste des personnages dans l'ordre des tours
			this.GetComponent<GameManager_Commands>().enabled = true;													//Active le script des commandes
			StartCoroutine(begin_hero_turn());
		}
	}		
		
	//Fonction qui supprime un personnage de la liste
	public void Remove_From_List(string name){		
		int dead_indice = -1;
		for (int i = 0; i < liste_perso.Length && liste_perso[i] != null; i++) {										//Parcours toute la liste jusqu'à la fin ou jusqu'à une case vide
			if (dead_indice == -1 && liste_perso[i].name == name) {														//Si le nom dans la liste correspond au mort
				dead_indice = i;																						//Définit l'indice du mort
			}
			if (dead_indice != -1) {																					//Une fois que l'indice du mort est trouvé
				liste_perso [i] = liste_perso [i + 1];																	//"Recule" toute les cases de 1 à partir de cette indice
			}
		}

		if (indice_playing_perso > dead_indice) {																		//Si l'indice du personnage qui joue est supérieur à l'indice du mort
			indice_playing_perso--;																						//Baisse l'indice du personnage de 1
		}

		Is_Fight_Done();																								//Vérifie que les deux camps soit toujours vivant
	}

	//Fonction qui vérifie si le combat n'est pas terminé
	private void Is_Fight_Done(){
		bool is_any_hero_alive = false;
		bool is_any_ennemy_alive = false;

		foreach (GameObject g in liste_perso) {																			//Parcours la liste des personnage								
			if (g != null) {
				if (g.tag == "Hero") {																					//Vérifie s'il reste un héros vivant
					is_any_hero_alive = true;
				} else if (g.tag == "Ennemy") {																			//Vérifie s'il reste un ennemi vivant
					is_any_ennemy_alive = true;
				}
			}
		}

		if(is_any_hero_alive == false || is_any_ennemy_alive == false){
			if (is_any_ennemy_alive == false) {
				End_Fight (true);
			} else {
				End_Fight (false);
			}

		}
	}

	//Fonction qui termine le combat
	private void End_Fight(bool victory){
		is_fight_over = true;																							//Déclare le combat terminé
		combatHUD_master.Show_End_Screen(victory);																		//Affiche l'écran de fin
	}
		
	public void passer_le_tour(){
		if (get_playing_perso ().tag == "Hero") {																		//Si le personnage précédent est un héros
			end_hero_turn();
		}

		if (get_playing_perso ().tag == "Ennemy") {																		//Si le personnage précédent est un ennemi
			end_ennemy_turn();
		}
			
		if (liste_perso [indice_playing_perso + 1] == null) {															//Si la case suivante est null revient à 0
			indice_playing_perso = 0;
		} else {
			indice_playing_perso++;																						//Passe au personnage suivant
		}
		turn++;

		if (get_playing_perso ().tag == "Hero") {																		//Si le personnage suivant est un héros
			StartCoroutine(begin_hero_turn());
		}

		if (get_playing_perso ().tag == "Ennemy") {																		//Si le personnage suivant est un ennemi
			StartCoroutine(begin_ennemy_turn());
		}
	}

	//Gère la fin de tour allié
	private void end_hero_turn(){
		is_it_your_turn = false;																						//On ne donne plus la main au joueur
		script_commande.Reset_cursor();																					//On change le curseur si besoin
		get_playing_perso ().GetComponent<Hero_Master> ().Point_Character();											//Désactive la flèche au dessus du personnage
		combatHUD_master.enable_disable_button_and_stats ();															//On désactive les infos du héros et le bouton fin de tour
		get_playing_perso ().GetComponent<Hero_Master>().Reset_Point();													//On lui redonne ses points
	}

	//Gère la fin de tour ennemi
	private void end_ennemy_turn(){
		combatHUD_master.disable_ennemy_stats();																		//Cache les stats du personnage
		get_playing_perso ().GetComponent<Ennemy_Master> ().Point_Character();											//Désactive la flèche au dessus du personnage
		get_playing_perso ().GetComponent<Ennemy_Master>().Reset_Point();
	}

	//Gère le début de tour allié
	IEnumerator begin_hero_turn(){
		while(combatHUD_master.Is_Animating()){
			yield return new WaitForSeconds (0.5f);
		}
		is_it_your_turn = true;																							//On donne la main au joueur
		combatHUD_master.Announce ("Your Turn !");																		//Annonce le tour allié
		combatHUD_master.Set_Hero_Points (get_playing_perso());															//On affiche ses stats
		combatHUD_master.enable_disable_button_and_stats ();															//On désactive les infos du héros et le bouton fin de tour
		script_commande.Set_new_references();																			//On donne les références du nouveaux personnage aux commandes
		get_playing_perso ().GetComponent<Hero_Master> ().Point_Character();											//Affiche la flèche au dessus du personnage
	}

	//Gère le début de tour ennemi
	IEnumerator begin_ennemy_turn(){
		while(combatHUD_master.Is_Animating()){
			yield return new WaitForSeconds (0.5f);
		}
		combatHUD_master.Announce ("Ennemy Turn !");																	//Annonce le tour ennemi
		combatHUD_master.Set_Ennemy_Health(get_playing_perso());														//Fais correspondre les stats du personnage
		combatHUD_master.enable_ennemy_stats();																			//Affiche les stats du personnage
		get_playing_perso ().GetComponent<Ennemy_Master> ().Point_Character();											//Affiche la flèche au dessus du personnage
		get_playing_perso ().GetComponent<Ennemy_Master> ().Comportement ();											//Appel son comportement
	}

	//Retourne le personnage qui joue son tour
	public GameObject get_playing_perso(){
		return liste_perso [indice_playing_perso];
	}

	//Retourne le personnage qui joue son tour
	public int get_turn(){
		return turn;
	}

	public void set_matrice_case(int x,int y,int val){																	//Modifie la valeur d'une case de la matrice passé en paramètre
		matrice_case[x,y].state = val;
	}

	public int get_matrice_case(int x,int y){																			//Renvoie la valeur d'une case de la matrice passé en paramètre
		return matrice_case[x,y].state;
	}

	public Tile[,] get_matrice(){																						//Retourne la matrice
		return matrice_case;
	}	

	//Fonction qui redémarre la scène
	public void Restart_Level(){
		SceneManager.LoadScene (0);
	}
}
