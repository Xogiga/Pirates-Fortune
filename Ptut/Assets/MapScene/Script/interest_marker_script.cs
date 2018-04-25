using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interest_marker_script : MonoBehaviour {
	List<GameObject> liste_points;

	void OnEnable() {
		liste_points = null;
	}

	void OnMouseEnter(){
		if (liste_points == null) {
			ChercherPointsProche ();
		}
		ChangerCouleur ();
	}


	void OnMouseExit(){
		if (liste_points != null) {
			foreach (GameObject g in liste_points) {
				g.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 255);
			}
		}
	}


	void ChercherPointsProche(){
		liste_points = new List<GameObject> ();
		int rayon = 5;
		this.GetComponent<Collider> ().enabled = false;
		Collider[] liste_colliders = null;
		while (liste_colliders == null) {
			liste_colliders = Physics.OverlapSphere (this.transform.position, rayon);
			rayon++;
		}
		this.GetComponent<Collider> ().enabled = true;
		if (rayon == 6) {
			foreach (Collider c in liste_colliders) {
				liste_points.Add (c.gameObject);
			}
		} else {
			liste_points.Add (liste_colliders [0].gameObject);
		}
			
	}

	private void ChangerCouleur(){
		foreach (GameObject g in liste_points) {
			g.GetComponent<SpriteRenderer> ().color = new Color32 (255, 255, 255, 50);
		}
	}
}
