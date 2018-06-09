using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_BeginFight : MonoBehaviour {

	private GameManager_Master game_manager_master;

	public GameObject empty_tile;
	public GameObject obstacle;
	public GameObject player;
	public GameObject ennemi;
	private int width;
	private int height;
	[Range(0,100)] public int nb_elem_rand;
	[Range(0,8)] public int nb_ennemies;
	[Range(0,8)] public int nb_allies; 
	private Tile[,] grid;

	private List<Tile> case_spawn_equipe_alies ;																				//Liste des cases d'apparition possible des personnages alliés
	private List<Tile> case_spawn_equipe_ennemy;																				//Liste des cases d'apparition possible des personnages ennemis

	private GameObject[] liste_perso = new GameObject[16];
	private FightParameters fp;

	void OnEnable(){
		Set_initial_references ();
		game_manager_master.event_begin_fight += Begin_fight;																	//Ajoute des scripts à l'évènement Begin_Fight
	}

	void OnDisable(){
		game_manager_master.event_begin_fight -= Begin_fight;
	}

	void Set_initial_references()
	{
		width = 18;
		height = 10;
		game_manager_master = GetComponent<GameManager_Master> ();
		create_list_spawn_alies ();
		create_list_spawn_ennemy ();
	}

	void Begin_fight(){
		if (DataTransporter_Script.data != null) {																	//Si un objet transporte des paramètres de combat
			Use_Fight_Parameters();																					//Récupère ces paramètres
		}
		create_matrice ();
		instantiate_matrice ();
		Pop_allié (nb_allies);
		Pop_ennemi (nb_ennemies);
		rempli_liste_perso();
	}

	void Use_Fight_Parameters(){
		FightParameters fp = DataTransporter_Script.data.Get_Fight_Parameters ();									//Récupère les données de l'objet
		nb_elem_rand = fp.random_obstacles;																			//Attribue ces donnèes aux variables correspondantes
		nb_allies = fp.allies_number;
		nb_ennemies = fp.ennemies_number;
		Destroy (DataTransporter_Script.data.gameObject);															//Détruit l'objet
	}

	private void create_list_spawn_alies(){																			//Remplie la liste des cases de spawn alliés
		case_spawn_equipe_alies = new List<Tile>();
		for (int x = 3; x <= 4; x++) {
			for (int y = 3; y <= 6; y++) {
				case_spawn_equipe_alies.Add(new Tile(x, y));
			}
		}
	}

	private void create_list_spawn_ennemy(){																		//Remplie la liste des cases de spawn ennemis
		case_spawn_equipe_ennemy = new List<Tile>();
		for (int x = 13; x <= 14; x++) {
			for (int y = 3; y <= 6; y++) {
				case_spawn_equipe_ennemy.Add(new Tile(x, y));
			}
		}

	}

	public Tile[,] get_matrice_case(){																					//Retourne la matrice
		return grid;
	}
		

	public void create_matrice(){																						//Crée une matrice de 0 et de 1
		grid = new Tile[width,height];
		for (int x = 0; x < width; x++) {
			for (int y = 0;y<height; y++) {
				if (x == 0 || y == 0 || x == width-1 || y == height-1) {
					grid [x, y] = new Tile(x, y, -1);
				} else {
					grid [x, y] = new Tile(x, y, 0);
				}
			}
		}
		Instantiate_random_obstacle ();
	}

	private bool is_it_in_list(int random_x,int random_y){																//Fonction détermine si un couple x,y fait partie des cases de spawn
		bool Inlist = false;
		foreach (Tile t in case_spawn_equipe_alies) {
			if (t.x == random_x && t.y == random_y) {
				Inlist = true;
			}
		}
		foreach (Tile t in case_spawn_equipe_ennemy) {
			if (t.x == random_x && t.y == random_y) {
				Inlist = true;
			}
		}


		return Inlist;
	}

	private void Instantiate_random_obstacle (){																		//Crée des obstacles aléatoires
		int random_x, random_y;

		for (int k = 0; k < nb_elem_rand; k++) {																		//Boucle qui met des obstacles de manière aléatoire
			random_x = Random.Range (1, width-1);
			random_y = Random.Range (1, height-1);
			if (grid [random_x, random_y].state == 0 && is_it_in_list(random_x,random_y) == false)						//Si la case est vide et qu'elle n'est pas dans la liste de case des spawn
			{
				grid [random_x, random_y].state = -1;
			} else {
				k--;
			}
		}
	}



	private void instantiate_matrice(){																					//Crée la map en placant des cases à partir de la matrice
		GameObject tile;
		int x, y;

		GameObject map = new GameObject("map");
		for (x = 0; x <width; x++) {
			for (y = 0;y< height; y++) {
				if (grid [x, y].state == 0) {
					tile = Instantiate (empty_tile, new Vector3 (x, y, 0f), Quaternion.identity,map.transform);
				} else {
					tile = Instantiate (obstacle, new Vector3 (x, y, 0f), Quaternion.identity,map.transform);
				}
				grid [x, y].obj = tile;
			}
		}
	}
		

	void Pop_allié(int nb_allies)
	{
		int x =0, y=0, cpt, cpt_allies =0, i;																		//cpt_allies : répétitions de la boucle pour chaque spawn
		while (cpt_allies < nb_allies) {
			do {
				cpt=0;
				i = Random.Range (0, case_spawn_equipe_alies.Count);
				foreach (Tile t in case_spawn_equipe_alies) {
					if (cpt == i) {
						x = t.x;
						y = t.y;
					}
					cpt++;
				}

			} while (grid [x, y].state == 1); 

			grid [x, y].state = 1;
			Instantiate (player, new Vector3 (x, y, 0f), Quaternion.identity); 
			cpt_allies++;
		}
	}

	void Pop_ennemi(int nb_enn){
		int x =0, y=0, cpt, cpt_enn =0, i;																		//cpt_allies : répétitions de la boucle pour chaque spawn
		while (cpt_enn < nb_enn) {
			do {
				cpt=0;
				i = Random.Range (0, case_spawn_equipe_ennemy.Count);
				foreach (Tile t in case_spawn_equipe_ennemy) {
					if (cpt == i) {
						x = t.x;
						y = t.y;
					}
					cpt++;
				}

			} while (grid [x, y].state == 1); 

			grid [x, y].state = 1;
			Instantiate (ennemi, new Vector3 (x, y, 0f), Quaternion.identity); 
			cpt_enn++;
		}
	}
		

	//Remplissage de la liste des personnages en alternant allié/ennemi
	private void rempli_liste_perso () {																	
		int compteur_ennemies = 0;
		int compteur_heros = 0;
		int indice_tableau_globale = 0;
		GameObject[] liste_gentil = GameObject.FindGameObjectsWithTag ("Hero");
		GameObject[] liste_mechant = GameObject.FindGameObjectsWithTag ("Ennemy");

		while (indice_tableau_globale < liste_perso.Length-1) {																//Tant que tous les personnages ne sont pas ajoutés, continue de boucler
			if (compteur_heros < liste_gentil.Length) { 																	//Si le compteur est bien dans la liste
				if (liste_gentil [compteur_heros] != null) {																//Si la case à cette indice n'est pas nulle
					liste_gentil [compteur_heros].name = "Hero_" + indice_tableau_globale;											//Change le nom de l'objet
					liste_perso [indice_tableau_globale] = liste_gentil [compteur_heros];									//Ajoute un personnage à la liste

				}
				indice_tableau_globale++;
				compteur_heros++;
			}
			if (compteur_ennemies < liste_mechant.Length) {
				if (liste_mechant [compteur_ennemies] != null) {
					liste_mechant [compteur_ennemies].name = "Ennemi_" + indice_tableau_globale;
					liste_perso [indice_tableau_globale] = liste_mechant [compteur_ennemies];

				}
				indice_tableau_globale++;
				compteur_ennemies++;
			}
			if (compteur_ennemies >= liste_mechant.Length && compteur_heros >= liste_gentil.Length ){						//S'il n'y a ni gentil, ni méchant  a ajouter on augmente le compteur jusqu'à sortir de la boucle
				indice_tableau_globale++;
			}
		}
	}
		
	//Fonction qui retourne la liste ordonnée des personnages
	public GameObject[] get_liste_perso(){
		return liste_perso;
	}

	public int Get_Width(){
		return width;
	}

	public int Get_Height(){
		return height;
	}
}
