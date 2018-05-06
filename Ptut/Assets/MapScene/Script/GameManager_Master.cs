using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapScene {
	public class GameManager_Master : MonoBehaviour {
		public GameObject interest_point;
		public GameObject line;
		public List<GameObject> global_list_point;
		public List<GameObject> global_list_line;
		private GameObject map;

		void OnEnable(){
			Set_Initial_References ();
			Create_Map ();
		}

		private void Set_Initial_References(){
			map = GameObject.FindGameObjectWithTag ("Map");
		}

		//Fonction qui crée la Map
		private void Create_Map(){
			Create_All_Points ();															//Crée les points
			Create_All_Lines ();															//Crée les lignes
			if (Check_Map ()) {																//Vérifie qu'il n'y a pas de point isolé
				print ("Map parfaite");
			}
		}

		//Fonction qui dispere aléatoirement les points d'intérêts sur la map
		private void Create_All_Points(){
			List<Tuple> cases = new List<Tuple> ();

			for (float i = -6; i < 21; i+=3) {												//Crée une liste de case
				for (float j = -2; j < 25; j+=3) {
					cases.Add (new Tuple (i, j));
				}
			}

			int cpt_case = 1;
			float chance = 0.5f;
			foreach(Tuple c in cases){																			//Pour chaque case
				if (cpt_case == 10) {																			//A chaque début de ligne, met la chance d'apparition à 50%
					chance = 0.5f;
					cpt_case = 1;
				}
				if (Random.Range (0f, 1f) < chance) {															//Fait apparaître un point quand le chiffre random est inférieur à la chance d'apparition
					float posX = Random.Range (c.x, c.x + 2);
					float posY = Random.Range (c.y, c.y + 2);
					GameObject nouvpoint = Instantiate (interest_point, new Vector3 (posX, posY, 1), Quaternion.identity, map.transform);
					nouvpoint.name = "Point" + global_list_point.Count;
					global_list_point.Add (nouvpoint);
					chance -= 0.1f;																				//Si un point apparaît les chances d'apparition du suivant sont plus faible
				} else {
					chance += 0.1f;																				//S'il n'apparaît pas l'inverse
				}
				cpt_case++;
			}
		}

		//Fonction qui relie tous les points à leurs voisins
		private void Create_All_Lines(){
			foreach (GameObject point in global_list_point) {													//Pour chaque point créé
				Collider[] collider_list = Find_Neighbours (point);												//Récupère la liste des voisins
				foreach (Collider voisin in collider_list) {													//Pour chaque voisin
					if (Check_All_Lines ("Line" + voisin.gameObject.name + point.name)) {						//Vérifie que la liaison n'existe pas déjà
						Create_Line (point, voisin);															//Si aucune liaison n'existe, on en fait une nouvelle
					}
				}
			}
		}

		//Fonction qui retourne la liste des points voisins d'un point
		private Collider[] Find_Neighbours(GameObject centre){
			int rayon = 5;
			Collider[] collider_list;
			centre.GetComponent<Collider> ().enabled = false;													//Désactive la hitbox du point étudié pour ne pas le détecter

			do {
				collider_list = Physics.OverlapSphere (centre.transform.position, rayon);						//Détecte tous les points autour du centre dans un rayon
				rayon++;
			} while (collider_list.Length == 0);																//Tant que la liste de voisin est vide, agrandi le rayon et détecte à nouveau

			centre.GetComponent<Collider> ().enabled = true;													//Réactive la hitbox du centre

			return collider_list;																				//Retourne la liste de voisin
		}

		//Fonction qui vérifie si une ligne existe déjà où non
		private bool Check_All_Lines(string futur_nom){
			foreach (GameObject line in global_list_line) {
				if (line.name == futur_nom) {																	//Si elle existe déjà
					return false;																				//Retourne False
				}
			}
			return true;
		}

		//Fonction qui crée une ligne entre deux points
		private void Create_Line(GameObject centre, Collider voisin){
			GameObject nouvelle_ligne = Instantiate (line, Vector3.zero, Quaternion.identity, map.transform);	//Crée une ligne qui a pour parent la carte
			LineRenderer linerenderer = nouvelle_ligne.GetComponent<LineRenderer> ();
			linerenderer.SetPosition (0, centre.transform.position);											//On la place de façon à relier les points
			linerenderer.SetPosition (1, voisin.transform.position);
			linerenderer.name = "Line" + centre.name + voisin.gameObject.name;									//On la renomme en fonction des points qu'elle joint
			global_list_line.Add (linerenderer.gameObject);														//On l'ajoute à la liste de toutes les lignes
			centre.GetComponent<interest_marker_script> ().local_list_line.Add (nouvelle_ligne);				//On l'ajoute à la liste des lignes reliés à ces points
			voisin.gameObject.GetComponent<interest_marker_script> ().local_list_line.Add (nouvelle_ligne);
			centre.GetComponent<interest_marker_script> ().local_list_point.Add (voisin.gameObject);			//On ajoute chaque point à la liste des points reliés de l'autre point
			voisin.gameObject.GetComponent<interest_marker_script> ().local_list_point.Add (centre);

			nouvelle_ligne.SetActive (false);																	//On désactive la ligne, pour qu'elle soit invisible
		}

		//Fonction qui vérifie qu'il n'y a pas de point isolé
		private bool Check_Map(){
			int i = 0;

			foreach (GameObject point in global_list_point) {													//Pour chaque point de la map
				if (point.GetComponent<interest_marker_script> ().color == 0) {									//S'il n'a pas de couleur
					i++;																						//Crée une nouvelle couleur
					point.GetComponent<interest_marker_script> ().color = i;									//Colorie le point
					Color_Points (point, point.GetComponent<interest_marker_script> ().color);					//Et colorie tous les points qui lui sont reliés dans cette couleur
				}
			}

			if (i == 1) {																						//S'il n'y a qu'une couleur
				return true;
			}
			print (i + " couleurs !");
			return false;
		}

		//Fonction qui donne une couleur à un point et à tous ses voisins récursivement
		private void Color_Points(GameObject point, int color){
			foreach (GameObject voisin in point.GetComponent<interest_marker_script>().local_list_point) {		//Pour tous les voisins d'un point
				if (voisin.GetComponent<interest_marker_script> ().color != color) {							//Si le voisin à une couleur différente
					voisin.GetComponent<interest_marker_script> ().color = color;								//Le colorie de la même couleur
					Color_Points (voisin, color);																//Et colorie ses voisins à son tour
				}
			}
		}
	}
}
	
