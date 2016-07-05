using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
	//Parameters
	private int meat;

	public int Meat{
		get{ return meat * 2; }
		set{ meat = value; }
	}

	float hitPower = -15;
	float energy = 0.7f; //Height
	float force = 14; //Speed
	float xPos = -2;

	float fullEnergy;
	float yPos;
	float ballSize;
	float halfBallSize;
	float landHeight;
	bool beenHit;
	float ballBase;

	public bool alive;

	public Transform floorTrans;
	public SpikeGeneratorScript generatorScript;

	void Start ()
	{
		fullEnergy = energy;

		ballSize = transform.localScale.x;
		halfBallSize = ballSize / 2;

		transform.position = new Vector3 (xPos, 0);

		alive = true;

	}

	void Update ()
	{

		yPos = transform.position.y;

		CalculateLandHeight ();

		if (transform.position.y == ballBase) {
			ResetBounce ();
		}

		if (GameManager.instance.gameStarted) {

			if(alive){

				if (!beenHit) {
					Bounce ();
				} else {
					HitBall ();
				}
			}
		}

		TouchInput ();
		KeyboardInput ();
	}

	void CalculateLandHeight(){
		landHeight = generatorScript.LandHeight (xPos);
		ballBase = landHeight + halfBallSize ;
	}

	void Bounce(){

		energy -= Time.deltaTime * 2;

		if (energy < 0) {
			if (yPos <= ballBase + 0.2f ) {
				transform.position = new Vector3 (transform.position.x, ballBase);
				energy = fullEnergy;
			}
		}
		transform.Translate (0, energy * force * Time.deltaTime, 0);
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
		

	void HitBall ()
	{
		transform.Translate (0, hitPower * Time.deltaTime, 0);

		if (yPos <= ballBase + halfBallSize) {
			transform.position = new Vector3 (transform.position.x, ballBase);
		}
	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Spike") {
			Dead();
		}

		if (coll.gameObject.tag == "Hill") {
			if(transform.position.x < coll.collider.bounds.min.x){
				Dead ();
				//print("yPos: " + (transform.position.y).ToString() + " plat: " + (ballBase).ToString() );

			}
		}
	}

	public void Dead(){
		GameManager.instance.CurrentState (GameStates.GameOver);
		alive = false;
	}


	public void ResetBounce(){
		beenHit = false;
		energy = fullEnergy;
	}
}
	