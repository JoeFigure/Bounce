using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {


	public bool pointSpike;
			
	float offScreen;

	public GameObject spikeParticleSystem;

	float spriteBounds;

	float speed{
		get{ return GameplayController.speed ;}
	}


	// Use this for initialization
	void Start () {

		spriteBounds = gameObject.GetComponent<SpriteRenderer> ().sprite.bounds.extents.x;

		offScreen = GameplayController.zeroScreenX - spriteBounds;

		spikeParticleSystem.transform.position = new Vector3 (GameplayController.zeroScreenX, transform.position.y, 0);

	}
	
	// Update is called once per frame
	void Update () {

		transform.Translate (speed, 0, 0);

		if (transform.position.x < offScreen) {
			Destroy (gameObject);

				spikeParticleSystem.GetComponent<SpikeBurst> ().CreateParticles ();
				if (pointSpike) {
					GameManager.currentPoints++;
				}
		}
	}
}
