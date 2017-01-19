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


	public void SubmitScore (int points, bool instantWin){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetDurable (true).
		SetEventKey ("SUBMIT_SCORE").
		SetEventAttribute ("SCORE", points.ToString ()).
		Send ((response) => {
			if (!response.HasErrors) {

				bool universalWin = false;

				//Puts score in 1st if universal score beaten
				if (points > GameManager.instance.universalTopScore) {
					GameManager.instance.universalTopScore = points;
					universalWin = true;
				}

				UpdateInformation ();
				StartCoroutine (UIManager.instance.WaitAndDisplayGameOver (universalWin, instantWin));
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

			var fbPic = response.ScriptData.GetString ("FBPIC");
			GameManager.instance.fbPicUrl = fbPic;

			string userName = response.DisplayName;
			GameManager.userName = userName;
			UIManager.instance.usernameMainMenuString = userName;
		});
	}

	public void InstantWin (){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("INSTANTWIN").
		Send ((response) => {
			if (!response.HasErrors) {
				//Debug.Log ("ADDED");
				GameManager.instantWinsAvailable = false;
			}
		});
	}

	public void GetInstantScore(int prizesLeft){
		new GetPropertyRequest()
			.SetPropertyShortCode("WINCOUNT")
			.Send((response) => {
				int score = (int)response.Property.GetInt("SCORE");
				int prize = (int)response.Property.GetInt("PRIZE");
				GameManager.instance.instantCashScore = score;
				GameManager.instance.instantWinPrize = prize;

				UIManager.instance.SetInstantWinText(prizesLeft);
			});
	}

	public void GetInstantCount (int score, bool postGame){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("GETINSTANTCOUNT").
		Send ((response) => {
			if (!response.HasErrors) {
				string instantWinsAvailable = response.ScriptData.GetString ("INSTANTAVAILABLE");
				//Check if there are instant wins available
				bool available = false;
				if (instantWinsAvailable == "TRUE")
					available = true;
				//Check if player has won already
				var previousWinner = response.ScriptData.GetGSData("PREVIOUSWINNER");
				if(previousWinner != null){
					available = false;
				}

				GameManager.instantWinsAvailable = available;
				UIManager.instance.DisplayInstantWinPanel (available);

				//Invoke Get Instant Score
				int prizesLeft = (int)response.ScriptData.GetInt("PRIZESLEFT");
				GameManager.instance.instantWinsLeft = prizesLeft;
				GetInstantScore(prizesLeft);

				//Calculate if score is instantWinner
				if(postGame)
				GameManager.instance.CashPrizeScore(score);
			}
		});

	}

	public void InitSubmit (){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("INIT_SUBMIT").
		SetEventAttribute ("EMAIL", GameManager.email).
		SetEventAttribute ("FB_PIC", GameManager.instance.fbPicUrl).
		Send ((response) => {
			if (!response.HasErrors) {
				Debug.Log ("VICTORY");
				GameManager.instance.FirstPlay ();
			}
		});
	}

}
