using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New FightParameters", menuName = "FightParameters")]
public class FightParameters : ScriptableObject {
	public int allies_number;
	public int ennemies_number;
	public int random_obstacles;
}
