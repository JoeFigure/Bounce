using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	float freq = 5;
	float height = 1.6f;
	float hitPower = -0.55f;

	float ballSize;
	float floorSize = 1;

	float xPos = -2;

	float posCos;
	float bouncingTime;

	float halfBallSize;
	float halfFloorSize;
	float ballBase;

	bool beenHit;

	public bool gameStarted;

	//public GameObject hitWords;

	public Transform floorTrans;


	void Start ()
	{

		gameStarted = false;

		ballSize = transform.localScale.x;
		halfBallSize = ballSize / 2;
		halfFloorSize = floorSize / 2;
		ballBase = floorTrans.position.y + halfBallSize + halfFloorSize;

		transform.position = new Vector3 (xPos, ballBase);
	}

	void Update ()
	{

		if (gameStarted) {

			if (!beenHit) {
				Bounce ();
			} else {
				HitBall ();
			}
		}

		TouchInput ();
		KeyboardInput ();

		if (transform.position.y == ballBase) {
			ResetBounce ();
		}
	}

	void TouchInput ()
	{

		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);
			switch (touch.phase) {
			case TouchPhase.Began:
				beenHit = true;
				break;
			}
		}	
	}

	void KeyboardInput(){
		if (Input.GetButtonDown ("Jump")) {
			beenHit = true;
		}
	}

	void Bounce ()
	{

		bouncingTime += Time.deltaTime;

		posCos = Mathf.Abs (Mathf.Sin (freq * (bouncingTime)) * height);
		transform.position = new Vector2 (xPos, ballBase + posCos);
	}

	void HitBall ()
	{
		transform.Translate (0, hitPower, 0);

		float yPos = transform.position.y;

		if (yPos <= ballBase +0.5f) {

			transform.position = new Vector3 (transform.position.x, ballBase);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {

		GameManager.instance.ResetGame ();
	}


	public void ResetBounce(){
		beenHit = false;
		bouncingTime = 0;
	}
}

// ALT BOUNCE - transform.localPosition = new Vector3(0, Mathf.PingPong( Mathf.Sin(Time.time*4),4),0);