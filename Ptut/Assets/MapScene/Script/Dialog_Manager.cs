using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapScene {
	public class Dialog_Manager : MonoBehaviour {
		public Text name_text;
		public Text dialog_text;
		public Animator animator;
		public static Dialog_Manager DialogueManager;
		private Queue<string> sentences = new Queue<string> ();

		void Awake(){
			DialogueManager = this;
		}

		//Fonction qui transmet le dialogue associé à ce GO et appel l'affichage
		public void Choice_Dialogue(GameObject point)
		{
			interest_marker_script point_script = point.GetComponent<interest_marker_script> ();
			Dialogue dialogue = point_script.Get_dialogue ();											//Récupère le dialogue de l'évènement
			if (point_script.done == false) {															//Si l'évènement n'a pas déjà était fait
				Start_Dialogue (dialogue);																//Transmet le dialogue à l'affichage
			} else {																					//Sinon,
				Display_Default_Text (dialogue);														//Transmet le texte par défaut à l'affichage

			}
		}

		//Fonction qui démarre le dialogue
		void Start_Dialogue(Dialogue dialogue){
			animator.SetBool ("isOpen", true);
			name_text.text = dialogue.name;

			sentences.Clear ();

			foreach (string sentence in dialogue.sentences) {
				sentences.Enqueue (sentence);
			}

			DisplayNextSentence ();
		}

		//Fonction qui affiche la phrase suivante
		void DisplayNextSentence(){
			if (sentences.Count == 0) {
				EndDialogue ();
				return;
			}
			string next_sentence = sentences.Dequeue ();
			StopAllCoroutines ();
			StartCoroutine (Type_Sentences(next_sentence));
		}

		//Fonction qui affiche le texte par défaut pour les visities suivantes
		void Display_Default_Text(Dialogue dialogue){
			name_text.text = dialogue.name;
			animator.SetBool ("isOpen", true);
			StopAllCoroutines ();
			StartCoroutine (Type_Sentences(dialogue.default_text));
		}

		//Routine qui affiche les lettres une par une
		IEnumerator Type_Sentences(string sentence){
			dialog_text.text = "";
			foreach (char letter in sentence.ToCharArray()) {
				dialog_text.text += letter;
				yield return 10;
			}
		}

		void EndDialogue(){
			animator.SetBool ("isOpen", false);
			GameManager_Master.GameMaster.Execute_Event ();
		}
	}
}
