using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {


	public Text inGameScore;

	public GameObject mainMenuUI;

	public GameObject gameUI;

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
	}

	public void PlayUI(){

		mainMenuUI.SetActive (false);
		gameUI.SetActive (true);
	}

	public void MenuUI(){
		gameUI.SetActive (false);
	}
}
