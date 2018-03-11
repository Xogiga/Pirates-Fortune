using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class S_Event2 : MonoBehaviour {

	public GameObject jaloux;
	public Canvas canvas;
	public Text tinstruction;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player")) 
		{
			Time.timeScale = 0;
			Instantiate (jaloux, new Vector3 (0, 0, 0), Quaternion.identity);

			tinstruction.text = "Catch the Jaloux with \"Spacebar\"";
			Instantiate (canvas);

			Destroy (this.gameObject);
		}
	}

}
