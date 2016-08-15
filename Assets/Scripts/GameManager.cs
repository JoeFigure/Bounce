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

	public static bool signedIn = false;

	//static UIManager ui;

	public static BallScript ball;

	//public bool gameStarted;

	static int _zoins;

	public static int zoins {
		get{ return _zoins; }
		set {
			_zoins = value;
			UIManager.instance.ZoinCount (_zoins);
		}
	}

	public static int currentPoints;

	public static float gameTimeStart;

	public static float gameTime {
		get{ return Time.time - gameTimeStart; }
	}

	static float _speed = -3.8f;

	public static float speed{
		get{ 
			if(ball.alive){
				return (_speed * speedMult) * Time.deltaTime;
			} else{
				return 0;
			}
		}
		set{ _speed = value ; }
	}

	public static float gameTimePercentOfFullSpeed{
		get {
			int secondsToFullSpeed = 120;
			float percentageToFullSpeed = gameTime / secondsToFullSpeed;
			return percentageToFullSpeed;
		}
	}

	static float speedMult {
		get { 
			float percOfFullSpeed = gameTimePercentOfFullSpeed;
			float multiplier = 2;
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
	
		currentState = GameStates.Mainmenu;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void CurrentState (GameStates currentState)
	{

		ball = GameObject.Find ("Ball").GetComponent<BallScript>();


		switch (currentState) {

		case GameStates.Mainmenu:

			UIManager.instance.MenuUI ();

			currentPoints = 0;

			break;

		case GameStates.PlayGame:

			ball.alive = true;
			ball.ResetBounce ();

			UIManager.instance.ZoinCount (zoins);

			break;

		case GameStates.GameOver:
			StartCoroutine (UIManager.instance.WaitAndDisplayScore ());
			GameSparksManager.GameOver ();
			zoins--;
			break;

		default:
			break;
		}
	}

	public void ResetGame(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		currentState = GameStates.Mainmenu;

	}

	public void RecordStartTime(){
		gameTimeStart = Time.time;
	}
		
}
