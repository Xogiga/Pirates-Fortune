﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager_BeginFight : MonoBehaviour {

	private GameManager_Master game_manager_master;

	public GameObject empty_tile;
	public GameObject obstacle;
	public GameObject player;
	public GameObject ennemi;
	public Canvas combat_HUD;
	public int width;
	public int height;
	public int nb_elem_rand;

	private int[,] matrice_case;
 
	private Collider[] colliders_list;

	private List<Tuples> case_spawn_equipe_alies ;																			//Liste des cases d'apparition possible des personnages alliés
	private List<Tuples> case_spawn_equipe_ennemy;																			//Liste des cases d'apparition possible des personnages ennemis

	public class Tuples																										//Permet de créer une sous-classe de type Tuples (int,int)
	{
		public int x;
		public int y;

		public Tuples(int i, int j){
			x=i;
			y=j;
		}				
	}



	void OnEnable(){
		Set_initial_references ();
		game_manager_master.event_begin_fight += Begin_fight;															//Ne marche plus
		/*Begin_fight ();
		game_manager_master.is_it_your_turn = true;*/
	}

	void OnDisable(){
		game_manager_master.event_begin_fight -= Begin_fight;
	}

	void Begin_fight(){
		create_matrice ();
		instantiate_matrice ();
		Pop_allié ();
		Pop_ennemi ();
		Display_HUD ();
	}

	void Set_initial_references()
	{
		game_manager_master = GetComponent<GameManager_Master> ();
		matrice_case = new int[width, height];
		create_list_spawn_alies ();
		create_list_spawn_ennemy ();

	}

	private void create_list_spawn_alies(){																			//Remplie la liste des cases de spawn alliés
		case_spawn_equipe_alies = new List<Tuples>();
		for (int x = 3; x <= 4; x++) {
			for (int y = 3; y <= 6; y++) {
				case_spawn_equipe_alies.Add(new Tuples(x, y));
			}

		}

	}

	private void create_list_spawn_ennemy(){																			//Remplie la liste des cases de spawn ennemis
		case_spawn_equipe_ennemy = new List<Tuples>();
		for (int x = 13; x <= 14; x++) {
			for (int y = 3; y <= 6; y++) {
				case_spawn_equipe_ennemy.Add(new Tuples(x, y));
			}
		}

	}

	public int[,] get_matrice_case(){																					//Retourne la matrice
		return matrice_case;
	}
		

	public void create_matrice(){																						//Crée une matrice de 0 et de 1
		for (int x = 0; x < width; x++) {
			for (int y = 0;y<height; y++) {
				if (x == 0 || y == 0 || x == width-1 || y == height-1) {
					matrice_case [x, y] = 1;
				} else {
					matrice_case [x, y] = 0;
				}
			}
		}
		Instantiate_random_obstacle ();
	}

	private bool is_it_in_list(int random_x,int random_y){																//Fonction qui chercher à savoir si un couple x,y fait partie des cases de spawn
		bool Inlist = false;
		foreach (Tuples t in case_spawn_equipe_alies) {
			if (t.x == random_x && t.y == random_y) {
				Inlist = true;
			}
		}
		foreach (Tuples t in case_spawn_equipe_ennemy) {
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
			if (matrice_case [random_x, random_y] == 0 && is_it_in_list(random_x,random_y) == false)					//Si la case est vide et qu'elle n'est pas dans la liste de case des spawn
			{
				matrice_case [random_x, random_y] = 1;
			} else {
				k--;
			}
		}
	}



	private void instantiate_matrice(){																					//Crée la map en placant des cases à partir de la matrice
		int x, y;
		for (x = 0; x <width; x++) {
			for (y = 0;y< height; y++) {
				if (matrice_case [x, y] == 0) {
					Instantiate (empty_tile, new Vector3 (x, y, 0f), Quaternion.identity);
				} else {
					Instantiate (obstacle, new Vector3 (x, y, 0f), Quaternion.identity);
				}
			}
		}
	}
		

	void Pop_allié()
	{
		int x =0, y=0, cpt =0;
		int i = Random.Range(0, case_spawn_equipe_alies.Count);
		foreach (Tuples t in case_spawn_equipe_alies) {
			if (cpt == i) {
				x = t.x;
				y = t.y;
			}
			cpt++;
		}
		Instantiate (player,new Vector3(x,y,0f), Quaternion.identity); 
	}

	void Pop_ennemi(){
		int x =0, y=0, cpt =0;
		int i = Random.Range(0, case_spawn_equipe_ennemy.Count);
		foreach (Tuples t in case_spawn_equipe_ennemy) {
			if (cpt == i) {
				x = t.x;
				y = t.y;
				matrice_case [x, y] = 1;
			}
			cpt++;
		}
		Instantiate (ennemi,new Vector3(x,y,0f), Quaternion.identity);  
	}

	void Display_HUD(){
		Instantiate(combat_HUD);
	}

	private Vector3 Find_a_free_tile (){

		Vector3 freePlace = Vector3.zero;
		colliders_list = Physics.OverlapSphere(freePlace, 0);
		while (colliders_list.Length != 1) {
			freePlace= new Vector3(Random.Range (0, width), Random.Range (0, height), 0f);
			colliders_list = Physics.OverlapSphere(freePlace, 0);
		}
		return freePlace;
	}
}
