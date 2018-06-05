﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	public class interest_marker_script : MonoBehaviour {
		public List<GameObject> local_list_line;
		public List<GameObject> local_list_point;
		public GameEvent Event;
		public int index_event;
		public int color;
		public bool reachable;
		public bool done;

		void OnEnable() {
			Set_Initial_References ();
		}

		private void Set_Initial_References(){
			done = false;
			local_list_line = new List<GameObject>();
			local_list_point = new List<GameObject>();
			color = 0;
			reachable = false;
		}

		public Dialogue Get_dialogue(){
			return Event.dialogue;
		}



		void OnMouseEnter(){
			if (References.DialogueManager.popup_open == false) {				//Si une fenetre de dialogue est ouverte, sort de la fonction.
				Show_Lines ();
			}
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
}