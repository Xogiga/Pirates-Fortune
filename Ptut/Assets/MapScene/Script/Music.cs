using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {

	public AudioSource musicSource;
	public AudioSource musicSource2;
	public AudioClip[] music;

	private bool event2 = true;


	public void player()
	{
		int rand = Random.Range (0, 2);
		musicSource.clip = music[rand];
		musicSource.Play();
	}

	public void mcontinue()
	{
		musicSource.UnPause ();
	}
	public void stopplayer()
	{
		musicSource.Pause();
	}

	public void playevent2()
	{
		musicSource2.clip = music [3];
		musicSource2.Play();
		event2 = false;
	}

	// Use this for initialization
	void Start () {
		player ();
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("Jaloux_avatar(Clone)")) 
		{
			if (event2 == true) {
				stopplayer ();
				playevent2 ();
			}
		}
		if (Input.GetKey (KeyCode.Space)) {
			mcontinue ();
		}
	}
}
