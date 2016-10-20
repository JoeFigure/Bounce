using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public static GameplayController instance;
	public static BallScript ball;

	static float _speed = -3.8f;
	public static float gameTimeStart;
	static float _percentageOfFullSpeed;

	public static float gameTime {
		get{ return Time.time - gameTimeStart; }
	}

	public static float speed{
		get{ 
			float speedMult = (gameTimePercentOfFullSpeed * 2) + 1;

			if(ball.alive){
				return (_speed * speedMult) * Time.deltaTime;
			} else{
				return 0;
			}
		}
	}

	public static float gameTimePercentOfFullSpeed {
		get {
			int secondsToFullSpeed = 120;
			float _percentageOfFullSpeed = gameTime / secondsToFullSpeed;
			return _percentageOfFullSpeed;
		}
	}

	public static float screenHeight {
		get{ return Camera.main.orthographicSize * 2; }
	}

	public static float screenWidth{
		get{	
			float worldScreenHeight = Camera.main.orthographicSize * 2;
			float screenWidth = worldScreenHeight / Screen.height * Screen.width;
			return screenWidth;
		}
	}

	public static float zeroScreenX {
		get{ return (screenWidth / 2) * -1; }
	}

	void Start () {

		instance = this;
	}
	
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
