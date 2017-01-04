using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;
using GameSparks.Core;

public class UIData : MonoBehaviour {
	[Header("2nd Canvas layer")]
	public Canvas secondCanvas;
	[Header("Main Menu")]
	//public Text topScorerNameText;
	public GameObject overlayPanel;
	public Text days, hours, minutes, seconds;
	public Text days1, hours1, minutes1, seconds1;
	public Image background;
	public Text cashPrizeScore, winOrLoseText;
	public Text playButtonText;
	public GameObject topScorePanel1, topScorePanel2, offlinePanel1, offlinePanel2;
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
	public Text retypePasswordSignupText;
	public InputField passwordSignup, retypePasswordSignup;
	public InputField dateDay, dateMonth, dateYear;
	public Button loginButton;
	public GameObject tandCPanel;
	public Toggle tAndCToggle, maleToggle, femaleToggle;
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
	public GameObject gameOverPage;
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
	public GameObject offlineWarningContent;
	public GameObject winContent;
	public GameObject winInstantContent;
	public GameObject textPopupContent;
	public GameObject playGameContent;
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

		Signup.uiData = this;

		foreach (Text t in playerHighScore) {
			t.text = " ";
		}
	}

	public void ShowMainMenu(){
		GameManager.instance.CurrentState (GameStates.Mainmenu);
		UIManager.instance.MainMenuUI ();
	}

	public void StartGame(){
		UIManager.instance.StartGame();
	}

	public void ClosePopup(){
		UIManager.instance.ClosePopup ();
	}

	public void ShowWelcomeUIPanel(GameObject panel){ 
		UIManager.instance.ShowWelcomeUIPanel (panel);
	}

	public void ActivateOverlayPanel(){
		UIManager.instance.ActivateOverlayPanel (true);
	}

	public void PlayGame(){
		UIManager.instance.PlayGame ();
	}

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

	public void TandCPanel(){
		UIManager.instance.TandCPanel ();
	}

	public void ShowPage(string page){
		UIManager.instance.ShowPage (page);
	}

	public void ChangeTextColor(Text text){
		UIManager.instance.ChangeTextColor (text);
	}
	public void EditTextColor(Text text){
		UIManager.instance.EditTextColor (text);
	}
	public void ResetGame(){
		GameManager.instance.ResetGame ();
	}


}
