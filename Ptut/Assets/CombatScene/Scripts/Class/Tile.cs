using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile {
	public int state;
	public int x;
	public int y;
	public Tile parent;
	public int gCost;															//Coût qui représente la distance entre cette case et la case de départ
	public int hCost;															//Coût qui représente la distance entre cette case et la case d'arrivée
	public GameObject obj;

	/*Différents état de la variable state des cases :
	 * -1 = Mur
	 * 0 = Vide
	 * 1 = Occupé
	 */

	public Tile(int _x, int _y, int _state){
		x = _x;
		y = _y;
		state = _state;
	}

	public Tile(int _x, int _y){
		x = _x;
		y = _y;
	}

	public int fCost{
		get{
			return gCost + hCost;
		}
	}
}
