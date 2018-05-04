using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		public GameObject interest_point;
		public GameObject line;
		public List<GameObject> liste_point;
		public List<GameObject> liste_lignes;
		private GameObject map;

		void OnEnable(){
			map = GameObject.FindGameObjectWithTag ("Map");
			create_point ();
			verif_map ();

		}

		//Fonction qui dispere aléatoirement les points d'intérêts sur la map
		private void create_point(){
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
					GameObject nouvpoint = Instantiate (interest_point, new Vector3 (posX, posY, 1), Quaternion.identity, map.transform);
					nouvpoint.name = "Point" + liste_point.Count;
					liste_point.Add (nouvpoint);
					chance -= 0.1f;															//Si un point apparaît les chances d'apparition du suivant sont plus faible
				} else {
					chance += 0.1f;															//S'il n'apparaît pas l'inverse
				}
				cpt_case++;
			}
		}

		private void verif_map(){
			/*liste_point [3].GetComponent<SpriteRenderer> ().flipY = true;
			liste_point [liste_point.Count - 3].GetComponent<SpriteRenderer> ().flipX = true;
			liste_point [liste_point.Count - 3].GetComponent<SpriteRenderer> ().flipY = true;*/

			foreach (GameObject g in liste_point) {
				create_line (g);
			}
		}

		private void create_line(GameObject centre){
			int rayon = 5;
			centre.GetComponent<Collider> ().enabled = false;
			Collider[] liste_colliders = Physics.OverlapSphere (centre.transform.position, rayon);
			while (liste_colliders.Length==0) {
				rayon++;
				liste_colliders = Physics.OverlapSphere (centre.transform.position, rayon);
			}
			centre.GetComponent<Collider> ().enabled = true;


			foreach (Collider c in liste_colliders) {
				if (verif_list_lines ("Line" + c.gameObject.name + centre.name)) {
					creer_ligne (centre, c);
				}
			}
		}

		private bool verif_list_lines(string futur_nom){
			foreach (GameObject line in liste_lignes) {
				if (line.name == futur_nom) {
					return false;
				}
			}
			return true;
		}

		private void creer_ligne(GameObject centre, Collider c){
			GameObject nouvelle_ligne = Instantiate (line, Vector3.zero, Quaternion.identity, map.transform);
			LineRenderer linerenderer = nouvelle_ligne.GetComponent<LineRenderer> ();
			linerenderer.SetPosition (0, centre.transform.position);
			linerenderer.SetPosition (1, c.transform.position);
			linerenderer.name = "Line" + centre.name + c.gameObject.name;
			liste_lignes.Add (linerenderer.gameObject);
			centre.GetComponent<interest_marker_script> ().liste_lignes_proche.Add (nouvelle_ligne);
			c.gameObject.GetComponent<interest_marker_script> ().liste_lignes_proche.Add (nouvelle_ligne);

			nouvelle_ligne.SetActive (true);
		}
	}
}
	
