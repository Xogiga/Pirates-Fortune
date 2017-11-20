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
	public Canvas combat_HUD;
	public int width;
	public int height;
	public int nb_elem_rand;
	private List<Vector3> list_tile = new List<Vector3>();
	private Collider[] colliders_list;

	void OnEnable(){
		Set_initial_references ();
		game_manager_master.event_begin_fight += Begin_fight;
	}

	void OnDisable(){
		game_manager_master.event_begin_fight -= Begin_fight;
	}

	void Set_initial_references()
	{
		game_manager_master = GetComponent<GameManager_Master> ();
	}

	void Begin_fight(){
		Create_map ();
		Pop_heros ();
		Pop_ennemi ();
		Display_HUD ();
	}


	void Create_map()
	{
		Initial_list ();                                         //Serait mieux si on pouvait supprimer les cases bloqués de la liste au fur et à mesure

		int max = (width)*(height);

		for(int k=0; k<nb_elem_rand;k++)
		{
			Vector3 place = Vector3.zero;
			int randomIndex = Random.Range (0, max);
			place= list_tile[randomIndex];
			if (place.x != 0 && place.y != 0 && place.x != width && place.y != height) {
				Instantiate (obstacle, place, Quaternion.identity);
				list_tile.RemoveAt (randomIndex);
				max = max - 1;
			} else {
				k--;
			}
		}

		foreach (Vector3 place in list_tile) {
			if (place.x == 0 || place.y == 0 || place.x == width || place.y == height) {
				Instantiate (obstacle, place, Quaternion.identity);
			}
		}

		foreach (Vector3 place in list_tile) {
			if (place.x != 0 && place.y != 0 && place.x != width && place.y != height) {
				Instantiate (empty_tile, place, Quaternion.identity);
			}
		}
	}

	void Initial_list()
	{
		list_tile.Clear ();
		for (int i = 0; i <= width; i++) {
			for (int j = 0; j <= height; j++) {
				list_tile.Add(new Vector3(i,j,0f));
			}
		}

	}

	void Pop_heros()
	{
		Vector3 freeTileP = Find_a_free_tile ();    
		Instantiate (player, freeTileP, Quaternion.identity); 
	}

	void Pop_ennemi(){
		Vector3 freeTileE = Find_a_free_tile ();            
		Instantiate (ennemi, freeTileE, Quaternion.identity); 
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
