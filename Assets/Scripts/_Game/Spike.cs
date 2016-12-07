using UnityEngine;
using System.Collections;

public class Spike : MonoBehaviour {


	public static int spikeNumber;

	public bool pointSpike;
	public GameObject spikeParticleSystem;
	float offScreen, spriteBounds;

	float speed{
		get{ return GameplayController.speed ;}
	}


	// Use this for initialization
	void Start () {

		spriteBounds = gameObject.GetComponent<SpriteRenderer> ().sprite.bounds.extents.x;

		offScreen = GameplayController.zeroScreenX - spriteBounds;

		spikeParticleSystem.transform.position = new Vector3 (GameplayController.zeroScreenX, transform.position.y, 0);

		spikeNumber++;
	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (speed, 0, 0);

		DestroySpike ();
	}

	void DestroySpike(){
		if (transform.position.x < offScreen) {
			Destroy (gameObject);

			spikeParticleSystem.GetComponent<SpikeBurst> ().CreateParticles ();
			if (pointSpike) {
				GameManager.currentPoints++;
			}
		}
	}
}
