using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {


	public Text inGameScore;

	public Text gameOverScore;
	public Text nextPrizeScore;

	public GameObject mainMenuUI;
	public GameObject gameOverUI;
	public GameObject gameUI;

	public int pointsToNextPrize {
		get{ return 100 - GameManager.instance.currentPoints; } 
	}

	// Use this for initialization
	void Start () {

		GameManager.instance.CurrentState (GameStates.Mainmenu);
	
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
	}

	public void GameOverUI(){
		gameUI.SetActive (false);
		gameOverUI.SetActive (true);
		gameOverScore.text = GameManager.instance.currentPoints.ToString ();
		nextPrizeScore.text = pointsToNextPrize.ToString ();
	}

	public IEnumerator WaitAndDisplayScore() {
		yield return new WaitForSeconds(1.2f);
		GameOverUI ();
	}
}
