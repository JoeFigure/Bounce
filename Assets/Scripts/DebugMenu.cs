using UnityEngine;
using System.Collections;

public class DebugMenu : MonoBehaviour {

	public GameObject mainMenuUI;
	public GameObject debugMenuUI;

	// Use this for initialization
	void Start () {
		debugMenuUI.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DebugModeBttn(){
		mainMenuUI.SetActive (false);
		debugMenuUI.SetActive (true);
	}

	public void HideDebugBttn(){
		debugMenuUI.SetActive (false);
	}
}
