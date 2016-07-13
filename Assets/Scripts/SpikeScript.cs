using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour {


	float _speed = -5;

	public bool pointSpike;

	public float speedMult {
		get { 
			float percOfFullSpeed = GameManager.instance.gameTimePercentOfFullSpeed;
			float multiplier = 1.5f;
			return (percOfFullSpeed * multiplier) + 1;
		}
	}

	public float speed{
		get{ 
			return (_speed * speedMult) ;
			}
		set{ _speed = value ; }
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


		if (GameManager.instance.ball.alive) {
			transform.Translate (speed * Time.deltaTime, 0, 0);
		} 

		Vector3 place = camera.WorldToScreenPoint (transform.position);

		if (place.x < offScreen) {
			Destroy (gameObject);

			if (spike) {
				spikeParticleSystem.GetComponent<SpikeBurst> ().CreateParticles ();
				if (pointSpike) {
					GameManager.instance.currentPoints++;
				}
			}
		}
	}
}
