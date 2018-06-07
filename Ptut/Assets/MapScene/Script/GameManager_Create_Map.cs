using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MapScene {
	public class GameManager_Create_Map : MonoBehaviour {
		[Header("Prefabs :")]
		public GameObject interest_point;
		public GameObject line;
		public GameObject ship;

		[Header("Map Sprites :")]
		public Sprite start_sprite;
		public Sprite end_sprite;
		public Sprite marine_sprite;

		private GameObject map;
		private List<GameObject> global_list_point;
		private List<GameObject> global_list_line;
		private List<List<GameObject>> colors_list;
		private List<GameEvent> all_event;
		private GameObject start_point;
		private GameObject end_point;
		private GameObject player_position;
		private List<GameObject> visited_points;
		private List<GameObject> marine_territory;

		[SerializeField]
		private int turn;

		void OnEnable(){
			Set_Initial_References ();
		}

		private void Set_Initial_References(){
			EventList EventData = (EventList)AssetDatabase
				.LoadAssetAtPath("Assets/MapScene/Event/AllEventList.asset", typeof(EventList));//Charge l'object qui contient la list des évènement
			all_event = EventData.eventList;													//Copie la liste de tous les évènements possibles

			map = GameObject.FindGameObjectWithTag ("Map");
			global_list_point = new List<GameObject> ();
			global_list_line = new List<GameObject> ();
			visited_points = new List<GameObject> ();
			marine_territory = new List<GameObject> ();
			turn = 0;
		}

		//Fonction qui crée la Map
		public void Create_Map(){
			if (References.SaveMapScript.Load() == false) {										//S'il n'y a rien a charger
				Create_All_Points ();															//Crée les points
				Choose_Start_End ();															//Détermine le debut et l'arrivée
				Create_All_Lines ();															//Crée les lignes
				colors_list = Check_Map ();														//Fais des colorations sur le graphe de le map
				if (colors_list != null) {														//S'il y a plusieurs couleurs
					Correct_Map (colors_list);													//Corrige la map
				}
				Instantiate_Ship ();															//Crée le bateau
				start_point.GetComponent<interest_marker_script>().done = true;
				Add_Visited_Points(start_point);												//Grise le point de départ
				Attribute_All_Events();															//Détermine tous les events
			} else {
				turn = References.SaveMapScript.turn ;											
				Recreate_Previous_Map ();
			}

			Center_Camera ();																	//Centre la caméra
			Draw_Point ();																		//Change les sprites selon les points
			SaveToData();																		//Envoie toutes les données au script de sauvegarde
			References.GameMaster.Interact_With_Interest_Point(player_position, true);			//Donne le point actuel au gameMaster
		}

		//Fonctions qui attribue un évènement à chaque point
		private void Attribute_All_Events(){
			Set_One_Event (start_point, 0);
			Set_One_Event (end_point, 1);

			foreach (GameObject point in global_list_point) {
				if(point != start_point && point != end_point){
					int random_index = Random.Range (3,all_event.Count);
					Set_One_Event (point, random_index);
				}
			}
		}

		//Fonctions qui attribue un évènement à chaque point
		private void Set_One_Event(GameObject point, int event_index){
			interest_marker_script point_script = point.GetComponent<interest_marker_script> ();
			point_script.Set_Event (all_event [event_index], event_index);
		}

		//Envoie toutes les informations au script qui contient la sauvegarde
		private void SaveToData(){
			References.SaveMapScript.global_list_line = global_list_line;
			References.SaveMapScript.global_list_point = global_list_point;							//Sauvegarde tous les points
			References.SaveMapScript.startPoint = start_point;										//Enregistre le point de départ
			References.SaveMapScript.endPoint = end_point;											//Enregistre le point d'arrivée
			References.SaveMapScript.playerPos = player_position;									//Enregistre sa position actuel
		}

		//Fonction qui recrée la map à partir du fichier de sauvegarde
		private void Recreate_Previous_Map(){
			foreach (MapSave.PointData d in References.SaveMapScript.global_data_point) {			//Recrée tous les points de la map
				Create_One_Point(d.posX, d.posY);
				GameObject last_point = global_list_point [global_list_point.Count - 1];
				interest_marker_script last_script = last_point.GetComponent<interest_marker_script>();//Récupère le script du dernier point créé
				last_script.index_event = d.event_index;
				last_script.done = d.done;
				last_script.index_event = d.event_index;
				if (d.done == true) {																//Si le joueur a déjà fait l'évènement
					visited_points.Add(global_list_point[global_list_point.Count -1]);				//Ajoute le point à ceux visités
				}
				Set_One_Event (last_point, last_script.index_event);								//Réattribue les évènements
				if (last_script.Event.EventName == "Marine") {										//Si l'évènement est celui de la marine, l'ajoute à la liste
					last_script.marine = true;
					last_script.done = false;
					marine_territory.Add (last_point);
				}
			}

			foreach (MapSave.LineData l in References.SaveMapScript.global_data_line) {
				ReCreate_Line (l.index1, l.index2);
			}


			player_position = 
				global_list_point[References.SaveMapScript.playerPos_data];							//Replace le joueur	
				Instantiate_Ship();																	//Crée le bateau

			//Recupère les indices des points de départ et d'arrivé puis les enregistre à nouveau
			start_point = global_list_point [References.SaveMapScript.startPoint_data];				//Définit le start_point à partir de l'indice
			end_point = global_list_point[References.SaveMapScript.endPoint_data];					//Définit le end-point à partir de l'indice
		}
			

		//Recrée toutes les lignes à partir des données enregistrées
		private void ReCreate_Line(int index1, int index2){
			GameObject point1 = global_list_point [index1];
			GameObject point2 = global_list_point [index2];
			GameObject nouvelle_ligne = Instantiate (line, Vector3.zero, Quaternion.identity, map.transform);	//Crée une ligne qui a pour parent la carte
			LineRenderer linerenderer = nouvelle_ligne.GetComponent<LineRenderer> ();
			linerenderer.SetPosition (0, point1.transform.position);											//On la place de façon à relier les points
			linerenderer.SetPosition (1, point2.transform.position);
			linerenderer.name = "Line" + point1.name + point2.name;												//On la renomme en fonction des points qu'elle joint
			global_list_line.Add (linerenderer.gameObject);														//On l'ajoute à la liste de toutes les lignes

			interest_marker_script script1 = point1.GetComponent<interest_marker_script> ();
			interest_marker_script script2 = point2.GetComponent<interest_marker_script> ();

			script1.local_list_line.Add (nouvelle_ligne);														//On l'ajoute à la liste des lignes reliés à ces points
			script2.local_list_line.Add (nouvelle_ligne);
			script1.local_list_point.Add (point2);																//On ajoute chaque point à la liste des points reliés de l'autre point
			script2.local_list_point.Add (point1);

			nouvelle_ligne.SetActive (false);				
		}

		//Fonction qui dispere aléatoirement les points d'intérêts sur la map
		private void Create_All_Points(){
			List<Tuple> cases = new List<Tuple> ();

			for (float i = -6; i < 21; i+=3) {																	//Crée une liste de case
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
					Create_One_Point (posX, posY);																//Créer le gameobject
					chance -= 0.1f;																				//Si un point apparaît les chances d'apparition du suivant sont plus faible
				} else {
					chance += 0.1f;																				//S'il n'apparaît pas l'inverse
				}
				cpt_case++;
			}
		}

		//Fonction qui créer un GO point à une position donnée
		private void Create_One_Point(float posX,float posY){
			GameObject nouvpoint = Instantiate (interest_point, new Vector3 (posX, posY, 1), Quaternion.identity, map.transform);
			nouvpoint.name = "Point" + global_list_point.Count.ToString("00");
			global_list_point.Add (nouvpoint);
		}

		//Fonction qui détermine les points de départ et d'arrivé en prenant ceux les plus proches des angles de la carte
		private void Choose_Start_End(){
			Vector3 StartAngle;
			Vector3 FinishAngle;
			start_point = null;
			end_point = null;

			int rng = Random.Range (1, 5);
			switch (rng)																						//Choisi l'angle de départ et d'arrivé
			{
			case 1:
				StartAngle = new Vector3 (-7, 25, 0);															//Haut Gauche
				FinishAngle = new Vector3 (21,-3, 0);															//Bas Droite
				break;
			case 2:
				StartAngle = new Vector3 (21, 25, 0);															//Haut Droite
				FinishAngle = new Vector3 (-7,-3, 0);															//Bas Gauche
				break;
			case 3:
				StartAngle = new Vector3 (21,-3, 0);															//Bas Droite
				FinishAngle = new Vector3 (-7, 25, 0);															//Haut Gauche
				break;
			default:
				StartAngle = new Vector3 (-7,-3, 0);															//Bas Gauche
				FinishAngle = new Vector3 (21, 25, 0);															//Haut Droite
				break;
			}

			int radius = 5;
			Collider[] list_neighbourghs;
			while (start_point==null){																			//Trouve un point proche de l'angle
				list_neighbourghs = Physics.OverlapSphere (StartAngle,radius);	
				if (list_neighbourghs.Length > 0) {
					start_point = list_neighbourghs [0].gameObject;
				} else {
					radius++;
				}
			}
			radius = 5;
			while (end_point==null){
				list_neighbourghs = Physics.OverlapSphere (FinishAngle,radius);	
				if (list_neighbourghs.Length > 0) {
					end_point = list_neighbourghs [0].gameObject;
				} else {
					radius++;
				}
			}
		}

		//Fonction qui change le logo et la couleur des points d'intérêt selon leur type
		private void Draw_Point(){
			start_point.GetComponent<SpriteRenderer> ().sprite = start_sprite;									//Change les sprites d'arrivés et de départ												
			end_point.GetComponent<SpriteRenderer> ().sprite = end_sprite;
			foreach (GameObject p in visited_points) {															//Grise les points visités
				p.GetComponent<SpriteRenderer> ().color = new Color(1,1,1,0.5f);
			}
			Draw_Marine_Waves ();
		}

		//Fonction qui change le logo et la couleur des points d'intérêt selon leur type
		private void Draw_Marine_Waves(){
			foreach (GameObject p in marine_territory) {														//Pour chaque point sous contrôle de la marine
				SpriteRenderer SP = p.GetComponent<SpriteRenderer> ();
				SP.sprite = marine_sprite;																		//Donne le logo de la marine
				SP.color = new Color(1,1,1,1);																	//"Dégrise" le point
			}

		}

		//Fonction qui permet de griser le dernier point visité
		public void Add_Visited_Points(GameObject point){
			visited_points.Add(point);
			point.GetComponent<SpriteRenderer> ().color = new Color(1,1,1,0.5f);
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

		//Fonction qui vérifie qu'il n'y a pas de point isolé et les listes de point par coloration
		private List<List<GameObject>> Check_Map(){
			int i = 0;
			List<List<GameObject>> colors_list = new List<List<GameObject>>();									//Liste de liste de chaque couleur de point (Les bleus, les rouges..)

			foreach (GameObject point in global_list_point) {													//Pour chaque point de la map
				if (point.GetComponent<interest_marker_script> ().color == 0) {									//S'il n'a pas de couleur
					i++;																						//Crée une nouvelle couleur
					point.GetComponent<interest_marker_script> ().color = i;									//Colorie le point
					List<GameObject> current_color_list = new List<GameObject>();								//Crée la liste de cette couleur
					Color_Points (point, i, current_color_list);												//Et colorie tous les points qui lui sont reliés dans cette couleur
					colors_list.Add(current_color_list);														//Ajoute la nouvelle liste de couleur aux couleurs
				}
			}

			if (i == 1) {																						//S'il n'y a qu'une couleur
				return null;
			}
			return colors_list;
		}

		//Fonction qui donne une couleur à un point et à tous ses voisins récursivement
		private void Color_Points(GameObject point, int color, List<GameObject> current_color_list){
			current_color_list.Add (point);																		//Ajoute le point à la liste de cette couleur
			foreach (GameObject voisin in point.GetComponent<interest_marker_script>().local_list_point) {		//Pour tous les voisins d'un point
				if (voisin.GetComponent<interest_marker_script> ().color != color) {							//Si le voisin à une couleur différente
					voisin.GetComponent<interest_marker_script> ().color = color;								//Le colorie de la même couleur
					Color_Points (voisin, color,current_color_list);											//Et colorie ses voisins à son tour
				}
			}
		}

		//Fonction qui lient toutes les groupes de point entre eux
		private void Correct_Map(List<List<GameObject>> colors_list){
			List<GameObject> smallest_list = null;
			int new_color;

			while (colors_list.Count > 1) {																		//Tant qu'il y a plus d'une couleur
				foreach (List<GameObject> L in colors_list) {													//On commence par déterminer la plus petite couleur
					if (smallest_list == null || L.Count < smallest_list.Count) {
						smallest_list = L;
					}
				}

				colors_list.Remove (smallest_list);																//On supprime cette couleur de la liste de couleur
				new_color = Search_Further (smallest_list);														//On détermine la couleur la plus proche
				Change_Color(smallest_list,new_color);															//On change la plus petite liste dans cette nouvelle couleur


				foreach (List<GameObject> L in colors_list) {													//On cherche parmi les liste celle à fusionner											
					if (L [0].GetComponent<interest_marker_script> ().color == new_color) {						//Si la couleur est la même
						L.AddRange (smallest_list);																//On ajoute la plus petite liste à celle la plus proche
					}
				}
				smallest_list = null;
			}


		}

		//Fonction qui cherche le premier voisin d'une autre couleur et retourne cette couleur
		private int Search_Further(List<GameObject> current_list){
			float shortest_distance = 100;
			Collider close_point = null;
			GameObject friendly_point = null;
			float distance;
			Collider[] collider_list;

			int range;

			foreach (GameObject point in current_list) {
				point.GetComponent<Collider> ().enabled = false;													//Désactive les hitbox de tous les points de cette couleur
			}

			foreach (GameObject point in current_list) {															//Pour tous les points de cette couleur
				range = 6;
				collider_list = null;
				do {
					collider_list = Physics.OverlapSphere (point.transform.position, range);						//Détecte tous les points autour du centre dans un rayon
					foreach(Collider neighbours in collider_list){
						distance = Vector3.Distance(point.transform.position, neighbours.transform.position);		//Trouve la distance entre ces points
						if(shortest_distance > distance){															//Si cette distance est la plus courte trouvé jusqu'à présent
							shortest_distance = distance;															//Définit cette distance comme la plus courte
							close_point = neighbours;																//Ce point voisin comme le plus proche
							friendly_point = point;																	//Et ce point comme le plus en bordure
						}
					}
					range++;
				} while (collider_list == null || collider_list.Length ==0);										//Tant qu'il n'a pas trouvé un point d'une autre couleur augmante la portée de détection
			}

			if (friendly_point != null) {
				Create_Line (friendly_point, close_point);															//Créer une ligne entre les deux points de couleurs différentes trouvés
			} 


			foreach (GameObject point in current_list) {
				point.GetComponent<Collider> ().enabled = true;														//On réactive les hitbox de tous les points de cette couleur
			}

			return close_point.gameObject.GetComponent<interest_marker_script>().color;								//Retourne la couleur de la liste qui a été rejointe
		}

		//Fonction qui place le bateau au point de départ
		private void Instantiate_Ship(){
			if (player_position == null) {
				player_position = start_point;
			}
			Instantiate (ship, player_position.transform.position + new Vector3 (1, 0, 0), Quaternion.identity);
		}

		//Fonction qui place la caméra sur le joueur au démarage
		public void Center_Camera(){
			GameObject camera1 = GameObject.FindWithTag ("MainCamera");
			float camX = Mathf.Clamp (player_position.transform.position.x, 0,15);									//Encadre les valeurs X,Y de la caméra dans les limites de la carte
			float camY = Mathf.Clamp (player_position.transform.position.y, 0,22);
			camera1.transform.position =  new Vector3 (camX,camY, -10);

		}

		//Change la couleur d'une liste
		private void Change_Color(List<GameObject> list, int new_color){
			foreach (GameObject g in list) {
				g.GetComponent<interest_marker_script> ().color = new_color;
			}
		}

		//Fonction qui ajoute un tour à chaque déplacement
		public void Add_Turn(){
			turn = turn + 1;
			if (turn%2 == 0 && turn > 5 || turn == 3) {																//Si le tour est le tour 3 ou s'il est un multiple de deux (modulo = 0)
				New_Waves(turn);																					//Ajoute une vague de marine
			}
			References.SaveMapScript.turn = turn;																	//Enregistre le tour
		}

		//Fonction qui avance la vague de la Marine
		public void New_Waves(int t){
			if (t == 3) {																							//Au troisième tour
				Set_One_Event (start_point, 2);																		//Arrivé de la marine sur le point de départ
				marine_territory.Add (start_point);
			}
			else if (t > 5) {																						//Après le tour 5, tous les tour pair
				List<GameObject> temporary_list = new List<GameObject> ();
				foreach (GameObject p in marine_territory) {														//Pour toutes les îles de la marines
					interest_marker_script script_point = p.GetComponent<interest_marker_script> ();
					foreach(GameObject neighbourg in script_point.local_list_point){								//Colonise toutes les îles voisines
						interest_marker_script script_neighbourg = neighbourg.GetComponent<interest_marker_script> ();
						if (script_neighbourg.marine == false) {													//Qui ne sont pas déjà de la marine
							Set_One_Event (neighbourg, 2);
							temporary_list.Add (neighbourg);
						}
					}
				}
				marine_territory.AddRange(temporary_list);															//Ajoute les dernières îles à la liste totale
			}
			Draw_Marine_Waves ();																					//Change les sprites des marqueurs
		}
	}
}

