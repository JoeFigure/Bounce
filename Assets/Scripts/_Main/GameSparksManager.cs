using UnityEngine;
using System.Collections;
using GameSparks.Core;
using System.Collections.Generic;
using GameSparks.Api.Messages;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using System.Linq;
using System.Collections.Specialized;


public class GameSparksManager : MonoBehaviour
{

	public bool gsAuthenticated {
		get { return GS.Authenticated; }
	}

	public static string gsActivity;

	public static GameSparksManager instance = null;

	void Awake (){
		if (instance == null) {
			instance = this;
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
		SetDurable(true).
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
		SetDurable(true).
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
		SetDurable(true).
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
		SetDurable(true).
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
		SetDurable(true).
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
		new AccountDetailsRequest ()
		.SetDurable(true)
			.Send ((response) => {

			var zoinData = response.ScriptData.GetInt ("ZOIN");
			int zoins = zoinData.HasValue ? (int)zoinData : 0;
			GameManager.zoins = zoins;

			var temp = response.ScriptData.GetInt ("TOPSCORE");
			int score = temp.HasValue ? (int)temp : 0;
			GameManager.instance.playerTopScore = score;

			var temp2 = response.ScriptData.GetInt ("TOTAL_PLAYED");
			int totalPlayed = temp2.HasValue ? (int)temp2 : 0;
			GameManager.instance.totalGamesPlayed = totalPlayed;

			var temp3 = response.ScriptData.GetInt ("TOTAL_SCORE");
			int totalScore = temp3.HasValue ? (int)temp3 : 0;
			GameManager.instance.totalScore = totalScore;

			var fbPic = response.ScriptData.GetString ("FBPIC");
			GameManager.instance.fbPicUrl = fbPic;

			string tEmail = (string)response.ScriptData.GetString ("EMAIL");
			GameManager.email = tEmail;

			string userName = response.DisplayName;
			GameManager.userName = userName;
		});
	}

	public void InstantWin (){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetDurable(true).
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
			.SetDurable(true)
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
		SetDurable(true).
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
		SetDurable(true).
		Send ((response) => {
			if (!response.HasErrors) {
				GameManager.instance.FirstPlay ();
			}
		});
	}

	public void LogPurchase (string type){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("LOG_PURCHASE").
		SetEventAttribute("TYPE",type).
		SetDurable(true).
		Send((response) => {
			if (!response.HasErrors) {
			}
		});
	}

	public void GetInstantWinners (){

		List<Data> profileData = new List<Data>();

		new GameSparks.Api.Requests.LogEventRequest ().
		SetEventKey ("GETINSTANTWINNERS").
		SetDurable(true).
		Send ((response) => {
			if (!response.HasErrors) {

				//Get images
				var dump = response.ScriptData.GetGSDataList("DUMP");
				List<GameSparks.Core.GSData> d = (List<GameSparks.Core.GSData>)dump;

				int tot = (int)response.ScriptData.GetInt("WINNERBATCHAMOUNT");
				for (int i = 0; i < tot; i++){
					profileData.Add(new Data("Empty","Empty"));
				}

				int x = 0;
				foreach(GSData i in d){
					profileData.Insert(x, new Data( i.GetString("playerID"),i.GetString("image")));
					x++;
				}

				WinnerPageUI.instance.profileData = profileData;	

				//StartCoroutine (WinnerPageUI.instance.Icons1());
				WinnerPageUI.instance.page = 0;
				StartCoroutine (WinnerPageUI.instance.ChangePage());

			} else { Debug.Log("ERRRROR");}
		});
	}

	public void GetInstantWinnerProfile (string playerID, Sprite profileImage){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetDurable (true).
		SetEventKey ("GETINSTANTDETAILS").
		SetEventAttribute ("IMG_URL", playerID).
		Send ((response) => {
			if (!response.HasErrors) {

				string totalPlays = response.ScriptData.GetInt("TOTALPLAYS").ToString();
				string topScore = response.ScriptData.GetInt("TOPSCORE").ToString();
				string averageScore = response.ScriptData.GetInt("AVERAGESCORE").ToString();
				string location = response.ScriptData.GetString("LOCATION");
				string displayName = response.ScriptData.GetString("DISPLAYNAME");

				WinnerPageUI.instance.ShowWinnerProfile(displayName,location,profileImage,totalPlays,topScore,averageScore);
			}
		});
	}

	public void GetGrandWinner (){
		new GameSparks.Api.Requests.LeaderboardDataRequest ().
		SetLeaderboardShortCode ("High_Score_Leaderboard").
		SetEntryCount (1).
		SetDurable(true).
		Send ((response) => {
			if (!response.HasErrors) {
				int i = 0;
				string playerName;
				string location;
				foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) {
					playerName = entry.UserName;

					if(!string.IsNullOrEmpty(entry.City)){
						location = entry.City;
					}else{
						location = entry.Country;
					}
					string fbPic = response.ScriptData.GetString("FB_PIC");

					WinnerPageUI.instance._grandWinnerLocation = location;
					WinnerPageUI.instance._grandWinnerName = playerName;
					WinnerPageUI.instance.grandWinnerTotalGames = (int)response.ScriptData.GetInt("TOTAL_PLAYED");
					WinnerPageUI.instance.grandWinnerTopScore = (int)response.ScriptData.GetInt("TOPSCORE");
					WinnerPageUI.instance.grandWinnerAverageScore = (int)response.ScriptData.GetInt("AVERAGESCORE");

					WinnerPageUI.instance.ShowGrandWinner(playerName, location, fbPic);
				}
			} else {
				Debug.Log ("Error");
			}
		});
	}

	public void SubmitNewEmail (string newEmail){
		new GameSparks.Api.Requests.LogEventRequest ().
		SetDurable (true).

		SetEventKey ("CHANGEEMAIL").
		SetEventAttribute ("NEWEMAIL", newEmail).
		Send ((response) => {
			if (!response.HasErrors) {
			}
		});

	}

	public void ManagePushNotifications (string deviceToken){
		new GameSparks.Api.Requests.PushRegistrationRequest ()
			.SetPushId(deviceToken)
			.SetDurable(true)
			.Send((response) => {
				if (response.HasErrors) {
					Debug.Log("Push registration error");
				}else{
					string registrationId = response.RegistrationId; 
					Debug.Log("ID: " + registrationId);
				}
			});
	}

}
