using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	float hitPower = -15;
	float xPos = -2;
	float size = 0.6f;

	float fullEnergy;
	bool beenHit;
	float scaleMult;
	public bool alive;

	public float energy{ get; set; }
	public float energyLoss{ get; set; }
	public float force{ get; set; }

	public float ScaleMult{
		get{ return scaleMult ;}
		set {
			scaleMult = value ;
			ResizeBall ();
		}
	}

	float landHeight{
		get { return generatorScript.LandHeight (xPos); }
	}
		
	float ballBase{

		get{ return landHeight + halfBallSize; }
	}

	float halfBallSize {
		get{ return  transform.localScale.x / 2 ;}
	}

	float yPos{
		get{return transform.position.y ;}
	}
		
	public SpikeGeneratorScript generatorScript;

	void Start ()
	{
		energy = 0.7f;
		energyLoss = 2;
		force = 13;

		scaleMult = 1;
		fullEnergy = energy;
		transform.position = new Vector3 (xPos, 0);
		alive = false;
		transform.localScale = transform.localScale * ScaleMult;
		ResizeBall ();
	}

	void Update ()
	{
		if (transform.position.y == ballBase) {
			ResetBounce ();
		}


		if (alive) {
			if (!beenHit) {
				Bounce ();
			} else {
				HitBall ();
			}
		}

		TouchInput ();
		KeyboardInput ();
	}

	public void ResizeBall(){
		transform.localScale = new Vector3( size * scaleMult, size * scaleMult, 0) ;
	}

	void Bounce(){

		float forceTimesGameSpeed = force + (-GameManager.instance.speed);

		energy -= Time.deltaTime * energyLoss;
		transform.Translate (0, energy * forceTimesGameSpeed * Time.deltaTime, 0);

		if (yPos <= ballBase) {
			transform.position = new Vector2 (xPos, ballBase);
		}

	}

	public void ResetBounce(){
		beenHit = false;
		energy = fullEnergy;
	}

	void HitBall ()
	{
		transform.Translate (0, hitPower * Time.deltaTime, 0);

		if (yPos <= ballBase + halfBallSize) {
			transform.position = new Vector2 (transform.position.x, ballBase);
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
		
	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Spike") {
			//print ("spike");
			Dead();
		}

		if (coll.gameObject.tag == "Hill") {

			if (transform.position.x < coll.collider.bounds.min.x
				
				&& coll.collider.bounds.max.y > landHeight) {

				Dead ();
			} else {
				ResetBounce ();
			}
		}
	}

	public void Dead(){
		GameManager.instance.CurrentState (GameStates.GameOver);
		alive = false;
	}



}
	