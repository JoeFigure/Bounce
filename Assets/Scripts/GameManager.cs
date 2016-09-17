using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;
using System;

public enum GameStates
{
	Welcome,
	Mainmenu,
	PlayGame,
	GameOver
}

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	//Static instance of GameManager which allows it to be accessed by any other script

	public static int currentPoints;

	public GameStates currentState;

	int _playerTopScore;

	int _universalTopScore;

	public string[] topPlayerNames = new string[3];

	public DateTime dateLastPlayed;

	public bool signedInLastSession;

	public DateTime day;

	public bool lastGameWasOnline;

	static int _zoins;

	public List<int> playerScores = new List<int>();

	public static int zoins {
		get{ return _zoins; }
		set {
			_zoins = value;
			UIManager.instance.ZoinCount (_zoins);
		}
	}

	public int universalTopScore {
		get{ return _universalTopScore; }
		set{ _universalTopScore = value;
			UIManager.instance.SetTopScores (value.ToString());
		}
	}

	public int highestSavedScore{
		get{ 
			int[] s = playerScores.ToArray ();

			if (s.Length > 0) {
				int highest = Mathf.Max (s);
				return highest;
			} else {
				return 0;
			}
		}
	}

	public string playerScoresString{
		get{
			string scoreOut = "";
			foreach (int score in playerScores) {
				string s = score.ToString ();
				scoreOut = scoreOut + "," + s;
			}
			return scoreOut;
		}
	}

	public int playerTopScore {
		get{ return _playerTopScore; }
		set{
			_playerTopScore = value;
			UIManager.instance.SetPlayerTopScore (value); 
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
		
	void Awake ()
	{

		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);  
		}

		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {

		day = DateTime.Now.Date;//new DateTime (2000, 1, 1);//
		Load ();

	}

	void Update () {
		UIManager.instance.InternetAccessNotification (online);
	}

	public void CurrentState (GameStates currentState)
	{
		switch (currentState) {

		case GameStates.Welcome:

			UIManager.instance.ShowWelcome ();

			break;

		case GameStates.Mainmenu:

			Load ();

			//UIManager.instance.MainMenuUI ();

			UIManager.instance.ShowIntroTutorial ();


			currentPoints = 0;
			zoins = zoins;
			GameSparksManager.instance.CheckConnected ();
			if (GameSparksManager.instance.gsAuthenticated) {
				LoggedIn ();
			}
			GameSparksManager.GetTopScore ();
			GameSparksManager.instance.GetScores ();
			CheckDailyReward (dateLastPlayed);

			break;

		case GameStates.PlayGame:

			GameplayController.instance.StartGame ();
			GameplayController.instance.RecordStartTime ();

			UIManager.instance.ShowGameUI ();

			break;

		case GameStates.GameOver:
			
			if (currentPoints < universalTopScore) {
				StartCoroutine (UIManager.instance.WaitAndDisplayScore ());
			} else {
				StartCoroutine (UIManager.instance.WaitAndDisplayWinningScore());
			}
				

			playerScores.Add (currentPoints);
			if (GameSparksManager.instance.gsAuthenticated) {
				GameSparksManager.SubmitScore (currentPoints);
				GameSparksManager.SetTopScore (currentPoints);
				lastGameWasOnline = true;
				GameSparksManager.GameOver ();
			} else {
				lastGameWasOnline = false;
				zoins--;
			}
				
			dateLastPlayed = day;
			Save ();

			break;

		default:
			break;
		}

		this.currentState = currentState;
	}

	public void ResetGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		SaveData saveData = new SaveData();
		playerScores.Clear ();
		saveData.savedPlayerScores = playerScores;
		saveData.savedZoins = zoins;
		saveData.signedIn = signedInLastSession;
		saveData.lastGameWasOnline = online;
		saveData.dateLastPlayed = day;//new DateTime (2000, 1, 1);
		bf.Serialize(file, saveData);
		file.Close();
	}

	public void ClearSavedData(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		SaveData saveData = new SaveData();
		playerScores.Clear ();
		saveData.savedPlayerScores = playerScores;
		saveData.savedZoins = 0;
		saveData.lastGameWasOnline = true;
		saveData.dateLastPlayed = day;
		bf.Serialize(file, saveData);
		file.Close();
	}

	public void Load() {
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			SaveData saveData = (SaveData)bf.Deserialize(file);
			file.Close();
			playerScores = saveData.savedPlayerScores;
			zoins = saveData.savedZoins;
			signedInLastSession = saveData.signedIn;
			lastGameWasOnline = saveData.lastGameWasOnline;
			dateLastPlayed = saveData.dateLastPlayed;
		}
	}

	void CheckDailyReward(DateTime datelastPlayed){
		if (datelastPlayed != day) {
			UIManager.instance.DailyRewardPopup();
			GameSparksManager.AddZoin (10);
			Save ();
		}
	}

	public void FirstPlay(){
		int firstTimeFreeZoins = 10;
		GameSparksManager.ManualReset(firstTimeFreeZoins);
		zoins = firstTimeFreeZoins;
		UIManager.instance.ShowTextPopup ("Welcome", "First play.", true);
		Save ();
	}

	public void LoggedIn(){
		
		if(lastGameWasOnline){
			GameSparksManager.GetZoins();
		}else{
			GameSparksManager.ManualReset(zoins);
		}

		GameSparksManager.SetTopScore(highestSavedScore);
		GameSparksManager.SubmitScore(highestSavedScore);

	}
}

[Serializable]
public class SaveData { 

	public bool lastGameWasOnline;

	public DateTime dateLastPlayed;

	//Delete this
	public bool signedIn;

	public int savedZoins;

	public List<int> savedPlayerScores = new List<int> ();

}
