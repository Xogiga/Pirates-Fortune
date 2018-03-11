using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {


	void Update () {
		if (Input.GetKey (KeyCode.Space)) {
			Destroy (this.gameObject);
			Time.timeScale = 1;
		}
	}
}
