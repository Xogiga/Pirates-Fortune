using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	[CreateAssetMenu(fileName = "New EventList", menuName = "EventList")]
	public class EventList : ScriptableObject {
		public List<GameEvent> eventList;
	}

}
