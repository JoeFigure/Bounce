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

	public static bool tAndCChecked = false;

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
	void Start (){

		uiData.zoinsText.text = GameManager.zoins.ToString ();
	}
	
	// Update is called once per frame
	void Update (){

		uiData.scoreText.text = GameManager.currentPoints.ToString ();
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

	public void ReplayBttn (){
		GameManager.instance.ResetGame ();
	}

	public void ZoinCount (int amount){
		uiData.zoinsText.text = amount.ToString ();
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

	public void ActivateOverlayPanel (bool activate){
		uiData.overlayPanel.SetActive (activate);
	}

	public void SetPlayerTopScore (int score){
		foreach (Text t in uiData.playerHighScore) {
			t.text = score.ToString ();
		}
	}

	public void EnableLoginButton (bool disable){
		uiData.loginButton.interactable = disable;
	}


	public void SetTopScores (string topScore){
		uiData.topScorerNameText.text = GameManager.instance.topPlayerNames [0];

		foreach (Text t in uiData.topScores) {
			t.text = topScore.ToString();
		}
	}

	//In Game

	public IEnumerator WaitAndDisplayScore (){

		uiData.gameOverScore.text = GameManager.currentPoints.ToString ();
		uiData.scoreText.gameObject.SetActive (false);

		yield return new WaitForSeconds (1.2f);
		ShowPopup (uiData.gameOverPopupContent, "Game Over", false);

	}

	public IEnumerator WaitAndDisplayWinningScore (){

		yield return new WaitForSeconds (1.2f);
		ShowPopup (uiData.winContent, "Win", false);

	}

	//Menu

	public void ShowMenu (GameObject showUI){
		DeactivateAllChildren (uiData.uiViewsContainer);
		showUI.SetActive (true);
	}

	public void MainMenuUI (){
		ShowMenu (uiData.mainMenuUI);
		uiData.sidePanel.SetActive(true);
		PreClosedSidePanel ();
	}

	public void ShowWelcome (){
		ShowMenu (uiData.welcomeMenuUI);
		ShowWelcomeUIPanel (uiData.welcomeUIPanels [2]);
	}

	public void ShowGameUI (){
		ShowMenu (uiData.gameUI);
	}

	public void ShowSettings (GameObject settingsUI, Text scoreText){

		ShowMenu (settingsUI);
	}

	public void ShowIntroTutorial (){
		ShowMenu (uiData.IntroTutorialUI);
	}

	public void ShowLoadingScreen (){
		ShowMenu (uiData.welcomeMenuUI);
		ShowWelcomeUIPanel (uiData.welcomeUIPanels [3]);
	}

	//POPUP Creation

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

	//Show POPUP

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

	public void DailyRewardPopup (){
		ShowPopup (uiData.rewardPopupContent, "Daily Reward", true);
	}

	public void ShowPreviousWinnerPopup (){
		ShowPopup (uiData.previousWinnerContent, "Prize Winner", true);
	}

	public void ShowGrandPrizePopup (){
		ShowPopup (uiData.grandPrizeContent, "Prize", true);
	}

	public void ShowGameSparksActivityPopup (){
		uiData.gsActivityText.text = GameSparksManager.gsActivity;
		ShowPopup (uiData.gameSparksActivityContent, "Activity Monitor", true);
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

	public void TandCTickBox(){
		Image tImage = uiData.tAndCTickBox.gameObject.GetComponent<Image>();
		Sprite ticked = Resources.Load<Sprite> ("tickBox_Ticked");
		Sprite unticked = Resources.Load<Sprite> ("tickBox");



		if (tImage.sprite == unticked) {
			tImage.sprite = ticked;
			tAndCChecked = true;
		} else {
			tImage.sprite = unticked;
			tAndCChecked = false;
		}
	}

	public void TandCPanel(){
		if (uiData.tandCPanel.activeSelf) {
			uiData.tandCPanel.SetActive (false);
		} else {
			uiData.tandCPanel.SetActive (true);
		}
	}

}
