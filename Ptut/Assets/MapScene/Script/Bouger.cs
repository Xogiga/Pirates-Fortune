using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouger : MonoBehaviour {

	public float speed;
	private Rigidbody2D moteur;

	void Start() {
		moteur = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		float x = Input.GetAxis("Horizontal")*speed;
		float y = Input.GetAxis("Vertical")*speed;
		Vector2 vec = new Vector2(x,y);
		moteur.AddForce(vec);
	}
		


}

