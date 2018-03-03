using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Master : MonoBehaviour {
	private GameManager_Master Game_Manager;
	private Hero_Attack_1 Attack1;

	// Use this for initialization
	void Start () {
	}

	void OnEnable()
	{
		SetInitialReferences();
	}

	void SetInitialReferences()
	{
		Game_Manager = GameObject.Find("GameManager").GetComponent<GameManager_Master>();
		Attack1 = GameObject.Find ("Hero(Clone)").GetComponent<Hero_Attack_1> ();
	}

	// Update is called once per frame
	void Update () {
		if (Game_Manager.is_it_your_turn == true){												//Vérifie que c'est le tour du joueur
			if (Input.GetKeyDown (KeyCode.A))
				{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);					//Crée un rayon
				RaycastHit hit;																	//Permet de récupérer la hitbox touchée
				if(Physics.Raycast(ray, out hit))												//Return True si le Rayon touche une hitbox à la position de la souris
					{
					if (hit.transform.tag == "Ennemi")
						Attack1.attack1(hit.transform);														//Appel l'attaque
					}

				}
			}
	}
}
