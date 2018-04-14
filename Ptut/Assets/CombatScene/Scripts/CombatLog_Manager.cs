using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombatLog_Manager : MonoBehaviour {
	[SerializeField]
	private Transform log_text;
	[SerializeField]
	private GameObject text_prefab;

	private List<GameObject> text_items;

	public void OnEnable(){
		text_items = new List<GameObject> ();
	}

	//Fonction qui ajoute un élément de text dans la liste	
	public void Add_Text(string new_text, int type){
		if (text_items.Count == 50) {																		//Vérifie qu'il n'y a pas plus de 50 éléments
			Destroy(text_items[0].gameObject);
			text_items.RemoveAt (0);
		}

		GameObject new_item = Instantiate (text_prefab, log_text);											//Crée un nouveau GO texte, fils du log de texte

		string final_text = Identify_Type_Of_Message (new_text, type);
		new_item.GetComponent<LogText_Script> ().SetText (final_text);										//Assigne le texte et la couleur de ce GO

		text_items.Add (new_item);
	}

	//Fonction qui détermine le type de message à afficher
	private string Identify_Type_Of_Message(string text, int type){
		string final_text;
		switch (type) {
		case 0:
			final_text = "Announce : " + text;
			break;
		default:
			final_text = text;
			break;
		}

		return final_text;
	}
}
