using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;
using System.Collections.Generic; 


public class UIManager : MonoBehaviour {

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
		if (!GameManager.instance.online) {
			ShowOfflineWarningPopup ();
		} else {
				PlayGame ();
			}
		}
	}

	public void PlayGame(){
		GameManager.instance.CurrentState (GameStates.PlayGame);
	}

	public void ReplayBttn(){
		GameManager.instance.ResetGame();
	}

	public void MainMenuUI(){

		ShowMenu (uiData.mainMenuUI);
		uiData.topHUDUI.SetActive (true);

		ShowPopup (uiData.playGameContent, "This months game", false);
	}

	public void ShowWelcome(){
		ShowMenu (uiData.welcomeMenuUI);
		ShowWelcomeUIPanel (uiData.welcomeUIPanels [2]);
	}

	public void ShowGameUI(){
		ShowMenu (uiData.gameUI);
	}

	public void ShowSettings(GameObject settingsUI, Text scoreText){

		ShowMenu (settingsUI);
	}

	public void ShowIntroTutorial(){
		ShowMenu (uiData.IntroTutorialUI);
		/*
		foreach(var slide in uiData.introUISlides){
			slide.SetActive (false);
		}
		uiData.introUISlides [0].SetActive (true);
		*/
	}
		
	public void ShowMenu(GameObject showUI){

		DeactivateAllChildren (uiData.uiViewsContainer);
		showUI.SetActive (true);
	}

	public IEnumerator WaitAndDisplayScore() {
		
		uiData.gameOverScore.text = GameManager.currentPoints.ToString ();
		uiData.scoreText.gameObject.SetActive (false);

		yield return new WaitForSeconds(1.2f);
		ShowPopup(uiData.gameOverPopupContent,"Game Over",false);

	}

	public IEnumerator WaitAndDisplayWinningScore() {

		yield return new WaitForSeconds(1.2f);
		ShowPopup(uiData.winContent,"Win",false);

	}

	public void ZoinCount(int amount){
		uiData.zoinsText.text = amount.ToString ();
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

	public void ShowWelcomeUIPanel(GameObject iPanel){
		foreach(var panel in uiData.welcomeUIPanels){
			panel.SetActive (false);
		}
		iPanel.SetActive (true);
	}

	public void DailyRewardPopup(){
		ShowPopup (uiData.rewardPopupContent, "Daily Reward", true);
	}

	public void ShowGrandPrizePopup(){
		ShowPopup (uiData.grandPrizeContent, "Prize", true);
	}

	public void ShowPopup(GameObject popupContent, string titleText, bool displayCloseButton){
		ActivatePopup (titleText); 
		popupContent.SetActive (true);
		uiData.closePopupButton.SetActive (displayCloseButton);
	}

	public void ShowTextPopup( string titleText, string innerText, bool displayCloseButton){
		ActivatePopup (titleText); 
		uiData.textPopupContent.SetActive (true);
		uiData.popupText.text = innerText;

	}

	void ActivatePopup(string titleText){
		uiData.popupUI.SetActive (true);
		DeactivateAllChildren (uiData.popupPanel);
		uiData.popupHeader.SetActive (true);
		uiData.popupTitleText.text = titleText;
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

	public void ActivateOverlayPanel(bool activate){
		uiData.overlayPanel.SetActive (activate);
	}

	public void SetPlayerTopScore(int score){
		foreach (Text t in uiData.playerHighScore) {
			t.text = score.ToString();
		}
	}


	public void EnableLoginButton(bool disable){
		uiData.loginButton.interactable = disable;
	}

	public void ShowLoadingScreen(){
		ShowMenu (uiData.welcomeMenuUI);
		ShowWelcomeUIPanel (uiData.welcomeUIPanels [3]);
	}

	public void ClosePopup(){
		uiData.popupUI.SetActive (false);
		if (GameManager.instance.currentState == GameStates.Mainmenu) {
			MainMenuUI ();
		}
	}

	public void ShowOfflineWarningPopup(){
		ShowPopup (uiData.offlineWarningContent, "Warning", false);
	}

	public void SetTopScores(string topScore){
		uiData.topScorerNameText.text = GameManager.instance.topPlayerNames [0];
		//uiData.topScoreText.text = GameManager.instance.universalTopScore;

		foreach (Text t in uiData.topScores) {
			t.text = topScore;
		}
	}
}
