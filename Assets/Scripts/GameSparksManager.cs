using UnityEngine;
using System.Collections;
using GameSparks.Core;
using System.Collections.Generic;

public class GameSparksManager : MonoBehaviour {


	UIManager uiScript {
		get {
			GameObject authText = GameObject.Find ("UIManager");
			return authText.GetComponent<UIManager> ();
		}
	}

	string highScoreText {
		get{ return uiScript.highScoreString; }
		set{ uiScript.highScoreString = value; }
	}

	string usernameLogin {
		get{ return uiScript.usernameLoginString; }
	}
	string passwordLogin {
		get{ return uiScript.passwordLoginString; }
	}

	string usernameRegister {
		get{ return uiScript.usernameRegisterString; }
	}
	string passwordRegister {
		get{ return uiScript.passwordRegisterString; }
	}
	string displaynameRegister {
		get{ return usernameRegister + "1" ; }
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

		GS.GameSparksAvailable += HandleGameSparksAvailable;

		InvokeNotifications ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//not call if no internet connectivity
	private void HandleGameSparksAvailable (bool value)
	{
		//what is meaning of the bool parameter ???
		Debug.Log(value);
		uiScript.AvailabilityOfGameSparksNotification (value);
	}
	/*
	void GameSparksConnected(){

		new GameSparks.Api.Requests.LogEventRequest ()
		.SetEventKey ("REQUEST")
		.SetDurable(true)
		.Send (response => {
			if (response.HasErrors) {
				Debug.Log ("Event Request ERROR!!!");
					uiScript.SignedIntoGameSparksNotification (false);


			} else {
				//Debug.Log ("Event Request OK");
					uiScript.SignedIntoGameSparksNotification (true);
					GetZoins();
					CancelInvoke();

			}   
		}); 
	}
*/
	void OnlineNotification() {
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			uiScript.InternetAccessNotification (true);
			//Debug.Log("wifi");
		} else {
			uiScript.InternetAccessNotification (false);
			Debug.Log("no wifi");
		}
	}

	void InvokeNotifications() {
		InvokeRepeating("OnlineNotification", 0.2f, 2);
		//InvokeRepeating("GameSparksConnected", 2, 2);
	}

	public void RegistrationRequest(){
		new GameSparks.Api.Requests.RegistrationRequest().
		SetDisplayName(displaynameRegister).
		SetPassword(passwordRegister).
		SetUserName(usernameRegister).
		Send((response) => {
			if (!response.HasErrors)
			{
				Debug.Log("Player Registered");
				uiScript.usernameMainMenuString = usernameRegister;
				ManualReset(3);
				UIManager.instance.MenuUI();

			}
			else
			{
				Debug.Log("Error Registering Player");
			}
		});
	}


	public void Authorise(){

		new GameSparks.Api.Requests.AuthenticationRequest().
		SetUserName(usernameLogin).
		SetPassword(passwordLogin).
		Send((response) => {
			if (!response.HasErrors) {
				Debug.Log("Player Authenticated...");
				uiScript.usernameMainMenuString = usernameLogin;
				UIManager.instance.MenuUI();

				GameManager.signedIn = true;
				//CancelInvoke();

			} else {
				Debug.Log("Error Authenticating Player...");
			}
		});
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

		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("LOAD_PLAYER").
		Send ((response) => {
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
		
	public static void GetZoins(){

		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("GET_ZOIN").
		//SetEventAttribute("Z", uiScript.zoins).
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Received Player Data From GameSparks...");

				if(response.ScriptData.GetInt("ZOIN") == null){
					ManualReset(3);
					return;
				}

				//GSData data = response.ScriptData.GetGSData ("ZOIN");

				var zoins = response.ScriptData.GetInt("ZOIN").Value;

				GameManager.zoins = zoins;

				print("mike");

			} else {
				Debug.Log ("Error Loading Player Data...");

			}
		});

	}

	public static void SetZoin(int amount){
		
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("SET_ZOIN").
		SetEventAttribute ("ZOIN", amount).
		Send ((response) => {
			if (!response.HasErrors) {
				//GetZoins();
				//GetZoins();
			} else {
				Debug.Log("Error");
			}
		});
	}


	public static void ManualReset(int zoin){

		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("MANUAL_SET").
		SetEventAttribute ("ZOIN", zoin).
		Send ((response) => {
			if (!response.HasErrors) {

			} else {
				Debug.Log("Error");
			}
		});
	}


	public void SubmitScore(int points){
		
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("SUBMIT_SCORE").
		SetEventAttribute ("SCORE", points.ToString()).
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Score Posted Successfully...");
			} else {
				Debug.Log ("Error Posting Score...");
			}
		});
	}

	public void GetScores(){
		new GameSparks.Api.Requests.LeaderboardDataRequest ().
		SetLeaderboardShortCode ("High_Score_Leaderboard").
		SetEntryCount (10).
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Found Leaderboard Data...");

				highScoreText = null;

				foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) {
					int rank = (int)entry.Rank;
					string playerName = entry.UserName;
					string score = entry.JSONData ["SCORE"].ToString ();
					Debug.Log ("Rank:" + rank + " Name:" + playerName + " \n Score:" + score);
					CreateHighScoreText("\n Rank:" + rank + " Name:" + playerName + " Score:" + score);
				}
			} else {
				Debug.Log ("Error Retrieving Leaderboard Data...");
			}
		});
	}

	public void LogOut(){
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("LOGOUT").
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log("Player Logged out...");
				//Load next menu etc here?!
				GS.GSPlatform.AuthToken = "";
			} else {
				Debug.Log("Error Logging out...");
			}
		});

	}

	public static void GameOver(){
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("GAME_OVER").
		Send ((response) => {
			if (!response.HasErrors) {
				
			} else {
				Debug.Log("Error");
			}
		});
	}

	public void CreateHighScoreText(string input){
		highScoreText = highScoreText + input;
	}

}
