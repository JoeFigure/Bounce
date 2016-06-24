using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum GameStates
{

	Mainmenu,
	PlayGame,
	GameOver
}

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	//Static instance of GameManager which allows it to be accessed by any other script.

	private GameStates currentState;

	UIScript ui;
	BallScript ball;
	SpikeGeneratorScript spikes;

	public int currentPoints;

	void Awake ()
	{

		//Check if instance already exists
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);  
		}

		DontDestroyOnLoad (gameObject);

	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CurrentState (GameStates currentState)
	{

		ui = GameObject.Find ("UI").GetComponent<UIScript>();
		ball = GameObject.Find ("Ball").GetComponent<BallScript>();
		spikes = GameObject.Find ("SpikeGenerator").GetComponent<SpikeGeneratorScript>();


		switch (currentState) {

		case GameStates.Mainmenu:

			ui.MenuUI ();
			currentPoints = 0;

			break;

		case GameStates.PlayGame:

			ui.PlayUI ();
			ball.gameStarted = true;
			ball.ResetBounce ();
			spikes.gameStarted = true;

			break;

		case GameStates.GameOver:

			break;

		default:
			break;
		}
	}

	public void ResetGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
