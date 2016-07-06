using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	float hitPower = -15;
	public float energy = 0.7f; //Height
	public float force = 14; //Speed
	public float bounceHeight = 2;
	float xPos = -2;
	float size = 0.7f;


	float scaleMult;

	public float ScaleMult{
		get{ return scaleMult ;}
		set {
			scaleMult = value ;
			ResizeBall ();
		}
	}

	float fullEnergy;
	float landHeight;
	bool beenHit;
	float ballBase;
	public bool alive;

	public float ballSize {
		get{ return  transform.localScale.x ;}
	}

	public float halfBallSize {
		get{ return  ballSize / 2 ;}
	}

	public float yPos{
		get{return transform.position.y ;}
	}
		
	public Transform floorTrans;
	public SpikeGeneratorScript generatorScript;

	void Start ()
	{
		scaleMult = 1;

		fullEnergy = energy;

		transform.position = new Vector3 (xPos, 0);

		alive = true;

		transform.localScale = transform.localScale * ScaleMult;

		ResizeBall ();
	}

	void Update ()
	{

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

	public void ResizeBall(){
		transform.localScale = new Vector3( size * scaleMult, size * scaleMult, 0) ;
	}

	void CalculateLandHeight(){
		landHeight = generatorScript.LandHeight (xPos);
		ballBase = landHeight + halfBallSize ;
	}

	void Bounce(){

		energy -= Time.deltaTime * bounceHeight;

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
	