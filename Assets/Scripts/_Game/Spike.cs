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


	void Start () {
		
		spriteBounds = gameObject.GetComponent<SpriteRenderer> ().sprite.bounds.extents.x;

		offScreen = GameplayController.zeroScreenX - spriteBounds;

		spikeParticleSystem.transform.position = new Vector3 (GameplayController.zeroScreenX, transform.position.y, 0);

	}

	void Update () {

		RewardPoint();
	}

	public void RewardPoint(){
		if (transform.position.x < offScreen) {
			spikeParticleSystem.GetComponent<SpikeBurst> ().CreateParticles ();
			if (pointSpike) {
				GameManager.currentPoints++;
				pointSpike = false;
			}
		}
	}

}
