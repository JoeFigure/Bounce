using UnityEngine;
using System.Collections;
using GameSparks.Core;
using System.Collections.Generic;

public class GameSparksManager : MonoBehaviour {

	string userName = "Dingo";
	string displayName = "Dingo man";
	string password = "qwerty123";

	UIScript uiScript {
		get {
			GameObject authText = GameObject.Find ("UI");
			return authText.GetComponent<UIScript> ();
		}
	}

	private static GameSparksManager instance = null;
	void Awake() {
		if (instance == null) // check to see if the instance has a reference
		{
			instance = this; // if not, give it a reference to this class...
			DontDestroyOnLoad(this.gameObject); // and make this object persistent as we load new scenes
		} else // if we already have a reference then remove the extra manager from the scene
		{
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		//RegistrationRequest ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RegistrationRequest(){
		new GameSparks.Api.Requests.RegistrationRequest().
		SetDisplayName(displayName).
		SetPassword(password).
		SetUserName(userName).
		Send((response) => {
			if (!response.HasErrors)
			{
				Debug.Log("Player Registered");
			}
			else
			{
				Debug.Log("Error Registering Player");
			}
		});
	}

	public void Authorise(){
		new GameSparks.Api.Requests.AuthenticationRequest().SetUserName(userName).SetPassword(password).Send((response) => {
			if (!response.HasErrors) {
				Debug.Log("Player Authenticated...");
				uiScript.AuthName("your in");

			} else {
				Debug.Log("Error Authenticating Player...");
			}
		});

		//UpdatePlayerName ();
	}

	public void SaveData(){
		new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SAVE_PLAYER").
		SetEventAttribute("GOLD", 100).
		Send((response) => {
			if (!response.HasErrors) {
				Debug.Log("Player Saved To GameSparks...");
				//Load next menu etc here?!
			} else {
				Debug.Log("Error Saving Player Data...");
			}
		});
	}

	public void LoadPlayer(){

		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("LOAD_PLAYER").Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Received Player Data From GameSparks...");
				GSData data = response.ScriptData.GetGSData ("player_Data");
				print ("Player ID: " + data.GetString ("playerID"));
				//print ("Player XP: " + data.GetString ("playerXP"));
				print ("Player Gold: " + data.GetString ("playerGold"));
				//print ("Player Pos: " + data.GetString ("playerPos"));
			} else {
				Debug.Log ("Error Loading Player Data...");
			}
		});

	}


}
