using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public enum GameStates
{
	Init,
	Welcome,
	Mainmenu,
	PlayGame,
	GameOver
}

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;
	//Static instance of GameManager which allows it to be accessed by any other script

	public static int currentPoints;
	public GameStates currentState;
	int _playerTopScore, _universalTopScore, _highestSavedScore;
	public string[] topPlayerNames = new string[3];
	public DateTime dateLastPlayed, day, prizeDay;

	static int _zoins;

	static string _grandPrize;
	public static string grandPrize {
		get{ return _grandPrize; }
		set{ 
			_grandPrize = value;
			UIManager.instance.SetGrandPrize (value);
			WinnerPageUI.instance.SetGrandPrizeText (value);
			}
	}

	public int instantCashScore, instantWinsLeft, instantWinPrize, totalGamesPlayed, totalScore;

	public static bool instantWinsAvailable;

	public static int daysUntilPrize, hrsUntilPrize, minsUntilPrize, secsUntilPrize;

	static public string userID, deviceToken;

	static string _userName;

	public bool jointUniversalScore = false;

	public static string userName {
		get{return GetFirstName (_userName);}
		set{ _userName = value;}
	}

	static public string GetFirstName(string wholeName){
		string[] names = wholeName.Split(new char[] {' '});
		return names[0].ToUpper() ;
	}

	static string _email;

	public static string email {
		get { 
			if (string.IsNullOrEmpty (_email)) {
				return "No email provided";
			}
			return _email;
		}
		set{ _email = value; }
	}

	string _fbPicUrl;

	public string fbPicUrl {
		get{ return _fbPicUrl; }
		set { 
			_fbPicUrl = value;
			StartCoroutine (FetchProfilePic (value)); 
		}
	}

	static public Texture2D profilePic;


	public static int zoins {
		get{ return _zoins; }
		set {
			_zoins = value;
			UIManager.instance.ZoinCount (_zoins);
		}
	}

	public int universalTopScore {
		get{ return _universalTopScore; }
		set {
			_universalTopScore = value;
			UIManager.instance.SetTopScores (value.ToString ());

			if (value == playerTopScore) {
				UIManager.instance.SetTopScoreMessage (true, jointUniversalScore);

			} else {
				UIManager.instance.SetTopScoreMessage (false);
			}
				
		}
	}

	public int playerTopScore {
		get{ return _playerTopScore; }
		set {
			_playerTopScore = value > _playerTopScore ? value : _playerTopScore;
			UIManager.instance.SetPlayerTopScore (_playerTopScore); 

		}
	}

	public bool online {
		get {
			if (Application.internetReachability != NetworkReachability.NotReachable) {
				return true;
			} else {
				return false;
			}
		}
	}


	bool _sfxMuted;
	public bool sfxMuted{
		get { return _sfxMuted; }
		set{ 
			if(value == true){
				PlayerPrefs.SetInt ("mute", 1);
				_sfxMuted = true;
			}else{
				PlayerPrefs.SetInt ("mute", 0);
				_sfxMuted = false;
			};}
	}

	void Awake (){

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
				Destroy (gameObject);  
			}
	}

	void Start (){
		day = DateTime.Now.Date;

		GetPlayerPrefs ();
	}

	void Update (){
		UIManager.instance.InternetAccessNotification (online);
		if (currentState != GameStates.PlayGame) {
			Countdown ();
			UIManager.instance.PrizeCountdown ();
		}
	}

	void Countdown (){
		//TimeSpan timeLeft = prizeDay.Subtract (DateTime.Now);
		//Universal ver..
		TimeSpan timeLeft = prizeDay.Subtract (DateTime.UtcNow);

		if (timeLeft < TimeSpan.Zero) {
			timeLeft = TimeSpan.Zero;
		}

		daysUntilPrize = timeLeft.Days;
		hrsUntilPrize = timeLeft.Hours;
		minsUntilPrize = timeLeft.Minutes;
		secsUntilPrize = timeLeft.Seconds;

	}

	public void CurrentState (GameStates currentState){
		switch (currentState) {

		case GameStates.Welcome:

			UIManager.instance.ShowWelcome ();
			break;

		case GameStates.Mainmenu:

			GameSparksManager.instance.ManagePushNotifications (deviceToken);//iOS

			UIManager.instance.MainMenuUI ();

			break;

		case GameStates.PlayGame:

			currentPoints = 0;
			GameplayController.instance.RecordStartTime ();
			GameplayController.instance.StartGame ();

			UIManager.instance.ShowGameUI ();

			break;

		case GameStates.GameOver:
			
			//Instant win check, then submits to leaderboard
			GameSparksManager.instance.GetInstantCount (currentPoints, true);
			//Set Top Score
			playerTopScore = currentPoints;

			dateLastPlayed = day;

			break;

		default:
			break;
		}

		this.currentState = currentState;
	}

	public void FirstPlay (){
		int initialZoins = 10;
		GameSparksManager.ManualReset (initialZoins);
		zoins = initialZoins;
		_playerTopScore = 0;
		UIManager.instance.ShowLoadingScreen ();
	}

	public void ResetGame (){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	IEnumerator FetchProfilePic (string url){
		WWW www = new WWW (url);
		yield return www;
		profilePic = www.texture;
	}

	public void CashPrizeScore(int value) {

		bool instantWin = false;

		if (value > instantCashScore) {
			UIManager.instance.SetCashPrizeScore (0);
			if (instantWinsAvailable) {
				GameSparksManager.instance.InstantWin ();
				instantWin = true;
			}
		} else {
			int temp = instantCashScore - value;
			UIManager.instance.SetCashPrizeScore (temp);
		}

		GameSparksManager.instance.SubmitScore (value, instantWin);
	}

	public void UpdateGamesparks(){
		
		GameSparksManager.instance.UpdateInformation ();
		//Finds Leaderboard #1 score
		GameSparksManager.instance.GetScores ();
		//Updates InstantWin data
		GameSparksManager.instance.GetInstantCount (0,false);

	}

	static public void DestroyChildren(GameObject parent){
		Transform[] children = parent.GetComponentsInChildren<Transform> ();
		foreach(Transform i in parent.transform){
			if (i.gameObject == parent.gameObject)
				continue;
			Destroy (i.gameObject);
		}
	}

	void GetPlayerPrefs(){
		//Unomment out below for testing
		//PlayerPrefs.DeleteAll ();

		int hasPlayed = PlayerPrefs.GetInt( "HasPlayed");
		if( hasPlayed == 0 )
		{
			PlayerPrefs.SetInt ("mute", 1);
			PlayerPrefs.SetInt( "HasPlayed", 1 );
		}

		bool mute = PlayerPrefs.GetInt("mute") == 1?true:false;
		sfxMuted = mute;

	}

}
