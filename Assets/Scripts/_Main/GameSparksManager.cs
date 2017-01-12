using UnityEngine;
using System.Collections;
using GameSparks.Core;
using System.Collections.Generic;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;


public class GameSparksManager : MonoBehaviour
{

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
		get{ return usernameRegister + "1"; }
	}

	public bool gsAuthenticated {
		get { return GS.Authenticated; }
	}

	public static string gsActivity;

	public static GameSparksManager instance = null;

	void Awake (){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (this.gameObject); 
		} else {
			Destroy (this.gameObject);
		}
	}

	void Start (){
		GS.GameSparksAvailable += HandleGameSparksMessageReceived;
	}


	void HandleGameSparksMessageReceived (bool available){
		UIManager.instance.IndicateGameSparksAvailability (available);
	}

	public void StartCoroutineCheckIfGamesparksAvailable (){
		StartCoroutine (CheckIfGamesparksAvailable ());
	}


	IEnumerator CheckIfGamesparksAvailable (){
		if (GameManager.instance.online) {
			while (!GS.Available) {
				yield return null;
			}
			CheckIfAuthenticated ();
		}
	}

	void CheckIfAuthenticated (){
		if (gsAuthenticated) {
			GameManager.instance.CurrentState (GameStates.Mainmenu);
		} else {
			UIManager.instance.ShowWelcome ();
		}
	}


	public static void AddZoin (int amount){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("SET_ZOIN").
		SetEventAttribute ("ZOIN", amount).
		Send ((response) => {
			if (response.HasErrors) {
				Debug.Log ("Error");
			} else {
				GameManager.zoins += amount;
			}
		});
	}

	//Uses Cloudcode
	public static void ManualReset (int zoin){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("MANUAL_SET").
		SetEventAttribute ("ZOIN", zoin).
		SetEventAttribute ("TOPSCORE", 0).
		Send ((response) => {
			if (!response.HasErrors) {
				UIManager.instance.ShowIntroTutorial ();
			}
		});
	}


	public void SubmitScore (int points){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetDurable (true).
		SetEventKey ("SUBMIT_SCORE").
		SetEventAttribute ("SCORE", points.ToString ()).
		//SetEventAttribute ("TOPSCORE", points).
		Send ((response) => {
			if (!response.HasErrors) {

				bool universalWin = false;
				bool instantWin = false;

				//Puts score in 1st if universal score beaten
				if(points > GameManager.instance.universalTopScore){
					GameManager.instance.universalTopScore = points;
					universalWin = true;
				}
				if(points > GameManager.instance.instantCashScore){
					instantWin = true;
				}

				UpdateInformation();
				StartCoroutine (UIManager.instance.WaitAndDisplayGameOver (universalWin,instantWin));
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


	public void LogOut (){
		new GameSparks.Api.Requests.LogEventRequest ().SetEventKey ("LOGOUT").
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("Player Logged out...");
				GameManager.instance.CurrentState (GameStates.Welcome);
				GS.Reset ();
			} else {
				Debug.Log ("Error Logging out...");
				GS.Reset ();
			}
			//Records activity for in-game log
			gsActivity = response.JSONString;
		});
	}
		
	public void UpdateInformation (){
		new AccountDetailsRequest ().Send ((response) => {
			
			var zoinData = response.ScriptData.GetInt ("ZOIN");
			int zoins = zoinData.HasValue ? (int)zoinData : 0;
			GameManager.zoins = zoins;

			var temp = response.ScriptData.GetInt ("TOPSCORE");
			int score = temp.HasValue ? (int)temp : 0;
			GameManager.instance.playerTopScore = score;

			string userName = response.DisplayName;
			GameManager.userName = userName;
			UIManager.instance.usernameMainMenuString = userName;
		});
	}

}
