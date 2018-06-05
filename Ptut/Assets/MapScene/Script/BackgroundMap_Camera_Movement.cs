using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MapScene {
	public class BackgroundMap_Camera_Movement : MonoBehaviour {
		private GameObject camera1;

		float taille_cam;

		void OnEnable(){
			camera1 = GameObject.FindWithTag ("MainCamera");
			taille_cam = camera1.GetComponent<Camera> ().orthographicSize = 7;

		}

		//Fonction qui déplace la caméra en fonction des déplacements de la souris en maintenant le clic
		void OnMouseDrag(){

			if (References.DialogueManager.popup_open==true) {													//Si une fenetre de dialogue est ouverte, sort de la fonction.
				return;
			}

			float cam_speed = 0.12f*taille_cam;
			float mouse_movX = cam_speed * Input.GetAxis ("Mouse X");
			float mouse_movY = cam_speed * Input.GetAxis ("Mouse Y");

			Vector3 new_pos_cam = camera1.transform.position - new Vector3 (mouse_movX,mouse_movY, 0);

			float camX = Mathf.Clamp (new_pos_cam.x, 0,15);												//Encadre les valeurs X,Y de la caméra dans les limites de la carte
			float camY = Mathf.Clamp (new_pos_cam.y, 0,22);


			camera1.transform.position = new Vector3 (camX,camY, -10);
		}

		void Update(){
			//zoom et dézoom de la caméra
			if (References.DialogueManager.popup_open==false) {												//Si une fenetre de dialogue est ouverte, sort de la fonction.
				if (Input.GetAxis("Mouse ScrollWheel")!=0) {
					

					taille_cam = camera1.GetComponent<Camera> ().orthographicSize;

					taille_cam -= Input.GetAxis("Mouse ScrollWheel") * 5;
					taille_cam = Mathf.Clamp (taille_cam, 7, 10);											//Encadre la valeur de la taille de la caméra

					camera1.GetComponent<Camera> ().orthographicSize = taille_cam;
				}
			}
		}

	}
}
