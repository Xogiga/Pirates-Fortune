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
		private List<PointData> global_data_point;
		public Vector3 playerPos;
		public string startPoint;
		public string endPoint;

		//Fonction qui définit l'objet qui contient les informations de la map au chargement de la scène
		void Awake(){
			CurrentMap = this;
		}

		//Fonction qui sauvegarde les données dans un fichier externe
		public void Save(){
			BinaryFormatter bf = new BinaryFormatter ();											//Permet d'écrire des données dans des fichiers
			FileStream file = File.Create(Application.persistentDataPath + "/MapSave.dat");			//Crée un fichier

			MapData data = new MapData ();															//Crée une classe sérializable


			//data.global_list_line = global_list_line;
			data.global_list_point = Transform_Points_To_Serial();
			data.playerX = playerPos.x;
			data.playerY = playerPos.y;
			data.startPoint = startPoint;
			data.endPoint = endPoint;


			bf.Serialize (file, data);																//Enregistre
			file.Close ();																			//Ferme le fichier
		}

		//Fonction qui transforme les GameObjects en classe sérializable
		private List<PointData> Transform_Points_To_Serial(){
			List<PointData> SerialPoints = new List<PointData> ();

			foreach (GameObject g in global_list_point) {											//Crée une liste de point sérializable
				interest_marker_script object_script = g.GetComponent<interest_marker_script> (); 

				SerialPoints.Add(new PointData(g.transform.position.x,
					g.transform.position.y, object_script.done, object_script.event_name));
			}

			return SerialPoints;
		}

		//Fonction qui charge les données depuis un fichier externe
		public bool Load(){
			if (File.Exists (Application.persistentDataPath + "/MapSave.dat")) {					//Vérifie que le fichier existe
				BinaryFormatter bf = new BinaryFormatter ();										//Permet d'écrire des données dans des fichiers
				FileStream file = File.Open (Application.persistentDataPath + "/MapSave.dat", FileMode.Open);//Ouvre le fichier
				MapData data = (MapData)bf.Deserialize(file);										//Récupère les données dans une classe
				file.Close();

				//global_list_line = data.global_list_line;
				global_data_point = data.global_list_point;
				playerPos = new Vector3 (data.playerX, data.playerY);
				endPoint = data.endPoint;
				startPoint = data.startPoint;

				return true;
			} else {
				return false;
			}
		}

		public List<PointData> Get_global_data_point(){
			return global_data_point;
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

		//Classe qui représente les données à sauvergarder
		[Serializable]
		public class MapData {
			public List<PointData> global_list_point;
			//public List<GameObject> global_list_line;
			public float playerX;
			public float playerY;
			public string startPoint;
			public string endPoint;
		}

	}
}