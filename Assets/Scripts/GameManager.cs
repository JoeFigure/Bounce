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


	public DateTime dateLastPlayed;

	public static bool signedIn = false;

	public DateTime day;

	public static BallScript ball;

	public bool lastGameWasOnline;

	static int _zoins;

	static float _speed = -3.8f;

	public static int currentPoints;

	public static float gameTimeStart;

	public List<int> playerScores = new List<int>();

	public static int zoins {
		get{ return _zoins; }
		set {
			_zoins = value;
			UIManager.instance.ZoinCount (_zoins);
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

	public static float gameTime {
		get{ return Time.time - gameTimeStart; }
	}

	public static float speed{
		get{ 
			if(ball.alive){
				return (_speed * speedMult) * Time.deltaTime;
			} else{
				return 0;
			}
		}
		set{ _speed = value ; }
	}

	public static float gameTimePercentOfFullSpeed{
		get {
			int secondsToFullSpeed = 120;
			float percentageToFullSpeed = gameTime / secondsToFullSpeed;
			return percentageToFullSpeed;
		}
	}

	static float speedMult {
		get { 
			float percOfFullSpeed = gameTimePercentOfFullSpeed;
			float multiplier = 2;
			return (percOfFullSpeed * multiplier) + 1;
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
	
	// Update is called once per frame
	void Update () {

		OnlineNotification ();

	}

	public void CurrentState (GameStates currentState)
	{

		ball = GameObject.Find ("Ball").GetComponent<BallScript>();

		switch (currentState) {

		case GameStates.Welcome:

			UIManager.instance.ShowWelcome ();

			break;

		case GameStates.Mainmenu:

			UIManager.instance.MainMenuUI ();
			currentPoints = 0;
			zoins = zoins;
			GameSparksManager.instance.GameSparksConnected ();
			CheckDailyReward (dateLastPlayed);

			break;

		case GameStates.PlayGame:

			ball.alive = true;
			ball.ResetBounce ();
			UIManager.instance.ZoinCount (zoins);

			break;

		case GameStates.GameOver:
			
			StartCoroutine (UIManager.instance.WaitAndDisplayScore ());
			playerScores.Add (currentPoints);
			if (online && signedIn) {
				GameSparksManager.SubmitScore (currentPoints);
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
	}

	public void ResetGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	void OnlineNotification(){
			if (online) {
				UIManager.instance.InternetAccessNotification (true);
			} else {
				UIManager.instance.InternetAccessNotification (false);
			}
	}

	public void RecordStartTime(){
		gameTimeStart = Time.time;
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		SaveData saveData = new SaveData();
		playerScores.Clear ();
		saveData.savedPlayerScores = playerScores;
		saveData.savedZoins = zoins;
		saveData.signedIn = signedIn;
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
		saveData.signedIn = signedIn;
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
			lastGameWasOnline = saveData.lastGameWasOnline;
			dateLastPlayed = saveData.dateLastPlayed;
			signedIn = saveData.signedIn;
		}
	}

	void CheckDailyReward(DateTime datelastPlayed){
		if (datelastPlayed != day) {
			UIManager.instance.DailyRewardPopup();
			GameSparksManager.AddZoin (10);
		}
	}

	public void FirstPlay(){
		//Checks if null zoins in Get
		int firstTimeFreeZoins = 10;
		GameSparksManager.ManualReset(firstTimeFreeZoins);
		zoins = firstTimeFreeZoins;
		UIManager.instance.ShowTextPopup ("Welcome", "First play.", true);
	}

	public void LoggedIn(){

		UIManager.instance.SignedIntoGameSparksNotification (true);

		if(lastGameWasOnline){
			GameSparksManager.GetZoins();
		}else{
			GameSparksManager.ManualReset(zoins);
		}

		GameSparksManager.SetTopScore(highestSavedScore);
		GameSparksManager.SubmitScore(highestSavedScore);

	}
}

[System.Serializable]
public class SaveData { 

	public bool lastGameWasOnline;

	public DateTime dateLastPlayed;

	public bool signedIn;

	public int savedZoins;

	public List<int> savedPlayerScores = new List<int> ();

}
