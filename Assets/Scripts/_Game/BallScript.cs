using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{

	float hitPower = -15;
	float xPos = -2;
	float fullEnergy = 0.7f;
	public bool beenHit;
	public bool alive;
	public float energy;
	public float energyLoss;
	public float force;

	public BallParticles particles;

	float landHeight{
		get { return GameplayController.hills.HillHeightAtX (xPos); }//generatorScript.LandHeight (xPos); }
	}

	float yPos{
		get{return transform.position.y ;}
	}
		
	void Start (){
		GameplayController.ball = this;
		alive = false;
	}

	public void InitialSetup(){
		GetComponent<SpriteRenderer> ().enabled = true;
		energy = fullEnergy;
		transform.position = new Vector3 (xPos, 0);
	}

	void Update (){

		if (alive) {
			if (!beenHit) {
				Bounce ();
			} else {
				HitBall ();
			}

			if (yPos <= landHeight) {
				ResetBounce ();
			}
		}

	}

	void Bounce(){
		float forceTimesGameSpeed = force + (-GameplayController.speed);
		energy -= Time.smoothDeltaTime * energyLoss;

		transform.Translate (0, energy * forceTimesGameSpeed * Time.smoothDeltaTime, 0);
		//GetComponent<Rigidbody2D>().MovePosition(new Vector2( 0, energy * forceTimesGameSpeed * Time.smoothDeltaTime));
	}
		
	void HitBall (){
		transform.Translate (0, hitPower * Time.smoothDeltaTime, 0);
		//GetComponent<Rigidbody2D>().MovePosition(new Vector2(0, hitPower * Time.smoothDeltaTime));
	}

	public void ResetBounce(){
		transform.position = new Vector2 (xPos, landHeight);
		beenHit = false;
		energy = fullEnergy;
	}
		
	void OnCollisionEnter2D(Collision2D coll) {

		if (coll.gameObject.tag == "Spike") {
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
		GameplayController.instance.GameOver();
		alive = false;
		particles.CreateParticles ();
		GetComponent<SpriteRenderer> ().enabled = false;
	}

}
	