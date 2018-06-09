using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
	public class GameEvent : ScriptableObject {
		public string EventName;												//Nom de l'évènement
		public Dialogue dialogue;												//Dialogue de l'évènement
		public bool is_unique;													//Nombre d'occurence possible de l'évènement
		public bool Involve_Fight;												//Bool qui permet de lancer un combat ou non
		public FightParameters fp;
	}

}