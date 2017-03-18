using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Initializer : MonoBehaviour {

	string iOS_APIKey = "VFFHBMJBDRRTNRQHNXRY";

	void Awake() {
		Application.targetFrameRate = 60;
	}

	// Use this for initialization
	void Start () {

		KHD.FlurryAnalytics.Instance.StartSession(iOS_APIKey,"o",false);
	
		InitialiseGame ();

		Time.maximumDeltaTime=0.03f; //<<just over your estimated average frame time.
		//or alternatively:

		//if(Time.deltaTime>0.03){Time.deltaTime=0.03;}//constrain it
	}

	void InitialiseGame(){

		UIManager.instance.ShowLoadingScreen ();
		GameSparksManager.instance.StartCoroutineCheckIfGamesparksAvailable ();
	}
}

