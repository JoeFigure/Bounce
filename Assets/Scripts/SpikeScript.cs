using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {


	public bool pointSpike;

	float speed{
		get{ return GameplayController.speed ;}
	}

	public bool spike;

	public int offScreen;

	public Camera camera;

	public GameObject spikeParticleSystem;


	// Use this for initialization
	void Start () {

		Vector3 particlePosition = camera.ScreenToWorldPoint (new Vector3(0,0,0));
		spikeParticleSystem.transform.position = new Vector3 (particlePosition.x, transform.position.y, 0);
	
	}
	
	// Update is called once per frame
	void Update () {


		transform.Translate (speed, 0, 0);

		Vector3 place = camera.WorldToScreenPoint (transform.position);

		if (place.x < offScreen) {
			Destroy (gameObject);

			if (spike) {
				spikeParticleSystem.GetComponent<SpikeBurst> ().CreateParticles ();
				if (pointSpike) {
					GameManager.currentPoints++;
				}
			}
		}
	}
}
