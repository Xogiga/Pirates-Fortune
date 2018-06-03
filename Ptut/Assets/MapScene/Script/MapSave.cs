using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


namespace MapScene {

	public class MapSave : MonoBehaviour {

		public static MapSave CurrentMap;

		public List<GameObject> global_list_point;
		public List<GameObject> global_list_line;
		public List<PointData> global_data_point;
		public List<LineData> global_data_line;
		public GameObject playerPos;
		public int playerPos_data;
		public GameObject startPoint;
		public int startPoint_data;
		public GameObject endPoint;
		public int endPoint_data;


		//Fonction qui définit l'objet qui contient les informations de la map au chargement de la scène
		void Awake(){
			CurrentMap = this;
		}

		//Fonction qui sauvegarde les données dans un fichier externe
		public void Save(){
			BinaryFormatter bf = new BinaryFormatter ();											//Permet d'écrire des données dans des fichiers
			FileStream file = File.Create(Application.persistentDataPath + "/MapSave.dat");			//Crée un fichier

			MapData data = new MapData ();															//Crée une classe sérializable


			data.global_list_line = Transform_Lines_To_Serial();
			data.global_list_point = Transform_Points_To_Serial();
			data.playerPos = Transform_Name_To_Int(playerPos);
			data.startPoint_data = Transform_Name_To_Int(startPoint);
			data.endPoint_data = Transform_Name_To_Int(endPoint);


			bf.Serialize (file, data);																//Enregistre
			file.Close ();																			//Ferme le fichier
		}

		//Fonction qui transforme les GameObjects Point en classe sérializable
		private List<PointData> Transform_Points_To_Serial(){
			List<PointData> SerialPoints = new List<PointData> ();

			foreach (GameObject g in global_list_point) {											//Crée une liste de point sérializable
				interest_marker_script object_script = g.GetComponent<interest_marker_script> (); 

				SerialPoints.Add(new PointData(g.transform.position.x,
					g.transform.position.y, object_script.done, object_script.event_name));
			}

			return SerialPoints;
		}

		//Fonction qui transforme les GameObjects Line en classe sérializable
		private List<LineData> Transform_Lines_To_Serial(){
			List<LineData> SerialLines = new List<LineData> ();

			foreach (GameObject line in global_list_line) {											//Crée une liste de point sérializable
				
				int index1 = int.Parse(line.name.Substring (9, 2));
				int index2 = int.Parse(line.name.Substring(16,2));
				SerialLines.Add(new LineData(index1,index2));				//Crée un DataLine avec les deux indices contenus dans le nom
			}

			return SerialLines;
		}


		//Fonction qui change les noms des points en index de la liste
		private int Transform_Name_To_Int(GameObject point){
			string stringIndex = point.name.Substring (5);											//Supprime les 5 premiers caractères d'un string
			return int.Parse(stringIndex);															//Retourne l'index en int
		}

		//Fonction qui charge les données depuis un fichier externe
		public bool Load(){
			if (File.Exists (Application.persistentDataPath + "/MapSave.dat")) {					//Vérifie que le fichier existe
				BinaryFormatter bf = new BinaryFormatter ();										//Permet d'écrire des données dans des fichiers
				FileStream file = File.Open (Application.persistentDataPath + "/MapSave.dat", FileMode.Open);//Ouvre le fichier
				MapData data = (MapData)bf.Deserialize(file);										//Récupère les données dans une classe
				file.Close();

				global_data_line = data.global_list_line;
				global_data_point = data.global_list_point;											//Recupère la liste de point en PointData
				playerPos_data = data.playerPos;														//Récupère la position du joueur en Vector3
				endPoint_data = data.endPoint_data;													//Récupère l'indice du point d'arrivée
				startPoint_data = data.startPoint_data;												//Et de départ

				return true;
			} else {
				return false;
			}
		}



		//Classe qui représente les données à sauvergarder
		[Serializable]
		public class MapData {
			public List<PointData> global_list_point;
			public List<LineData> global_list_line;
			public int playerPos;
			public int startPoint_data;
			public int endPoint_data;
		}

		//Classe qui représente les données à sauvergarder
		[Serializable]
		public class PointData {
			public float posX;
			public float posY;
			public bool done;
			public string eventName;

			public PointData(float x, float y, bool d, string e){					//Constructeur
				posX = x;
				posY = y;
				done = d;
				eventName =e ;
			}
		}

		[Serializable]
		public class LineData {
			public int index1;														//Indice du premier point relié dans la liste globale
			public int index2;														//Second indice

			public LineData(int i1, int i2){										//Constructeur
				index1 = i1;
				index2 = i2;
			}
		}

	}
}