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
	//Static instance of GameManager which allows it to be accessed by any other script

	private GameStates currentState;

	UIScript ui;

	public BallScript ball;

	public bool gameStarted;

	public int currentPoints;

	public float gameTimeStart;

	public float gameTime {
		get{ return Time.time - gameTimeStart; }
	}

	float _speed = -5;

	public float speed{
		get{ 
			return (_speed * speedMult) ;
		}
		set{ _speed = value ; }
	}

	public float gameTimePercentOfFullSpeed{
		get {
			int secondsToFullSpeed = 120;
			float percentageToFullSpeed = gameTime / secondsToFullSpeed;
			return percentageToFullSpeed;
		}
	}

	float speedMult {
		get { 
			float percOfFullSpeed = gameTimePercentOfFullSpeed;
			float multiplier = 1.5f;
			return (percOfFullSpeed * multiplier) + 1;
		}
	}

	//public int score;

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
		//print (gameTimeStart);
	}

	public void CurrentState (GameStates currentState)
	{

		ui = GameObject.Find ("UI").GetComponent<UIScript>();
		ball = GameObject.Find ("Ball").GetComponent<BallScript>();


		switch (currentState) {

		case GameStates.Mainmenu:

			ui.MenuUI ();
			currentPoints = 0;

			gameStarted = false;

			break;

		case GameStates.PlayGame:

			ui.PlayUI ();
			ball.ResetBounce ();

			gameStarted = true;

			break;

		case GameStates.GameOver:
			ui.GameOverUI ();
			break;

		default:
			break;
		}
	}

	public void ResetGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void RecordStartTime(){
		gameTimeStart = Time.time;
	}
		
}
