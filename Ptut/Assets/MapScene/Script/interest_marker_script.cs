using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interest_marker_script : MonoBehaviour {
	public List<GameObject> liste_lignes_proche;
	public List<GameObject> liste_points_proche;
	public int couleur;

	void OnEnable() {
		liste_lignes_proche = new List<GameObject>();
		liste_points_proche = new List<GameObject>();
		couleur = 0;
	}

	void OnMouseEnter(){
		Show_Lines ();
	}


	void OnMouseExit(){
		Show_Lines ();
	}

	private void Show_Lines(){
		foreach (GameObject g in liste_lignes_proche) {
			g.SetActive (!g.activeSelf);
		}
	}


}
