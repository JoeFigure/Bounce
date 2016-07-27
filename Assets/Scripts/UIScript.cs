using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {


	public Text inGameScore;

	public Text gameOverScore;
	public Text nextPrizeScore;

	public Text authName;

	public Button authBttn;

	public GameSparksManager gsm;

	public GameObject mainMenuUI;
	public GameObject gameOverUI;
	public GameObject gameUI;
	public GameObject loginUI;

	public int pointsToNextPrize {
		get{ return 100 - GameManager.instance.currentPoints; } 
	}

	// Use this for initialization
	void Start () {

		GameManager.instance.CurrentState (GameStates.Mainmenu);

		authBttn.GetComponent<Button>().onClick.AddListener(() => { gsm.Authorise(); }); 
	
	}
	
	// Update is called once per frame
	void Update () {

		inGameScore.text = GameManager.instance.currentPoints.ToString();
	
	}

	public void StartGameBttn(){
		GameManager.instance.CurrentState (GameStates.PlayGame);
		GameManager.instance.RecordStartTime ();
	}

	public void ReplayBttn(){
		GameManager.instance.ResetGame();
	}

	public void PlayUI(){

		mainMenuUI.SetActive (false);
		gameUI.SetActive (true);
	}

	public void MenuUI(){
		gameUI.SetActive (false);
		mainMenuUI.SetActive (true);
		gameOverUI.SetActive (false);
		loginUI.SetActive (false);
	}

	public void GameOverUI(){
		gameUI.SetActive (false);
		gameOverUI.SetActive (true);
		gameOverScore.text = GameManager.instance.currentPoints.ToString ();
		nextPrizeScore.text = pointsToNextPrize.ToString ();
	}

	public void LoginUI(){
		loginUI.SetActive (true);
		mainMenuUI.SetActive (false);
	}

	public IEnumerator WaitAndDisplayScore() {
		yield return new WaitForSeconds(1.2f);
		GameOverUI ();
	}

	public void AuthName(string name){
		authName.text = name;
	}
}
