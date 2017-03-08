using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine.UI;
using GameSparks.Core;

public class UIData : MonoBehaviour {
	[Header("2nd Canvas layer")]
	public Canvas secondCanvas;
	[Header("Main Menu")]
	public GameObject overlayPanel;
	public Text days, hours, minutes, seconds;
	public Text days1, hours1, minutes1, seconds1;
	public Image background;
	public Button playButton;
	public Text cashPrizeScore, winOrLoseText;
	public Text grandPrizeText;
	public Text topScoreText;
	public Text prizesWonText;
	public Text playButtonText , instantPrizeText, instantWinPrizeAmountText;
	public GameObject topScorePanel1, topScorePanel2, offlinePanel1, offlinePanel2;
	public GameObject instantAvailable, instantUnavailable;
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
	public GameObject instantWinPanel;
	[Header("Profile")]
	public Text usernameMainMenuText;
	public Text totalGamesText;
	public Text averageScoreText;
	public Image profilePic;
	public Text emailText;
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
	public GameObject welcomeLogo;
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
	public GameObject winnersPage2;
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
	public Button popupScreenCoverButton;
	[Header("Popup Content")]
	public GameObject offlineWarningContent;
	public GameObject winContent;
	public GameObject winInstantContent;
	public GameObject textPopupContent;
	public GameObject playGameContent;
	public GameObject gameSparksActivityContent;
	public Text gsActivityText;
	public Text popupText, instantCashPrizePopupText;

	[Header("Menu Sounds")]
	public AudioClip startGameSfx;
	public AudioClip buttonSfx;

	[Header("Settings Menu")]
	public Toggle soundToggle;

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
		overlayPanel.SetActive (true);
	}

	public void PlayGame(){
		UIManager.instance.PlayGame ();
	}

	public void ShowIntroTutorial(){
		UIManager.instance.ShowIntroTutorial ();
	}

	public void ShowRewardedAd(){
		UnityAds.ShowRewardedAd ();
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
	}

	public void ShowSidePanel(){
		UIManager.instance.OpenSidePanel ();
	}

	public void CloseSidePanel(){
		UIManager.instance.PreClosedSidePanel ();
	}

	public void TandCPanel(){
		Application.OpenURL("http://mazoin.com/terms-conditions");
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

	public void Scroll(){
		Debug.Log ("SCROLLING");
	}

	public void StartGameSound(){
		playSound (startGameSfx);
	}
	public void ButtonSound(){
		playSound (buttonSfx);
	}

	void playSound(AudioClip sound){
		AudioSource a = GetComponent<AudioSource> ();
		a.mute = GameManager.instance.sfxMuted;

		a.clip = sound;
		a.Play ();
	}

	public void muteSfx(bool mute){
		GameManager.instance.sfxMuted = !mute;
	}

}
