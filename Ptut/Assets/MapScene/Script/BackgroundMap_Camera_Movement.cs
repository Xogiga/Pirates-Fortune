using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMap_Camera_Movement : MonoBehaviour {
	private GameObject camera1;

	void OnEnable(){
		camera1 = GameObject.FindWithTag ("MainCamera");
	}	

	//Fonction qui déplace la caméra en fonction des déplacements de la souris en maintenant le clic
	void OnMouseDrag(){
		float cam_speed = 0.5f;
		float mouse_movX = cam_speed * Input.GetAxis ("Mouse X");
		float mouse_movY = cam_speed * Input.GetAxis ("Mouse Y");

		Vector3 new_pos_cam = camera1.transform.position - new Vector3 (mouse_movX,mouse_movY, 0);

		float camX = Mathf.Clamp (new_pos_cam.x, 0,15);												//Encadre les valeurs X,Y de la caméra dans les limites de la carte
		float camY = Mathf.Clamp (new_pos_cam.y, 0,22);

		camera1.transform.position = new Vector3 (camX,camY, -10);
	}

}
