using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class S_Canvas : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Space)) {
			Destroy (this.gameObject);
		}
		if (Input.anyKey) {
			Destroy (this.gameObject,2f);
		}
	}
}
