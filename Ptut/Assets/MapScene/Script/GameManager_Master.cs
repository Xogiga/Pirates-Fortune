using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		public GameObject interest_point;

		void OnEnable(){
			create_point ();
		}

		//Fonction qui dispere aléatoirement les points d'intérêts sur la map
		private void create_point(){
			GameObject map = GameObject.FindGameObjectWithTag ("Map");
			List<Tuple> cases = new List<Tuple> ();

			for (float i = -6; i < 21; i+=3) {												//Crée une liste de case
				for (float j = -2; j < 25; j+=3) {
					cases.Add (new Tuple (i, j));
				}
			}

			int cpt_case = 1;
			float chance = 0.5f;
			foreach(Tuple c in cases){														//Pour chaque case
				if (cpt_case == 10) {														//A chaque début de ligne, met la chance d'apparition à 1 sur 2
					chance = 0.5f;
					cpt_case = 1;
				}
				if (Random.Range (0f, 1f) < chance) {										//Fait apparaître un point quand le chiffre random est inférieur à la chance d'apparition
					float posX = Random.Range (c.x, c.x + 2);
					float posY = Random.Range (c.y, c.y + 2);
					Instantiate (interest_point, new Vector3 (posX, posY, 1), Quaternion.identity, map.transform);
					chance -= 0.1f;															//Si un point apparaît les chances d'apparition du suivant sont plus faible
				} else {
					chance += 0.1f;															//S'il n'apparaît pas l'inverse
				}
				cpt_case++;
			}
		}
	}
}
