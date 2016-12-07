using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameTutorial : MonoBehaviour {

	Text label;

	void Start () {
		label = GetComponent<Text> ();
		GameplayController.tutorial = this;
		label.enabled = false;
	}

	public void Hide(){
		label.enabled = false;
	}

	public void AvoidSpikes(){
		label.enabled = true;
		label.text = "Bounce now to avoid the spike!";
	}
}
