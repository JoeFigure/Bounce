using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {
	
		InitialiseGame ();
	}

	void InitialiseGame(){

		UIManager.instance.ShowLoadingScreen ();
		//GameSparksManager.instance.StartCoroutineCheckIfGamesparksAvailable ();
		GameManager.instance.CurrentState (GameStates.Init);
	}
}
