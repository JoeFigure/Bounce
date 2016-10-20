using UnityEngine;
using System.Collections;
using GameSparks.Core;
using System.Collections.Generic;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;


public class GameSparksManager : MonoBehaviour {

	string usernameLogin {
		get{ return UIManager.instance.usernameLoginString; }
	}
	string passwordLogin {
		get{ return UIManager.instance.passwordLoginString; }
	}

	string usernameRegister {
		get{ return UIManager.instance.usernameRegisterString; }
	}
	string passwordRegister {
		get{ return UIManager.instance.passwordRegisterString; }
	}
	string displaynameRegister {
		get{ return usernameRegister + "1" ; }
	}

	public bool gsAuthenticated {
		get {return GS.Authenticated;}
	}
			
	public static string gsActivity;

	public static GameSparksManager instance = null;
	void Awake() {
		if (instance == null){
			instance = this;
			DontDestroyOnLoad(this.gameObject); 
		} else{
			Destroy(this.gameObject);
		}
	}
		
	void Start () {
		GS.GameSparksAvailable += HandleGameSparksMessageReceived;

	}

	void HandleGameSparksMessageReceived (bool available) {
		UIManager.instance.IndicateGameSparksAvailability (available);
	}

	public void StartCoroutineCheckIfGamesparksAvailable(){
		StartCoroutine(CheckIfGamesparksAvailable());
	}

	IEnumerator CheckIfGamesparksAvailable(){
		if (GameManager.instance.online) {
		while (!GS.Available) {
				yield return null;
		}
			CheckIfAuthenticated ();
		} else {
			//No Connection route
			if (GameManager.instance.signedInLastSession) {
				GameManager.instance.CurrentState (GameStates.Mainmenu);
			} else {
				UIManager.instance.ShowWelcome ();
			}
		}
	}

	void CheckIfAuthenticated(){
		if (gsAuthenticated) {
			GameManager.instance.CurrentState (GameStates.Mainmenu);
		} else {
			UIManager.instance.ShowWelcome ();
		}
	}

	public void RegistrationRequest(){
		new GameSparks.Api.Requests.RegistrationRequest().
		SetDisplayName(displaynameRegister).
		SetPassword(passwordRegister).
		SetUserName(usernameRegister).
		Send((response) => {
			if (!response.HasErrors){
				GameManager.instance.ClearSavedData();
				Login();
			}else{
				Debug.Log("Error Registering Player");
				UIManager.instance.ShowTextPopup("Warning", "Username taken" , true);
			}
			//Records activity for in-game log
			gsActivity = response.JSONString;
		});
	}


	public void Authorise(){
		new GameSparks.Api.Requests.AuthenticationRequest().
		SetUserName(usernameLogin).
		SetPassword(passwordLogin).
		Send((response) => {
			if (!response.HasErrors) {
				Login();
				//UIManager.instance.EnableLoginButton(true);
			} else {
				ShowAuthWarning();
				UIManager.instance.EnableLoginButton(true);
			}
		});
	}

	void ShowAuthWarning(){
		UIManager.instance.ShowTextPopup("Warning", "Username or Password unrecognized" , true);
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

	//Uses Cloudcode
	public void LoadPlayer(){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("LOAD_PLAYER").
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Received Player Data From GameSparks...");
			} else {
				Debug.Log ("Error Loading Player Data...");
			}
		});
	}

		
	public static void GetZoins(){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("GET_ZOIN").
		Send ((response) => {
			if (!response.HasErrors) {
				if(response.ScriptData.GetInt("ZOIN") == null){
					// ENTRY POINT FOR FIRST TIME PLAY RECOGNITION
					GameManager.instance.FirstPlay();
					return;
				}
				var zoins = response.ScriptData.GetInt("ZOIN").Value;
				GameManager.zoins = zoins;

			} else {
				Debug.Log ("Error Loading Player Data...");
			}
		});
	}

	public static void AddZoin(int amount){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("SET_ZOIN").
		SetEventAttribute ("ZOIN", amount).
		Send ((response) => {
			if (response.HasErrors) {
				Debug.Log("Error");
			} else {
				GameManager.zoins += amount;
			}
		});
	}

	//Uses Cloudcode
	public static void SetTopScore(int amount){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("SET_TOPSCORE").
		SetEventAttribute ("TOPSCORE", amount).
		Send ((response) => {
			if (response.HasErrors) {
				Debug.Log("Error");
			}
		});
	}


	public static void GetTopScore(){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("GET_TOPSCORE").
		Send ((response) => {
			if (!response.HasErrors) {
				var temp = response.ScriptData.GetInt("TOPSCORE");
				int score = temp.HasValue ? (int)temp : 0;
				GameManager.instance.playerTopScore = score;
			} else {
				Debug.Log ("Error Loading Player Data...");
			}
		});
	}

	//Uses Cloudcode
	public static void ManualReset(int zoin){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("MANUAL_SET").
		SetEventAttribute ("ZOIN", zoin).
		Send ((response) => {
			if (response.HasErrors) {
				Debug.Log("Error");
			}
		});
	}


	public static void SubmitScore(int points){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetDurable (true).
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


	public void GetScores (){
		new GameSparks.Api.Requests.LeaderboardDataRequest ().
		SetLeaderboardShortCode ("High_Score_Leaderboard").
		SetEntryCount (3).
		Send ((response) => {
			if (!response.HasErrors) {
				int i = 0;
				foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) {
					string playerName = entry.UserName;
					GameManager.instance.topPlayerNames [i] = playerName;
					string score = entry.JSONData ["SCORE"].ToString ();
					if (i == 0) {
						GameManager.instance.universalTopScore = int.Parse (score);
					}
					i++;
				}
			} else {
				Debug.Log ("Error");
			}
		});
	}


	public void LogOut(){
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("LOGOUT").
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log("Player Logged out...");
				GameManager.instance.signedInLastSession = false;
				GameManager.instance.Save();
				GameManager.instance.CurrentState(GameStates.Welcome);
				GS.Reset();
			} else {
				Debug.Log("Error Logging out...");
				GS.Reset();
			}
			//Records activity for in-game log
			gsActivity = response.JSONString;
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

	public void Login(){
		GameManager.instance.CurrentState(GameStates.Mainmenu);
		GameManager.instance.signedInLastSession = true;
	}
		
	public void UpdateInformation(){
		new AccountDetailsRequest().Send((response) =>{
				//UpdateGUI(response.DisplayName, response.UserId, response.ExternalIds.GetString("FB").ToString());
				string userName = response.DisplayName;
				GameManager.userName = userName;
				UIManager.instance.usernameMainMenuString = userName;
			});
	}
}
