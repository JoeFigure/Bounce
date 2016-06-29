using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{


	float hitPower = -15;

	float energy = 0.7f; //Height
	float force = 14; //Speed

	float fullEnergy;

	float xPos = -2;
	float yPos;

	float ballSize;
	float floorSize;

	public GameObject hitText;

	float halfBallSize;
	float halfFloorSize;

	//float frontOfBallY;
	//float landHeightUnderFrontOfBall;

	float landHeight;

	bool beenHit;

	public Transform floorTrans;

	public SpikeGeneratorScript generatorScript;

	float ballBase;

	//bool alive;


	void Start ()
	{
		fullEnergy = energy;

		hitText.SetActive (false);

		floorSize = floorTrans.localScale.y;
		ballSize = transform.localScale.x;
		halfBallSize = ballSize / 2;
		halfFloorSize = floorSize / 2;

		//ballBase = landHeight + halfBallSize;

		transform.position = new Vector3 (xPos, 0);

		//float poo = GetComponent<Collider2D>().bounds.

	}

	void Update ()
	{

		//frontOfBallY = xPos + halfBallSize;
		//landHeightUnderFrontOfBall = generatorScript.LandHeight (frontOfBallY) + 0.5f;


		yPos = transform.position.y;

		landHeight = generatorScript.LandHeight (xPos);

		ballBase = landHeight + halfBallSize ;




		if (transform.position.y == ballBase) {
			ResetBounce ();
		}


		if (GameManager.instance.gameStarted) {

			if (!beenHit) {
				Bounce ();
			} else {
				HitBall ();
			}
		}



		TouchInput ();
		KeyboardInput ();

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

		//CheckForWallHit ();
			
	}

	void CheckForWallHit(){
		
		if (yPos <= ballBase - 0.2f){

			GameManager.instance.ResetGame ();
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
		

	void HitBall ()
	{
		transform.Translate (0, hitPower * Time.deltaTime, 0);

		if (yPos <= ballBase + halfBallSize) {

			transform.position = new Vector3 (transform.position.x, ballBase);
		}


	}


	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "Spike") {
			GameManager.instance.ResetGame ();
		}

		if (coll.gameObject.tag == "Hill") {
			if(transform.position.x < coll.collider.bounds.min.x){
			//if(yPos < landHeightUnderFrontOfBall + halfBallSize) {
				//print ("oooo");
				GameManager.instance.ResetGame ();
				//print("yPos: " + (transform.position.y).ToString() + " plat: " + (ballBase).ToString() );

			}
		}
	}


	public void ResetBounce(){
		beenHit = false;
		energy = fullEnergy;
	}
}
	