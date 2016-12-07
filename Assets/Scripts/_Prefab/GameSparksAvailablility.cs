using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameSparksAvailablility : MonoBehaviour {

	public Text availableText;

	public static Color color;

	public static string text;

	void Start () {
		availableText = gameObject.GetComponent<Text> ();
	}

	void Update(){

		availableText.color = color;
		availableText.text = text;
	}

}
