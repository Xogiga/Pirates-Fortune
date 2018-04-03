using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Competence_Master : MonoBehaviour {
	private GameObject infobulle;
	private GameManager_Master game_master;
	private List<SpriteRenderer> sprite_list;

	void OnEnable(){
		Set_initial_reference ();
	}

	void Set_initial_reference(){
		game_master = GameObject.FindWithTag("GameManager").GetComponent<GameManager_Master>();
	}

	private void Find_Infobulle(int Numero_competence){
		infobulle = GameObject.Find("CombatHUD/Hero Stats Canvas/Barre des competences/Competence_"+Numero_competence+"/Infobulle_"+Numero_competence);
	}

	//Fonction qui affiche la range et les informations de la compétence au survol de celle-ci
	public void Show_Attack_Info(int Numero_competence){
		Show_infobulle(Numero_competence);
		Show_Attack_Range(Numero_competence);
	}

	//Fonction qui affiche la range et les informations de la compétence au survol de celle-ci
	public void Hide_Attack_Info(){
		Hide_infobulle ();
		Hide_Attack_Range();
	}

	//Fonction qui affiche l'infobulle
	private void Show_infobulle(int Numero_competence){
		Find_Infobulle(Numero_competence);
		infobulle.SetActive (true);
	}

	//Fonction qui cache l'infobulle
	private void Hide_infobulle(){
		if (infobulle != null) {
			infobulle.SetActive (false);
			infobulle = null;
		}
	}


	//Fonction qui affiche les cases que le joueur peut attaqué avec la compétence qu'il survole
	private void Show_Attack_Range(int Numero_competence){
		GameObject hero = game_master.get_playing_perso ();
		Hero_Master hero_master = hero.GetComponent<Hero_Master> ();
		Hero_Attack_1 hero_attack = hero.GetComponent<Hero_Attack_1> ();

		int range_min, range_max;
		hero_attack.Get_Range_Skill (Numero_competence,out range_min,out range_max);

		if (hero_master.is_moving == false) {																								//Si le héros survolé ne se déplace pas et que c'est son tour
			Get_Tile_List(range_min,range_max,hero);																						//Récupère la liste de case a portée d'attaque
			foreach (SpriteRenderer s in sprite_list) {																						//Pour chaque case
				s.color =  new Color32(255,45,36,255);																						//Change la couleur
			}
		}
	}

	//Fonction qui redonne leur couleur de base aux cases
	private void Hide_Attack_Range(){
		if (sprite_list != null) {																											//Si la liste des cases colorés n'est pas vide
			foreach (SpriteRenderer s in sprite_list) {																						//Pour chaque case
				s.color =  new Color32(255,255,255,255);																					//Change la couleur
			}
			sprite_list = null;
		}
	}

	//Fonction qui récupère la liste de case libre autour du joueur
	private void Get_Tile_List(int range_min, int range_max, GameObject hero){
		sprite_list = new List<SpriteRenderer>();
		Tile[,] grid = game_master.get_matrice ();

		int HeroX = Mathf.RoundToInt (hero.transform.position.x);
		int HeroY =  Mathf.RoundToInt(hero.transform.position.y);
		for (int i = HeroX - range_max ; i <= HeroX + range_max; i++) {																		//On récupère les cases autour de lui en fonction de la portée maximale du sort
			for (int j = HeroY - range_max; j <= HeroY + range_max; j++) {
				if (i> 0 && i <17 && j>0 && j <9) {																							//On vérifie que la case se situe bien dans la grille
					int distance = Mathf.Abs(i - HeroX) + Mathf.Abs(j - HeroY);
					if (distance <= range_max && distance >= range_min) {																	//On vérifie que la distance à la case est bien inférieure dans l'intervalle de la portée de la compétence
						if (grid [i, j].state != -1) {																						//On vérifie que la case n'est pas un mur
							sprite_list.Add (grid [i, j].obj.GetComponent<SpriteRenderer> ());												//Ajoute le sprite de la case à la liste
						}
					}
				}
			}
		}
	}
}



