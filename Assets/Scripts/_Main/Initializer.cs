using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour {

	//string iOS_APIKey = "VFFHBMJBDRRTNRQHNXRY";

	void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {

		//KHD.FlurryAnalytics.Instance.StartSession(iOS_APIKey,"o",false);
	
		InitialiseGame ();
	}

	void InitialiseGame(){

		UIManager.instance.ShowLoadingScreen ();
		GameSparksManager.instance.StartCoroutineCheckIfGamesparksAvailable ();
	}
}
