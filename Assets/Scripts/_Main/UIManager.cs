using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;
using System.Collections.Generic;
using Facebook;

public class UIManager : MonoBehaviour
{

	public static UIManager instance = null;

	public static UIData uiData;

	bool connectionLost = false;

	public int pointsToNextPrize {
		get{ return 100 - GameManager.currentPoints; } 
	}


	void Awake (){

		instance = this;
	}

	void Start (){

		uiData.zoinsText.text = GameManager.zoins.ToString ();

	}

	void Update (){

		uiData.scoreText.text = GameManager.currentPoints.ToString ();

	}

	public void DisplayFBPic (Sprite input){
		uiData.profilePic.sprite = input;
	}

	public void StartGame (){
		if (GameManager.zoins > 0) {
			if (!GameManager.instance.online) {
				ShowOfflineWarningPopup ();
			} else {
				PlayGame ();
			}
		}
	}
		

	public void PlayGame (){
		GameManager.instance.CurrentState (GameStates.PlayGame);
	}

	public void PrizeCountdown (){
		uiData.days.text = GameManager.daysUntilPrize.ToString ();
		uiData.hours.text = GameManager.hrsUntilPrize.ToString ();
		uiData.minutes.text = GameManager.minsUntilPrize.ToString ();
		uiData.seconds.text = GameManager.secsUntilPrize.ToString ();
		uiData.days1.text = uiData.days.text;
		uiData.hours1.text = uiData.hours.text;
		uiData.minutes1.text = uiData.minutes.text;
		uiData.seconds1.text = uiData.seconds.text;
	}


	public void ZoinCount (int amount){
		uiData.playButton.onClick.RemoveAllListeners ();
		foreach (Text t in uiData.playerZoins) {
			t.text = amount.ToString ();
		}
		if (amount < 1) {
			uiData.playButtonText.text = "GET MORE ZOINS";
			uiData.playButton.onClick.AddListener (ShowZoinShop);
		} else {
			uiData.playButtonText.text = "PLAY";
			uiData.playButton.onClick.AddListener (StartGame);
		}
	}

	public void InternetAccessNotification (bool connected){
		if (!connected) {
			ShowTextPopup ("Warning", "No internet access available", false);
			connectionLost = true;
		} else if (connectionLost) {
				ShowTextPopup ("Warning", "Connection found", true);
				connectionLost = false;
			}
	}

	public void ShowWelcomeUIPanel (GameObject iPanel){
		foreach (var panel in uiData.welcomeUIPanels) {
			panel.SetActive (false);
		}
		iPanel.SetActive (true);
	}

	public void DeactivateAllChildren (GameObject parent){
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

	public void DisplayInstantWinPanel (bool available){
		if (available) {
			uiData.instantAvailable.SetActive (true);
			uiData.instantUnavailable.SetActive (false);
			uiData.instantWinPanel.SetActive (true);
		} else {
			uiData.instantAvailable.SetActive (false);
			uiData.instantUnavailable.SetActive (true);
			uiData.instantWinPanel.SetActive (false);
		}
	}

	public void SetPlayerTopScore (int score){
		foreach (Text t in uiData.playerHighScore) {
			t.text = score.ToString ();
		}
	}

	public void SetTopScores (string topScore){
		foreach (Text t in uiData.topScores) {
			t.text = topScore.ToString ();
		}
	}

	public void SetCashPrizeScore (int input){
		uiData.cashPrizeScore.text = input.ToString ();
	}

	public void SetInstantWinText (int prizesLeft){
		uiData.instantPrizeText.text = "<color=#ffffffff>NEXT </color>" +
		prizesLeft.ToString () +
			"<color=#ffffffff> PLAYERS TO SCORE </color>" +
		GameManager.instance.instantCashScore.ToString ();

		uiData.instantWinPrizeAmountText.text = "<color=#ffffffff>WIN</color> £" +
		GameManager.instance.instantWinPrize.ToString () +
			"<color=#ffffffff> CASH</color>";

		//Sets prizes unavailable Text
		uiData.prizesWonText.text = "£" + GameManager.instance.instantWinPrize.ToString () + " PRIZES ALL WON";
	}


	//In Game

	public IEnumerator WaitAndDisplayGameOver (bool universalWin, bool instantWin){

		uiData.gameOverScore.text = GameManager.currentPoints.ToString ();
		uiData.scoreText.gameObject.SetActive (false);

		DisplayWinOrLose (false);

		yield return new WaitForSeconds (1.2f);
		ShowGameOver ();
		if (universalWin) {
			ShowWinningScorePopup (instantWin);
			DisplayWinOrLose (true);
			instantWin = false;
		}
		if (instantWin) {
			ShowInstantWinPopup ();
		}
	}

	public void ShowWinningScorePopup (bool instantWin){
		uiData.topScoreGameOverText.text = GameManager.currentPoints.ToString ();
		ShowPopup (uiData.winContent, "Win", true);

		if (instantWin) {
			uiData.popupScreenCoverButton.onClick.RemoveAllListeners ();
			uiData.popupScreenCoverButton.onClick.AddListener (ShowInstantWinPopup);
			uiData.closePopupButton.GetComponent<Button>().onClick.RemoveAllListeners ();
			uiData.closePopupButton.GetComponent<Button>().onClick.AddListener (ShowInstantWinPopup);
		}
	}

	void ShowInstantWinPopup (){
		ShowPopup (uiData.winInstantContent, "Instant Win", true);
		DisplayWinOrLose (true);
		uiData.instantCashPrizePopupText.text = "£ " +
		GameManager.instance.instantWinPrize.ToString ();

		DisplayInstantWinPanel (false);

		uiData.popupScreenCoverButton.onClick.RemoveAllListeners ();
		uiData.popupScreenCoverButton.onClick.AddListener (ClosePopup);
		uiData.closePopupButton.GetComponent<Button>().onClick.RemoveAllListeners ();
		uiData.closePopupButton.GetComponent<Button>().onClick.AddListener (ClosePopup);
	}

	public void DisplayWinOrLose (bool win){
		if (win) {
			uiData.winOrLoseText.text = "WELL DONE!";
		} else {
			uiData.winOrLoseText.text = "SOOO CLOSE!";
		}
	}

	//_______MAIN MENU

	public void ShowMenu (GameObject showUI){
		DeactivateAllChildren (uiData.uiViewsContainer);
		showUI.SetActive (true);
	}

	public void MainMenuUI (){
		ShowMenu (uiData.mainMenuUI);
		ShowPage ("home");
		uiData.secondCanvas.enabled = true;
		uiData.zoinsPanel.SetActive (true);

	}

	void ShowGameOver (){
		ShowMenu (uiData.mainMenuUI);
		ShowPage ("gameOver");
		uiData.background.color = new Color (1, 1, 1, 0.7f);
	}

	public void ShowZoinShop (){
		ShowPage ("zoins");
	}

	public void ShowWelcome (){
		ShowMenu (uiData.welcomeMenuUI);
		ShowWelcomeUIPanel (uiData.welcomeUIPanels [2]);
		uiData.secondCanvas.enabled = false;
	}

	public void ShowGameUI (){
		ShowMenu (uiData.gameUI);
		uiData.scoreText.gameObject.SetActive (true);
		uiData.zoinsPanel.SetActive (true);
	}

	public void ShowIntroTutorial (){
		ShowMenu (uiData.IntroTutorialUI);
		PreClosedSidePanel ();
	}

	public void ShowLoadingScreen (){
		ShowMenu (uiData.welcomeMenuUI);
		ShowWelcomeUIPanel (uiData.welcomeUIPanels [3]);
		uiData.secondCanvas.enabled = false;
	}

	//________POPUP Creation

	public void ShowPopup (GameObject popupContent, string titleText, bool displayCloseButton){
		ActivatePopup (titleText); 
		popupContent.SetActive (true);
		uiData.closePopupButton.SetActive (displayCloseButton);
	}

	void ActivatePopup (string titleText){
		uiData.popupUI.SetActive (true);
		DeactivateAllChildren (uiData.popupPanel);
		uiData.popupHeader.SetActive (true);
		uiData.popupTitleText.text = titleText;
	}

	//_________Show POPUP

	public void ShowTextPopup (string titleText, string innerText, bool displayCloseButton){
		ActivatePopup (titleText); 
		uiData.textPopupContent.SetActive (true);
		uiData.popupText.text = innerText;
	}

	public void ShowOfflineWarningPopup (){
		ShowPopup (uiData.offlineWarningContent, "Warning", false);
	}

	public void ClosePopup (){
		uiData.popupUI.SetActive (false);
	}

	public void ShowGameSparksActivityPopup (){
		uiData.gsActivityText.text = GameSparksManager.gsActivity;
		ShowPopup (uiData.gameSparksActivityContent, "Activity Monitor", true);
	}



	//OTHER

	public void IndicateGameSparksAvailability (bool available){
		if (available) {
			GameSparksAvailablility.color = Color.green;
			GameSparksAvailablility.text = "GS Available";
		} else {
			GameSparksAvailablility.color = Color.red;
			GameSparksAvailablility.text = "GS Unvailable";
		}
	}

	public void FacebookShare (){
		FaceBookGamesparks.instance.Share ();
	}

	public void Logout (){
		GameSparksManager.instance.LogOut ();
	}

	//_______SIDE PANEL

	public void OpenSidePanel (){
		uiData.sidePanel.GetComponent<Animator> ().SetTrigger ("Open");
		uiData.screenCoverButton.SetActive (true);
	}

	public void PreClosedSidePanel (){
		uiData.sidePanel.GetComponent<Animator> ().SetTrigger ("preClosed");
		uiData.screenCoverButton.SetActive (false);
	}

	//_______SIGNUP MENU

	public void ChangeTextColor (Text text){
		text.color = Color.yellow;
	}

	public void EditTextColor (Text text){
		text.color = Color.black;
	}

	public void ShowPage (string page){
		DeactivateAllChildren (uiData.pagesContainer);
		switch (page) {
		case "home":
			uiData.homePage.SetActive (true);
			GameManager.instance.UpdateGamesparks ();
			break;
		case "profile":
			uiData.profilePage.SetActive (true);
			SetProfilePic ();
			uiData.emailText.text = GameManager.email;
			uiData.usernameMainMenuText.text = GameManager.userName;
			uiData.totalGamesText.text = GameManager.instance.totalGamesPlayed.ToString ();
			if (GameManager.instance.totalGamesPlayed > 0) {
				uiData.averageScoreText.text = (GameManager.instance.totalScore / GameManager.instance.totalGamesPlayed).ToString ();
			} else {
				uiData.averageScoreText.text = "0";
			}
			break;
		case "rewind":
			uiData.rewindPage.SetActive (true);
			break;
		case "settings":
			uiData.settingsPage.SetActive (true);
			break;
		case "winners":
			uiData.winnersPage.SetActive (true);
			WinnerPageUI.instance.Activate ();
			break;
		case "winners2":
			uiData.winnersPage2.SetActive (true);
			WinnersPage2.instance.Init ();
			break;
		case "zoins":
			uiData.zoinsPage.SetActive (true);
			break;
		case "gameOver":
			uiData.gameOverPage.SetActive (true);
			return;
		}

		PreClosedSidePanel ();
	}

	void SetProfilePic (){
		Texture2D pic = GameManager.profilePic;
		Rect rec = new Rect (0, 0, pic.width, pic.height);
		Sprite fbSprite = Sprite.Create (pic, rec, new Vector2 ());

		uiData.profilePic.sprite = fbSprite;
	}

	public void SetTopScoreMessage(bool top, bool joint = false){
		if (top) {
			if (joint) {
				uiData.topScoreText.text = "YOU ARE JOINT FIRST!";
			} else {
				uiData.topScoreText.text = "YOU ARE CURRENTLY FIRST!";
			}
		} else {
			uiData.topScoreText.text = "SCORE TO BEAT";
		}
	}

	public void SetGrandPrize(string prize){
		uiData.grandPrizeText.text = prize;
	}

}
