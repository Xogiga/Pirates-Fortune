using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject {
	public new string name;													//Nom de l'évènement
	[TextArea(3,10)]
	public string[] sentences;												//Différents textes du dialogue
}