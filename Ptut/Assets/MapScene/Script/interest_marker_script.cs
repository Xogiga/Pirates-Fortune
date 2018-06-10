using System.Collections;
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
		public bool marine;
		private bool active_lines;

		void OnEnable() {
			Set_Initial_References ();
		}

		private void Set_Initial_References(){
			done = false;
			local_list_line = new List<GameObject>();
			local_list_point = new List<GameObject>();
			color = 0;
			reachable = false;
			active_lines = false;
			marine = false;
		}

		public Dialogue Get_dialogue(){
			return Event.dialogue;
		}
			
		public void Set_Event(GameEvent e, int index){
			this.Event = e;
			this.index_event = index;
			if (e.EventName == "Marine") {
				this.marine = true;
				this.done = false;
			}
		}

		void OnMouseEnter(){
			if (References.DialogueManager.popup_open == false) {				//Vérifie qu'aucun dialogue n'est en cours
				Show_Lines();
			}
		}


		void OnMouseExit(){
			if (References.DialogueManager.popup_open == false ||				//Vérifie qu'aucun dialogue n'est en cours
				active_lines == true) {											//Et que les lignes ne sont actives
				Show_Lines();
			}
		}

		//Fonction qui active/desactive les lignes reliés au point
		private void Show_Lines(){
			active_lines = !active_lines;
			foreach (GameObject g in local_list_line) {
				g.SetActive (!g.activeSelf);
			}
		}
	}
}
