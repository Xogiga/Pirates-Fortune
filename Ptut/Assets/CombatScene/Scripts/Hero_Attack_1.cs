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

	public void attack1(Transform ennemy)
	{
		int distance = Mathf.RoundToInt (ennemy.position.x - this.transform.position.x)
		               + Mathf.RoundToInt (ennemy.position.y - this.transform.position.y);
		if (distance <= 1) 
		{
			EnemyHealth ennemy_health = ennemy.GetComponent<EnemyHealth> ();
			ennemy_health.DeductHealth (30);
		}
	}
}
