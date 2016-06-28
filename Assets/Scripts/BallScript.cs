using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	float freq = 5;
	float height = 1.6f;
	float hitPower = -0.55f;

	float ballSize;
	float floorSize;

	float xPos = -2;

	float posCos;
	float bouncingTime;

	float halfBallSize;
	float halfFloorSize;
	float ballBase;

	float landHeight;

	bool beenHit;

	public Transform floorTrans;

	public SpikeGeneratorScript generatorScript;


	void Start ()
	{

		floorSize = floorTrans.localScale.y;
		ballSize = transform.localScale.x;
		halfBallSize = ballSize / 2;
		halfFloorSize = floorSize / 2;
		ballBase = floorTrans.position.y + halfBallSize + halfFloorSize;

		transform.position = new Vector3 (xPos, ballBase);
	}

	void Update ()
	{
		landHeight = generatorScript.LandHeight (xPos);

		print (landHeight);

		if (GameManager.instance.gameStarted) {

			if (!beenHit) {
				Bounce ();
			} else {
				HitBall ();
			}
		}

		if (transform.position.y == ballBase) {
			ResetBounce ();
		}

		TouchInput ();
		KeyboardInput ();
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