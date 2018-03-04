using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Attack_1 : MonoBehaviour {
	

	// Use this for initialization
	void Start () {
	}


	
	// Update is called once per frame
	void Update () {
		
	}

	public int Range(Transform ennemy)
	{
		int distance = Mathf.Abs(Mathf.RoundToInt (ennemy.position.x - this.transform.position.x))
			+ Mathf.Abs(Mathf.RoundToInt (ennemy.position.y - this.transform.position.y));
		return distance;
	}

	public void Frappe(Transform ennemy)
	{
		int range = Range (ennemy);
		if (range <= 1) {
			EnemyHealth ennemy_health = ennemy.GetComponent<EnemyHealth> ();
			ennemy_health.DeductHealth (30);
		} else {
			print ("Vous êtes trop loin");																								//Ajouter une annonce ou un son
		}
	}

	public void Lancer_de_Couteau(Transform ennemy)
	{
		int range = Range (ennemy);
		if (range <= 5) 
		{
			EnemyHealth ennemy_health = ennemy.GetComponent<EnemyHealth> ();
			ennemy_health.DeductHealth (10);
		} else {
			print ("Vous êtes trop loin");																								//Ajouter une annonce ou un son
		}
	}
}
