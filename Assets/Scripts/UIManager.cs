using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;

public class UIManager : MonoBehaviour {

	public static UIManager instance = null;

	public static UIData uiData;


	public string highScoreString {
		get{ return uiData.highScoreText.text; }
		set{ uiData.highScoreText.text = value; }
	}

	public string usernameLoginString {
		get{ return uiData.usernameLoginText.text; }
	}
	public string passwordLoginString {
		get{ return uiData.passwordLoginText.text; }
	}

	public string usernameRegisterString {
		get{ return uiData.usernameRegisterText.text; }
	}
	public string passwordRegisterString {
		get{ return uiData.passwordRegisterText.text; }
	}

	public string usernameMainMenuString {
		get { return uiData.usernameMainMenuText.text; }
		set{ uiData.usernameMainMenuText.text = value; }
	}

	public int pointsToNextPrize {
		get{ return 100 - GameManager.currentPoints; } 
	}

	void Awake ()
	{

		//Check if instance already exists
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);  
		}

		DontDestroyOnLoad (gameObject);

	}

	// Use this for initialization
	void Start () {


		GameManager.instance.CurrentState (GameStates.Mainmenu);

		uiData.authBttn.GetComponent<Button>().onClick.AddListener(() => { uiData.gsm.Authorise(); }); 
		uiData.logoutBttn.GetComponent<Button>().onClick.AddListener(() => { uiData.gsm.LogOut(); }); 

		uiData.zoinsText.text = GameManager.zoins.ToString();

	}
	
	// Update is called once per frame
	void Update () {

		uiData.inGameScore.text = GameManager.currentPoints.ToString();
	}

	public void StartGame(){

		if (GameManager.zoins > 0) {

			GameManager.instance.CurrentState (GameStates.PlayGame);
			GameManager.instance.RecordStartTime ();

			GamePlayUI ();
		}
	}

	public void ReplayBttn(){
		GameManager.instance.ResetGame();
	}
		

	public void MenuUI(){

		ShowMenu (uiData.mainMenuUI);
		uiData.topHUDUI.SetActive (true);

	}

	void GamePlayUI(){
		ShowMenu (uiData.gameUI);
		uiData.topHUDUI.SetActive (true);
	}
	 
	public void ShowHighScoresBttn(){

		uiData.gsm.GetScores ();
		ShowMenu (uiData.highScoresUI);
	}
		

	public void ShowMenu(GameObject showUI){
		Transform[] transform_list = uiData.uiViewsContainer.GetComponentsInChildren<Transform>();
		foreach (var tran in transform_list) {
			if ( tran.gameObject == uiData.uiViewsContainer )
			{
				continue;
			}
			if (tran.parent.gameObject == uiData.uiViewsContainer) {
				tran.gameObject.SetActive (false);
			}
		}
		showUI.SetActive (true);
	}

	public IEnumerator WaitAndDisplayScore() {
		yield return new WaitForSeconds(1.2f);
		uiData.gsm.SubmitScore (GameManager.currentPoints);

		uiData.gameOverScore.text = GameManager.currentPoints.ToString ();
		uiData.nextPrizeScore.text = pointsToNextPrize.ToString ();

		ShowMenu (uiData.gameOverUI);

	}

	public void ZoinCount(int amount){
		uiData.zoinsText.text = amount.ToString ();
		//print ("yo");
	}

	public void SignedIntoGameSparksNotification(bool connected){

		if (connected) {
			uiData.signedInNotification.text = "Signed in to GS";
		} else {
			uiData.signedInNotification.text = "GS not signed in";
		}
	}

	public void InternetAccessNotification(bool connected){

		if (connected) {
			//internetAccessNotification.text = "Internet Connected";
			uiData.signalImage.color = Color.green;
			uiData.connectionText.text = "Online";
		} else {
			//internetAccessNotification.text = "Internet Not Connected";
			uiData.signalImage.color = Color.red;
			uiData.connectionText.text = "Offline - Prizes currently unavailable";
		}
	}

	public void AvailabilityOfGameSparksNotification(bool connected){

		if (connected) {
			uiData.availabilityNotification.text = "GS Availabble";
		} else {
			uiData.availabilityNotification.text = "GS not Available";
		}
	}

	public void ActivateOverlayPanel(bool activate){
		if (activate) {
			uiData.overlayPanel.SetActive (true);
		} else {
			uiData.overlayPanel.SetActive (false);
		}
	}

}
