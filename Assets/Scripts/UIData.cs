using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using GameSparks.Core;

public class UIData : MonoBehaviour {

	public Text inGameScore;
	public Text gameOverScore;
	public Text nextPrizeScore;
	public Text authName;
	public Text usernameLoginText;
	public Text passwordLoginText;
	public Text usernameRegisterText;
	public Text passwordRegisterText;

	public Text usernameMainMenuText;
	public Text highScoreText;
	public Text availabilityNotification;
	public Text signedInNotification;
	public Text connectionText;

	public Image signalImage;

	public Text zoinsText;

	public Button authBttn;
	public Button logoutBttn;

	public GameObject overlayPanel;

	public GameSparksManager gsm;

	public GameObject uiViewsContainer;

	public GameObject mainMenuUI;
	public GameObject gameOverUI;
	public GameObject highScoresUI;
	public GameObject topHUDUI;
	public GameObject gameUI;

	void Start () {

		UIManager.uiData = this;
		gsm = GameObject.Find ("GameSparksManager").GetComponent<GameSparksManager> ();
		GameSparksManager.GetZoins ();

	}

	//Accessible for buttons

	public void ShowMainMenu(){
		UIManager.instance.MenuUI ();
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

	public void ActivateOverlayPanel(bool activate){
		UIManager.instance.ActivateOverlayPanel (activate);
	}

}
