using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class InternetAvailable : MonoBehaviour {

	public Text availableText;

	public Image onlineSignalImage;

	public static Color color;

	public static string text;

	void Update(){

		onlineSignalImage.color = color;
		availableText.text = text;
	}
}
