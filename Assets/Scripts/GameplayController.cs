using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;
	public static BallScript ball;

	static float _speed = -3.8f;
	public static float gameTimeStart;

	public static float gameTime {
		get{ return Time.time - gameTimeStart; }
	}

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


	// Use this for initialization
	void Start () {

		instance = this;
	}
	
	// Update is called once per frame
	void Update () {

		TouchCustom.Tap();
		TouchInput();

		KeyboardInput ();
	}

	public void StartGame(){
		ball.alive = true;
		ball.ResetBounce ();
	}

	public void GameOver(){

		GameManager.instance.CurrentState (GameStates.GameOver);
	}

	void TouchInput ()
	{
		if (TouchCustom.tapped) {
			ball.beenHit = true;
		}
	}

	void KeyboardInput(){
		if (Input.GetButtonDown ("Jump")) {
			ball.beenHit = true;
		}
	}

	public void RecordStartTime(){
		gameTimeStart = Time.time;
	}
}
