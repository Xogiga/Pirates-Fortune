using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interest_marker_script : MonoBehaviour {
	public List<GameObject> local_list_line;
	public List<GameObject> local_list_point;
	public int color;

	void OnEnable() {
		local_list_line = new List<GameObject>();
		local_list_point = new List<GameObject>();
		color = 0;
	}

	void OnMouseEnter(){
		Show_Lines ();
	}


	void OnMouseExit(){
		Show_Lines ();
	}

	//Fonction qui active/desactive les lignes reliés au point
	private void Show_Lines(){
		foreach (GameObject g in local_list_line) {
			g.SetActive (!g.activeSelf);
		}
	}
}
