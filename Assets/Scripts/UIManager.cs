using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;
using System.Collections.Generic; 


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
		get{ return uiData.usernameSignupText.text; }
	}
	public string passwordRegisterString {
		get{ return uiData.passwordSignupText.text; }
	}

	public string usernameMainMenuString {
		get { return uiData.usernameMainMenuText.text; }
		set{ uiData.usernameMainMenuText.text = value; }
	}

	public int pointsToNextPrize {
		get{ return 100 - GameManager.currentPoints; } 
	}

	void Awake (){
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

		uiData.zoinsText.text = GameManager.zoins.ToString();
	}
	
	// Update is called once per frame
	void Update () {

		uiData.scoreText.text = GameManager.currentPoints.ToString();
	}

	public void StartGame(){

		if (GameManager.zoins > 0) {

			GameManager.instance.CurrentState (GameStates.PlayGame);
			GameManager.instance.RecordStartTime ();

			ShowMenu (uiData.gameUI);
		}
	}

	public void ReplayBttn(){
		GameManager.instance.ResetGame();
	}
		

	public void MainMenuUI(){

		ShowMenu (uiData.mainMenuUI);
		uiData.topHUDUI.SetActive (true);

		uiData.lastGameText.text = "Last game online: " + GameManager.instance.lastGameWasOnline.ToString ();
	}

	public void ShowWelcome(){
		ShowMenu (uiData.welcomeMenuUI);
	}

	public void ShowHighScores(){

		GameSparksManager.instance.GetScores ();
		ShowMenu (uiData.highScoresUI);
	}

	public void ShowSettings(GameObject settingsUI, Text scoreText){

		ShowMenu (settingsUI);

	}
		
	public void ShowMenu(GameObject showUI){

		DeactivateAllChildren (uiData.uiViewsContainer);
		showUI.SetActive (true);
	}

	public IEnumerator WaitAndDisplayScore() {
		
		uiData.gameOverScore.text = GameManager.currentPoints.ToString ();
		uiData.nextPrizeScore.text = pointsToNextPrize.ToString ();
		uiData.scoreText.gameObject.SetActive (false);

		yield return new WaitForSeconds(1.2f);
		ShowPopup(uiData.gameOverPopupContent,"Game Over",false);

	}

	public void ZoinCount(int amount){
		uiData.zoinsText.text = amount.ToString ();
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
			uiData.signalImage.color = Color.green;
			uiData.connectionText.text = "Online";
		} else {
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

	public void ShowWelcomeUIPanel(GameObject iPanel){
		foreach(var panel in uiData.welcomeUIPanels){
			panel.SetActive (false);
		}
		iPanel.SetActive (true);
	}

	public void SignOut(){
		GameManager.signedIn = false;
		GameManager.instance.Save ();
	}

	public void DailyRewardPopup(){
		ShowPopup (uiData.rewardPopupContent, "Daily Reward", true);
	}

	public void ShowPopup(GameObject popupContent, string titleText, bool displayCloseButton){
		uiData.popupUI.SetActive (true);

		DeactivateAllChildren (uiData.popupPanel);

		uiData.popupHeader.SetActive (true);
		uiData.popupTitleText.text = titleText;
		popupContent.SetActive (true);

		uiData.closePopupButton.SetActive (displayCloseButton);

	}

	public void ShowTextPopup( string titleText, string innerText, bool displayCloseButton){
		uiData.popupUI.SetActive (true);

		DeactivateAllChildren (uiData.popupPanel);

		uiData.popupHeader.SetActive (true);
		uiData.popupTitleText.text = titleText;
		uiData.textPopupContent.SetActive (true);

		uiData.popupText.text = innerText;

	}
		

	void DeactivateAllChildren(GameObject parent){
		
		Transform[] transforms = parent.GetComponentsInChildren<Transform> ();
		foreach (Transform t in transforms) {
			if (t.gameObject == parent) {
				continue;
			}
			if (t.parent.gameObject == parent) {
				t.gameObject.SetActive (false);
			}
		}
	}

}
