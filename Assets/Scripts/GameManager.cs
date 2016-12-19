using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public enum GameStates
{
	Init, Welcome, Mainmenu, PlayGame, GameOver
}

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;
	//Static instance of GameManager which allows it to be accessed by any other script

	public static int currentPoints;
	public GameStates currentState;
	int _playerTopScore, _universalTopScore;
	public string[] topPlayerNames = new string[3];
	public DateTime dateLastPlayed, day, prizeDay;
	//public bool signedInLastSession;
	public bool lastGameWasOnline;
	static int _zoins;

	public static int daysUntilPrize,hrsUntilPrize,minsUntilPrize,secsUntilPrize;

	static public string userName, userID;

	public int instantCashScore = 50;

	public List<int> playerScores = new List<int> ();

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
		}
	}

	public int playerTopScore {
		get{ return _playerTopScore; }
		set {
			_playerTopScore = value;
			UIManager.instance.SetPlayerTopScore (value); 
		}
	}

	public int cashPrizeScore{
		set {
			if (value > instantCashScore) {
				UIManager.instance.SetCashPrizeScore (0);
			} else {
				int temp = instantCashScore - value;
				UIManager.instance.SetCashPrizeScore (temp);
			}; 
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

	void Awake (){

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
				Destroy (gameObject);  
			}

		DontDestroyOnLoad (gameObject);
	}

	void Start (){
		day = DateTime.Now.Date;
		prizeDay = new DateTime (2017, 1, 1 , 1, 1, 1);
		Load ();
	}

	void Update (){
		UIManager.instance.InternetAccessNotification (online);
		Countdown();
	}

	void Countdown(){
		TimeSpan timeLeft = prizeDay.Subtract (DateTime.Now);
		daysUntilPrize = timeLeft.Days;
		hrsUntilPrize = timeLeft.Hours;
		minsUntilPrize = timeLeft.Minutes;
		secsUntilPrize = timeLeft.Seconds;
	}

	public void CurrentState (GameStates currentState){
		switch (currentState) {

		case GameStates.Init:

			if (PlayerPrefs.HasKey ("Signed In")) {
				if (PlayerPrefs.GetString ("Signed In") == "True") {
					CurrentState (GameStates.Mainmenu);
				} else {
					CurrentState (GameStates.Welcome);
				}
			} else {
				//First Time Play
				PlayerPrefs.SetString ("Signed In", "False");
				CurrentState (GameStates.Welcome);
			}

			break;

		case GameStates.Welcome:

			UIManager.instance.ShowWelcome ();
			break;

		case GameStates.Mainmenu:

			Load ();

			UIManager.instance.MainMenuUI ();

			//currentPoints = 0;
			zoins = zoins;


			if (GameSparksManager.instance.gsAuthenticated) {
				GameSparksManager.instance.UpdateInformation ();
				LoggedIn ();
				GameSparksManager.GetTopScore ();
				GameSparksManager.instance.GetScores ();
			}



			//CheckDailyReward (dateLastPlayed);

			break;

		case GameStates.PlayGame:

			currentPoints = 0;

			GameplayController.instance.StartGame ();
			GameplayController.instance.RecordStartTime ();

			UIManager.instance.ShowGameUI ();

			break;

		case GameStates.GameOver:

			StartCoroutine (UIManager.instance.WaitAndDisplayGameOver ());

			cashPrizeScore = currentPoints;

			if (GameSparksManager.instance.gsAuthenticated) {
				GameSparksManager.SubmitScore (currentPoints);
				//lastGameWasOnline = true;
				GameSparksManager.GameOver ();
				GameSparksManager.GetTopScore ();
			} else {
				//lastGameWasOnline = false;
				zoins--;
			}

			if (currentPoints > playerTopScore) {
				UIManager.instance.DisplayWinOrLose (true);
			} else {
				UIManager.instance.DisplayWinOrLose (false);
			}
				
			dateLastPlayed = day;
			Save ();

			break;

		default:
			break;
		}

		this.currentState = currentState;
	}

	/*
	public void ResetGame (){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	*/

	public void Save (){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		SaveData saveData = new SaveData ();
		saveData.savedZoins = zoins;
		//saveData.signedIn = signedInLastSession;
		//saveData.lastGameWasOnline = online;
		saveData.dateLastPlayed = day;//new DateTime (2000, 1, 1);
		bf.Serialize (file, saveData);
		file.Close ();
	}

	public void ClearSavedData (){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		SaveData saveData = new SaveData ();
		saveData.savedZoins = 0;
		//saveData.lastGameWasOnline = true;
		saveData.dateLastPlayed = day;
		bf.Serialize (file, saveData);
		file.Close ();
	}

	public void Load (){
		if (File.Exists (Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			SaveData saveData = (SaveData)bf.Deserialize (file);
			file.Close ();
			zoins = saveData.savedZoins;
			//signedInLastSession = saveData.signedIn;
			//lastGameWasOnline = saveData.lastGameWasOnline;
			dateLastPlayed = saveData.dateLastPlayed;
		}
	}
	/*
	void CheckDailyReward (DateTime datelastPlayed){
		if (datelastPlayed != day) {
			UIManager.instance.DailyRewardPopup ();
			GameSparksManager.AddZoin (10);
			Save ();
		}
	}
*/
	public void FirstPlay (){
		int firstTimeFreeZoins = 10;
		GameSparksManager.ManualReset (firstTimeFreeZoins);
		zoins = firstTimeFreeZoins;
		UIManager.instance.ShowIntroTutorial ();
		Save ();
	}

	public void LoggedIn (){
		
		if (lastGameWasOnline) {
			GameSparksManager.GetZoins ();
		} else {
			GameSparksManager.ManualReset (zoins);
		}
	}

	public void SetSignedInPrefs(bool signedIn){
		if (signedIn) {
			PlayerPrefs.SetString ("Signed In", "True");
		} else {
			PlayerPrefs.SetString ("Signed In", "False");
		}
	}
}

[Serializable]
public class SaveData
{
	public bool lastGameWasOnline;

	public DateTime dateLastPlayed;

	[NonSerialized]//Delete this eventually
	public bool signedIn;

	public int savedZoins;

	//Useful?
	public List<int> savedPlayerScores = new List<int> ();

}
