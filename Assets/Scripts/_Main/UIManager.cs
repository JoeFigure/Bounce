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
		set{ uiData.usernameMainMenuText.text = value; }
	}

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
		PrizeCountdown ();
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

	void PrizeCountdown(){
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
		foreach (Text t in uiData.playerZoins) {
			t.text = amount.ToString();
		}
		if (amount < 1) {
			uiData.playButtonText.text = "OUT OF ZOINS";
		} else {
			uiData.playButtonText.text = "PLAY";
		}
	}

	public void InternetAccessNotification (bool connected){

		if (connected) {
			InternetAvailable.color = Color.green;
			InternetAvailable.text = "Online";
		} else {
			InternetAvailable.color = Color.red;
			InternetAvailable.text = "Offline - Prizes currently unavailable";
		}
	}

	public void ShowWelcomeUIPanel (GameObject iPanel){
		foreach (var panel in uiData.welcomeUIPanels) {
			panel.SetActive (false);
		}
		iPanel.SetActive (true);
	}

	void DeactivateAllChildren (GameObject parent){
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

	public void SwitchPlayBttnFunction(int zoins){
		uiData.playButton.onClick.RemoveAllListeners ();
		if (zoins > 0) {
			uiData.playButton.onClick.AddListener(StartGame);
		} else {
			uiData.playButton.onClick.AddListener(ShowZoinShop);
		}
	}

	public void ActivateOverlayPanel (bool activate){
		uiData.overlayPanel.SetActive (activate);
	}

	public void SetPlayerTopScore (int score){
		foreach (Text t in uiData.playerHighScore) {
			t.text = score.ToString ();
		}
	}
	/*
	public void EnableLoginButton (bool disable){
		uiData.loginButton.interactable = disable;
	}
*/

	public void SetTopScores (string topScore){
		//uiData.topScorerNameText.text = GameManager.instance.topPlayerNames [0];
		foreach (Text t in uiData.topScores) {
			t.text = topScore.ToString();
		}
	}

	public void SetCashPrizeScore(int input){
		uiData.cashPrizeScore.text = input.ToString();
	}

	public void DisplayWinOrLose(bool win){
		if (win) {
			uiData.winOrLoseText.text = "WELL DONE!";
		} else {
			uiData.winOrLoseText.text = "SOOO CLOSE!";
		}
	}

	//In Game

	public IEnumerator WaitAndDisplayGameOver (){

		uiData.gameOverScore.text = GameManager.currentPoints.ToString ();
		uiData.scoreText.gameObject.SetActive (false);

		yield return new WaitForSeconds (1.2f);
		ShowGameOver();

		if (GameManager.currentPoints > GameManager.instance.universalTopScore) {
			ShowWinningScorePopup ();
		} else if (GameManager.currentPoints > GameManager.instance.instantCashScore) {
				ShowPopup (uiData.winInstantContent, "Instant Win", true);
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
		PreClosedSidePanel ();
		uiData.zoinsPanel.SetActive (true);
	}
	/*
	public void OfflineMainMenu(){
		uiData.topScorePanel1.SetActive (false);
		uiData.topScorePanel2.SetActive (false);
		uiData.offlinePanel1.SetActive (true);
		uiData.offlinePanel2.SetActive (true);
	}
	*/

	void ShowGameOver(){
		ShowMenu (uiData.mainMenuUI);
		ShowPage ("gameOver");
		uiData.background.color = new Color (1, 1, 1, 0.7f);
	}

	public void ShowZoinShop(){
		//ShowMenu (uiData.mainMenuUI);
		ShowPage ("zoins");
		//uiData.background.color = new Color (1, 1, 1, 0.7f);
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
		PreClosedSidePanel();
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

	public void ShowWinningScorePopup (){
		uiData.topScoreGameOverText.text = GameManager.currentPoints.ToString ();
		ShowPopup (uiData.winContent, "Win", true);
	}


	//Other

	public void ShowRewardedAd (){
		UnityAds.ShowRewardedAd ();
	}

	public void ShowAd (){
		UnityAds.ShowAd ();
	}

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

	public void Logout(){
		GameSparksManager.instance.LogOut ();
	}

	//_______SIDE PANEL

	public void OpenSidePanel(){
		uiData.sidePanel.GetComponent<Animator> ().SetTrigger ("Open");
		uiData.screenCoverButton.SetActive (true);
	}

	public void CloseSidePanel(){
		uiData.sidePanel.GetComponent<Animator> ().SetTrigger ("Close");
		uiData.screenCoverButton.SetActive (false);
	}


	public void PreClosedSidePanel(){
		uiData.sidePanel.GetComponent<Animator> ().SetTrigger ("preClosed");
		uiData.screenCoverButton.SetActive (false);
	}

	//_______SIGNUP MENU

	public void TandCPanel(){
		if (uiData.tandCPanel.activeSelf) {
			uiData.tandCPanel.SetActive (false);
		} else {
			uiData.tandCPanel.SetActive (true);
		}
	}

	public void ChangeTextColor(Text text){
		text.color = Color.yellow;
	}
	public void EditTextColor(Text text){
		text.color = Color.black;
	}

	public void ShowPage(string page){
		DeactivateAllChildren (uiData.pagesContainer);
		switch (page) {
		case "home":
			uiData.homePage.SetActive (true);
			break;
		case "profile":
			uiData.profilePage.SetActive (true);
			break;
		case "rewind":
			uiData.rewindPage.SetActive (true);
			break;
		case "settings":
			uiData.settingsPage.SetActive (true);
			break;
		case "winners":
			uiData.winnersPage.SetActive (true);
			break;
		case "zoins":
			uiData.zoinsPage.SetActive (true);
			break;
		case "gameOver":
			uiData.gameOverPage.SetActive (true);
			PreClosedSidePanel ();
			return;
		}

		CloseSidePanel ();
	}

}
