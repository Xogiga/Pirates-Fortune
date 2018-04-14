using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogText_Script : MonoBehaviour
{
	public void SetText(string my_text){
		GetComponent<Text>().text = my_text;
	}
}

