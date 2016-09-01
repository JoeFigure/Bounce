using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;
using GameSparks.Core;

public class UIData : MonoBehaviour {

	public Text scoreText;
	public Text gameOverScore;
	public Text nextPrizeScore;
	public Text usernameLoginText;
	public Text passwordLoginText;
	public Text usernameSignupText;
	public Text passwordSignupText;

	public Text usernameMainMenuText;
	public Text highScoreText;
	public Text availabilityNotification;
	public Text signedInNotification;
	public Text connectionText;

	public Text savedScoreText;
	public Text lastDateText;
	public Text todayDateText;
	public Text lastGameText;

	public Text popupTitleText;
	public Text popupText;

	public Image signalImage;

	public Text zoinsText;

	//public GameObject overlayPanel;
	public GameObject uiViewsContainer;

	public GameObject mainMenuUI;
	public GameObject highScoresUI;
	public GameObject topHUDUI;
	public GameObject gameUI;
	public GameObject welcomeMenuUI;

	public List<GameObject> welcomeUIPanels = new List<GameObject> ();

	public GameObject popupUI;
	public GameObject popupPanel;
	public GameObject popupHeader;
	public GameObject closePopupButton;

	public GameObject rewardPopupContent;
	public GameObject shopPopupContent;
	public GameObject gameOverPopupContent;
	public GameObject settingsPopupContent;
	public GameObject textPopupContent;

	public float popupHeaderHeight{
		get{ 
			RectTransform popupHeaderRectTrans = popupHeader.GetComponent<RectTransform> ();
			return popupHeaderRectTrans.rect.height;
		}
	}



	void Start () {

		UIManager.uiData = this;
	}

	//Accessible for buttons

	public void ShowMainMenu(){
		UIManager.instance.MainMenuUI ();
	}

	public void ShowMenu(GameObject input){
		UIManager.instance.ShowMenu (input);
	}

	public void StartGame(){
		UIManager.instance.StartGame();
	}

	public void Replay(){
		UIManager.instance.ReplayBttn ();
	}
	/*
	public void ActivateOverlayPanel(bool activate){
		UIManager.instance.ActivateOverlayPanel (activate);
	}
*/
	public void SignOut(){
		UIManager.instance.SignOut ();
	}

	public void ClosePopup(){
		popupUI.SetActive (false);
	}

	public void ShowTopHUDUI(){
		topHUDUI.SetActive (true);
	}

	public void ShowHighScores(){
		UIManager.instance.ShowHighScores ();
	}

	public void ShowWelcomeUIPanel(GameObject panel){
		UIManager.instance.ShowWelcomeUIPanel (panel);
	}

	public void ShowPopup(){
		UIManager.instance.ShowPopup (rewardPopupContent, "Daily Reward", true);
	}

	public void ShowShopPopup(){
		UIManager.instance.ShowPopup (shopPopupContent, "Buy Zoins", true);
	}
	/*
	public void ShowGamoverPopup(){
		UIManager.instance.ShowPopup (gameOverPopupContent, "Game Over", false );
	}
*/
	public void ShowSettingsPopup(){
		UIManager.instance.ShowPopup (settingsPopupContent, "Settings", true);
	}
}
