using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog_Manager : MonoBehaviour {
	public Text name_text;
	public Text dialog_text;
	public Animator animator;

	private Queue<string> sentences = new Queue<string> ();

	//Fonction qui démarre le dialogue
	public void Start_Dialogue(Dialogue dialogue){
		animator.SetBool ("isOpen", true);
		name_text.text = dialogue.name;

		sentences.Clear ();

		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue (sentence);
		}

		DisplayNextSentence ();
	}

	//Fonction qui affiche la phrase suivante
	public void DisplayNextSentence(){
		if (sentences.Count == 0) {
			EndDialogue ();
			return;
		}
		string next_sentence = sentences.Dequeue ();
		StopAllCoroutines ();
		StartCoroutine (Type_Sentences(next_sentence));
	}

	//Routine qui affiche les lettres une par une
	IEnumerator Type_Sentences(string sentence){
		dialog_text.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			dialog_text.text += letter;
			yield return 10;
		}
	}

	private void EndDialogue(){
		animator.SetBool ("isOpen", false);
	}
}
