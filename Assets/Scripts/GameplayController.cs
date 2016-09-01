using UnityEngine;
using System.Collections;

public class GameplayController : MonoBehaviour {

	public static BallScript ball;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		TouchInput ();
		KeyboardInput ();
	}

	void TouchInput ()
	{
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {
			case TouchPhase.Began:
				ball.beenHit = true;
				break;
			}
		}	
	}

	void KeyboardInput(){
		if (Input.GetButtonDown ("Jump")) {
			ball.beenHit = true;
		}
	}
}
