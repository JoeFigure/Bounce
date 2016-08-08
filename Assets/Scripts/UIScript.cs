using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;

public class UIScript : MonoBehaviour {


	public Text inGameScore;
	public Text gameOverScore;
	public Text nextPrizeScore;
	public Text authName;
	public Text usernameLoginText;
	public Text passwordLoginText;
	public Text usernameRegisterText;
	public Text passwordRegisterText;

	public Text usernameMainMenuText;
	public Text highScoreText;
	public Text availabilityNotification;
	public Text signedInNotification;
	public Text internetAccessNotification;

	public Button authBttn;
	public Button logoutBttn;

	public GameSparksManager gsm;

	public GameObject mainMenuUI;
	public GameObject gameOverUI;
	public GameObject gameUI;
	public GameObject profileUI;
	public GameObject loginUI;
	public GameObject signUpUI;
	public GameObject highScoresUI;
	public GameObject debugUI;
	public GameObject settingsUI;


	public string highScoreString {
		get{ return highScoreText.text; }
		set{ highScoreText.text = value; }
	}

	public string usernameLoginString {
		get{ return usernameLoginText.text; }
	}
	public string passwordLoginString {
		get{ return passwordLoginText.text; }
	}

	public string usernameRegisterString {
		get{ return usernameRegisterText.text; }
	}
	public string passwordRegisterString {
		get{ return passwordRegisterText.text; }
	}

	public string usernameMainMenuString {
		get { return usernameMainMenuText.text; }
		set{ usernameMainMenuText.text = value; }
	}

	public int pointsToNextPrize {
		get{ return 100 - GameManager.instance.currentPoints; } 
	}


	// Use this for initialization
	void Start () {

		GameManager.instance.CurrentState (GameStates.Mainmenu);

		authBttn.GetComponent<Button>().onClick.AddListener(() => { gsm.Authorise(); }); 
		logoutBttn.GetComponent<Button>().onClick.AddListener(() => { gsm.LogOut(); }); 


	}
	
	// Update is called once per frame
	void Update () {

		inGameScore.text = GameManager.instance.currentPoints.ToString();
	
	}

	public void StartGameBttn(){
		GameManager.instance.CurrentState (GameStates.PlayGame);
		GameManager.instance.RecordStartTime ();
	}

	public void ReplayBttn(){
		GameManager.instance.ResetGame();
	}

	public void PlayUI(){

		mainMenuUI.SetActive (false);
		gameUI.SetActive (true);
	}

	public void MenuUI(){

		if (GS.Available) {
			if (GS.GSPlatform.AuthToken != " ") {
				print (GS.GSPlatform.AuthToken);
			}
		}

		gameUI.SetActive (false);
		mainMenuUI.SetActive (true);
		gameOverUI.SetActive (false);
		profileUI.SetActive (false);
		loginUI.SetActive (false);
		signUpUI.SetActive (false);
		highScoresUI.SetActive (false);
		settingsUI.SetActive (false);
	}

	public void GameOverUI(){
		gameUI.SetActive (false);
		gameOverUI.SetActive (true);
		gameOverScore.text = GameManager.instance.currentPoints.ToString ();
		nextPrizeScore.text = pointsToNextPrize.ToString ();
	}

	public void ProfileUI(){
		profileUI.SetActive (true);
		mainMenuUI.SetActive (false);
		loginUI.SetActive (false);
		signUpUI.SetActive (false);
	}

	public void LoginUI(){
		profileUI.SetActive (false);
		loginUI.SetActive (true);

	}

	public void SignUpUI(){
		signUpUI.SetActive (true);
		profileUI.SetActive (false);
		loginUI.SetActive (false);
	}

	public void HighScoresUI(){

		gsm.GetScores ();

		highScoresUI.SetActive (true);
		mainMenuUI.SetActive (false);
	}

	public void DebugUI(){
		mainMenuUI.SetActive (false);
		debugUI.SetActive (true);

	}

	public void SettingsUI(){
		mainMenuUI.SetActive (false);
		settingsUI.SetActive (true);

	}

	public IEnumerator WaitAndDisplayScore() {
		yield return new WaitForSeconds(1.2f);
		gsm.SubmitScore (GameManager.instance.currentPoints);

		GameOverUI ();

	}

	public void SignedIntoGameSparksNotification(bool connected){

		if (connected) {
			signedInNotification.text = "Signed in to GS";
		} else {
			signedInNotification.text = "GS not signed in";
		}
	}

	public void InternetAccessNotification(bool connected){

		if (connected) {
			internetAccessNotification.text = "Internet Connected";
		} else {
			internetAccessNotification.text = "Internet Not Connected";
		}
	}

	public void AvailabilityOfGameSparksNotification(bool connected){

		if (connected) {
			availabilityNotification.text = "GS Availabble";
		} else {
			availabilityNotification.text = "GS not Available";
		}
	}


}
