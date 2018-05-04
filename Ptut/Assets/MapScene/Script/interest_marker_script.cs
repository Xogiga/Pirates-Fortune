using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interest_marker_script : MonoBehaviour {
	public List<GameObject> liste_lignes_proche;

	void OnEnable() {
		liste_lignes_proche = new List<GameObject>();
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
