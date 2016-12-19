using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour
{

	public static GameplayController instance;
	public static BallScript ball;
	public static SpikeGeneratorScript spikes;
	public static HillScript hills;
	public static InGameTutorial tutorial;


	static float _speed = -3.8f;
	public static float gameTimeStart;
	static float _percentageOfFullSpeed;

	bool showSpikeTutorial = true;

	public static float gameTime {
		get{ return Time.time - gameTimeStart; }
	}

	public static float speed {
		get { 
			float speedMult = (gameTimePercentOfFullSpeed * 2) + 1;

			if (ball.alive) {
				return (_speed * speedMult) * Time.deltaTime;
			} else {
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

	public static float screenWidth {
		get {	
			float worldScreenHeight = Camera.main.orthographicSize * 2;
			float screenWidth = worldScreenHeight / Screen.height * Screen.width;
			return screenWidth;
		}
	}

	public static float zeroScreenX {
		get{ return (screenWidth / 2) * -1; }
	}

	void Start (){
		instance = this;
	}

	void Update (){
		TouchCustom.Tap ();
		TouchInput ();
		KeyboardInput ();

		if (ball.alive) {
			Tutorial ();
		}
	}

	public void StartGame (){

		Random.InitState (LevelDesign.randomSeed);

		hills.Init ();
		spikes.Init ();

		ball.InitialSetup ();
		ball.alive = true;
	}

	public void GameOver (){
		GameManager.instance.CurrentState (GameStates.GameOver);

	}

	void TouchInput (){
		if (TouchCustom.tapped) {
			HitBall ();
		}
	}

	void KeyboardInput (){
		if (Input.GetButtonDown ("Jump")) {
			HitBall ();
		}
	}

	void HitBall (){
		ball.beenHit = true;
	}

	public void RecordStartTime (){
		gameTimeStart = Time.time;
	}

	void Tutorial (){
		if (showSpikeTutorial == true) {
			if (GameManager.currentPoints == 0) {
				Transform[] transforms = spikes.spikesContainer.GetComponentsInChildren<Transform> ();
				foreach (Transform t in transforms) {
					if (t.gameObject.CompareTag("Spike")) {
						if (ball.transform.position.x > (t.position.x - 1)) {
							tutorial.AvoidSpikes ();
							StartCoroutine (PauseCoroutine ()); 
						}
					}
				}
			}
		}
	}

	IEnumerator PauseCoroutine (){

		Time.timeScale = 0;

		while (showSpikeTutorial) {
			if (Input.GetButtonDown ("Jump") || TouchCustom.tapped) {
				Time.timeScale = 1;
				showSpikeTutorial = false;
			} 

			yield return null;  
		}
		tutorial.Hide ();
	}
}
