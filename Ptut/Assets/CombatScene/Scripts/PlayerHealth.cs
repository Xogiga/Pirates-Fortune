using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	private float health_stat = 100;
	private float current_health;
	public Text health_text;
	public Image health_bar;

	void OnEnable()
	{
		SetInitialReferences ();
	}

	void SetInitialReferences(){
		current_health = health_stat;
		SetUI ();
	}

	void Start () {
		StartCoroutine (test ());
	}

	void IncreaseHealth(int health_change)
	{
		current_health += health_change;
		if (current_health > health_stat) {
			current_health = health_stat;
		}
		SetUI ();
		StartCoroutine (IncreaseBar());
	}

	void DeductHealth(int health_change){
		current_health -= health_change;
		if (current_health < 0) {
			current_health = 0;
		}
		SetUI ();
		StartCoroutine (DecreaseBar());
	}

	void SetUI(){
		health_text.text = current_health.ToString ();
	}
		

	IEnumerator test(){
		yield return new WaitForSeconds(2);
		DeductHealth(60);
		yield return new WaitForSeconds(2);
		IncreaseHealth(30);
	}

	IEnumerator IncreaseBar(){
		while (health_bar.fillAmount <= current_health / health_stat) {
			yield return new WaitForSeconds (Time.deltaTime);
			health_bar.fillAmount += 0.01f;
		}
		health_bar.fillAmount = current_health / health_stat;
	}

	IEnumerator DecreaseBar(){
		while (health_bar.fillAmount >= current_health / health_stat) {
			yield return new WaitForSeconds (Time.deltaTime);
			health_bar.fillAmount -= 0.01f;
		}
		health_bar.fillAmount = current_health / health_stat;  			// Les calculs sur les floats merdent toujours, donc je lui redonne la valeur exacte à la fin
	}

}
