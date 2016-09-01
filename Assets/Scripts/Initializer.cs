using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
	
		Load ();
		InitialiseGame ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Load(){
		GameManager.instance.Load ();
	}

	void InitialiseGame(){

		if (GameManager.signedIn) {
			GameManager.instance.CurrentState (GameStates.Mainmenu);
		} else {
			GameManager.instance.CurrentState (GameStates.Welcome);
		}
	}
}
