using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;
using GameSparks.Core;

public class UIData : MonoBehaviour {

	[Header("Main Menu")]
	public Text topScorerNameText;
	public GameObject overlayPanel;
	[Header("Side Panel")]
	public GameObject sidePanel;
	public GameObject screenCoverButton;
	[Header("HUD Top of screen")]
	public Text zoinsText;
	public GameObject zoinsPanel;
	[Header("In Game")]
	public Text scoreText;
	[Header("Game Over")]
	public Text gameOverScore;
	public Text topScoreGameOverText;
	[Header("Profile")]
	public Text usernameMainMenuText;
	[Header("Welcome Menus")]
	public Text usernameLoginText;
	public Text passwordLoginText;
	public Text usernameSignupText;
	public Text passwordSignupText;
	public Button loginButton;
	public Button tAndCTickBox;
	public GameObject tandCPanel;
	[Header("Lists")]
	public List<GameObject> welcomeUIPanels = new List<GameObject> ();
	public List<Text> playerHighScore = new List<Text> ();
	public List<Text> topScores = new List<Text> ();
	public List<Text> playerZoins = new List<Text> ();
	[Header("UI Pages")]
	public GameObject pagesContainer;
	public GameObject homePage;
	public GameObject profilePage;
	public GameObject winnersPage;
	public GameObject zoinsPage;
	public GameObject rewindPage;
	public GameObject settingsPage;
	[Header("UI Containers")]
	public GameObject uiViewsContainer;
	public GameObject mainMenuUI;
	public GameObject gameUI;
	public GameObject welcomeMenuUI;
	public GameObject IntroTutorialUI;
	[Header("Popup Container")]
	public GameObject popupUI;
	public GameObject popupPanel;
	public GameObject popupHeader;
	public GameObject closePopupButton;
	public Text popupTitleText;
	[Header("Popup Content")]
	public GameObject rewardPopupContent;
	//public GameObject shopPopupContent;
	public GameObject gameOverPopupContent;
	//public GameObject settingsPopupContent;
	public GameObject textPopupContent;
	public GameObject playGameContent;
	public GameObject grandPrizeContent;
	//public GameObject profileContent;
	public GameObject topScoreContent;
	public GameObject topScorersContent;
	public GameObject offlineWarningContent;
	public GameObject winContent;
	public GameObject previousWinnerContent;
	public GameObject gameSparksActivityContent;
	public Text gsActivityText;
	public Text popupText;


	public float popupHeaderHeight{
		get{ 
			RectTransform popupHeaderRectTrans = popupHeader.GetComponent<RectTransform> ();
			return popupHeaderRectTrans.rect.height;
		}
	}

	void Start () {

		UIManager.uiData = this;

		foreach (Text t in playerHighScore) {
			t.text = " ";
		}
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

	public void ClosePopup(){
		UIManager.instance.ClosePopup ();
	}

	public void ShowWelcomeUIPanel(GameObject panel){ 
		UIManager.instance.ShowWelcomeUIPanel (panel);
	}

	public void ShowPopup(){
		UIManager.instance.ShowPopup (rewardPopupContent, "Daily Reward", true);
	}
	/*
	public void ShowTopscorePopup(){
		UIManager.instance.ShowPopup (topScoreContent, "Top Score", true);
	}
*/
	public void ShowGrandPrizePopup(){
		UIManager.instance.ShowGrandPrizePopup ();
	}

	public void ActivateOverlayPanel(){
		UIManager.instance.ActivateOverlayPanel (true);
	}

	/*
	public void EnableLoginButton(bool disable){
		UIManager.instance.EnableLoginButton (disable);
	}
*/
	public void PlayGame(){
		UIManager.instance.PlayGame ();
	}
	/*
	public void ShowTopscorersPopup(){
		UIManager.instance.ShowPopup (topScorersContent, "Top Score", true);
	}
*/
	public void ShowIntroTutorial(){
		UIManager.instance.ShowIntroTutorial ();
	}

	public void ShowAd(){
		UIManager.instance.ShowAd ();
	}

	public void ShowRewardedAd(){
		UIManager.instance.ShowRewardedAd ();
	}

	public void ShowGameSparksActivityPopup(){
		UIManager.instance.ShowGameSparksActivityPopup ();
	}

	public void FaceBookInit(){
		FaceBookGamesparks.instance.CallFBLogin ();
	}

	public void FacebookShare(){
		UIManager.instance.FacebookShare ();
	}

	public void Logout(){
		UIManager.instance.Logout ();
		UIManager.instance.EnableLoginButton (true);
	}

	public void ShowSidePanel(){
		UIManager.instance.OpenSidePanel ();
	}

	public void CloseSidePanel(){
		UIManager.instance.CloseSidePanel ();
	}

	public void TandCBox(){
		UIManager.instance.TandCTickBox ();
	}

	public void TandCPanel(){
		UIManager.instance.TandCPanel ();
	}

	public void ShowPage(string page){
		UIManager.instance.ShowPage (page);
	}

}
